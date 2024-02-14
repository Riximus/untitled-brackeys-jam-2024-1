using System;
using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueParent;
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float turnSpeed = 2f; // The speed at which the player turns to face the NPC

        [Header("Player")]
        [SerializeField] private PlayerController playerController; // TODO: Reference it differently
        [SerializeField] private Transform playerCamera; // TODO: Reference it differently

        private List<DialogueString> _dialogueList;
        private Transform _playerCamera;

        private int _currentDialogueIndex = 0;
        
        private Button _option1Button;
        private Button _option2Button;
        private Label _dialogueText;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            
            _option1Button = root.Q<Button>("option-1");
            _option2Button = root.Q<Button>("option-2");
            _dialogueText = root.Q<Label>("dialogue-text");
        }

        private void Start()
        {
            dialogueParent.SetActive(false);
        }

        public void DialogueStart(List<DialogueString> dialogueStrings, Transform npc)
        {
            dialogueParent.SetActive(true);
            playerController.enabled = false;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartCoroutine(TurnCameraTowardsNpc(npc));
            
            _dialogueList = dialogueStrings;
            _currentDialogueIndex = 0;

            DisableButtons();
            
            StartCoroutine(PrintDialogue());
        }

        private void DisableButtons()
        {
            _option1Button.SetEnabled(false);
            _option2Button.SetEnabled(false);
            
            _option1Button.visible = false;
            _option2Button.visible = false;
        }

        private IEnumerator TurnCameraTowardsNpc(Transform npc)
        {
            Quaternion startRotation = playerCamera.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(npc.position - playerCamera.position);
            
            float elapsedTime = 0;
            while (elapsedTime < 1f)
            {
                playerCamera.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
                elapsedTime += Time.deltaTime * turnSpeed;
                yield return null;
            }
            
            playerCamera.rotation = targetRotation;
        }

        private bool _optionSelected = false;
        private IEnumerator PrintDialogue()
        {
            while (_currentDialogueIndex < _dialogueList.Count)
            {
                DialogueString line = _dialogueList[_currentDialogueIndex];
                
                line.startDialogueEvent?.Invoke();
                
                if (line.isQuestion)
                {
                    yield return StartCoroutine(TypeText(line.text));

                    _option1Button.SetEnabled(true);
                    _option2Button.SetEnabled(true);
                    
                    _option1Button.text = line.answerOption1;
                    _option2Button.text = line.answerOption2;
                    
                    _option1Button.visible = true;
                    _option2Button.visible = true;
                    
                    _option1Button.clicked += () => HandleOptionSelected(line.nextDialogue1);
                    _option2Button.clicked += () => HandleOptionSelected(line.nextDialogue2);

                    yield return new WaitUntil(() => _optionSelected);
                }
                else
                {
                    yield return StartCoroutine(TypeText(line.text));
                }
                
                line.endDialogueEvent?.Invoke();
                
                _optionSelected = false;
            }

            DialogueStop(); // TODO: https://youtu.be/BEaOHWkZhtE?si=12I340BuZDSiqUw7&t=1224
        }
    }
}