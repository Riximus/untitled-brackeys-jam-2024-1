﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Util;
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
        [SerializeField] private PlayerInput playerInput;

        private List<DialogueString> _dialogueList;
        private Transform _playerCamera;

        private int _currentDialogueIndex = 0;
        private bool _optionSelected = false;

        [DisallowNull, MaybeNull] private VisualElement _root;
        private VisualElement _dialogueWindow;
        private VisualElement _dialogueContainer;
        private VisualElement _textContainer;
        private VisualElement _optionsContainer;
        private Button _option1Button;
        private Button _option2Button;
        private Label _dialogueText;

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
            playerInput.enabled = false;

            StartCoroutine(TurnCameraTowardsNpc(npc));
            
            _dialogueList = dialogueStrings;
            _currentDialogueIndex = 0;
            
            var uiDocument = dialogueParent.GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            _root = root
                    ?? throw new InvalidOperationException(
                        $"{nameof(UIDocument)} component on {gameObject.name} has no root element!");
            
            _dialogueWindow = _root?.RequireElement<VisualElement>("dialogue-window");
            _dialogueContainer = _root?.RequireElement<VisualElement>("dialogue-container");
            _textContainer = _root?.RequireElement<VisualElement>("text-container");
            _optionsContainer = _root?.RequireElement<VisualElement>("options-container");
            _option1Button = _root?.RequireElement<Button>("option-1");
            _option2Button = _root?.RequireElement<Button>("option-2");
            _dialogueText = _root?.RequireElement<Label>("dialogue-text");

            if (_option1Button == null || _option2Button == null || _dialogueText == null)
            {
                Debug.LogError("One or more UI elements were not found. Check the names and ensure UIDocument is correctly set up.");
            }

            DisableButtons();
            
            StartCoroutine(PrintDialogue());
        }

        private void DisableButtons()
        {
            if (_option1Button != null && _option2Button != null)
            {
                _option1Button.SetEnabled(false);
                _option2Button.SetEnabled(false);
        
                _option1Button.visible = false;
                _option2Button.visible = false;
            }
            else
            {
                Debug.LogError("Attempted to disable buttons that are not initialized.");
            }
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


        private IEnumerator PrintDialogue()
        {
            while (_currentDialogueIndex < _dialogueList.Count)
            {
                DialogueString line = _dialogueList[_currentDialogueIndex];
                
                line.startDialogueEvent?.Invoke();
                
                line.indexReadOnly = _currentDialogueIndex.ToString() + ": " + line.text;
                
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

            DialogueStop();
        }
        
        private void HandleOptionSelected(int indexNextDialogue)
        {
            _optionSelected = true;
            DisableButtons();
            
            _currentDialogueIndex = indexNextDialogue;
        }
        
        private IEnumerator TypeText(string text)
        {
            _dialogueText.text = "";
            foreach (char letter in text) // Maybe toCharArray()?
            {
                _dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (!_dialogueList[_currentDialogueIndex].isQuestion)
            {
                yield return new WaitUntil(() => UnityEngine.Input.GetMouseButtonDown(0));
            }

            if (_dialogueList[_currentDialogueIndex].isEndOfDialogue)
            {
                DialogueStop();
            }
            
            _currentDialogueIndex++;
        }
        
        private void DialogueStop()
        {
            StopAllCoroutines();
            _dialogueText.text = "";
            dialogueParent.SetActive(false);
            
            playerController.enabled = true;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerInput.enabled = true;
        }
    }
}