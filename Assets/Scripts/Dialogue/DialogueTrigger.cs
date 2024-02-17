using System;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] public bool hasDialogueTriggered = false;
        [SerializeField] public bool isLoopingDialogue = false;
        [SerializeField] private List<DialogueString> dialogueStrings = new List<DialogueString>();
        [SerializeField] private Transform npcTransform;
        //[SerializeField] private GameObject player;
        
        private void StartDialogue()
        {
            if (!hasDialogueTriggered)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.GetComponent<DialogueManager>().DialogueStart(dialogueStrings, npcTransform);
                    if (!isLoopingDialogue)
                    {
                        hasDialogueTriggered = true;
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartDialogue();
            }
        }

        public void Interact()
        {
            Debug.Log("Interacting with dialogue trigger");
            StartDialogue();
        }
    }
    
    [System.Serializable]
    public class DialogueString
    {
        public string indexReadOnly; // The index of the dialogue
        [TextArea(3,3)]
        public string text; // The text to display what the NPC is saying
        public bool isEndOfDialogue; // Whether or not this is the end of the dialogue
        
        [Header("Branch")]
        public bool isQuestion; // Whether or not this is a question
        public string answerOption1;
        public string answerOption2;
        public int nextDialogue1; // The next dialogue to display based on answerOption1
        public int nextDialogue2; // The next dialogue to display based on answerOption2
                
        // Trying out these ones to see
        //public string[] answerOptions; // The options for the player to choose from
        //public DialogueString[] nextDialogue; // The next dialogue to display based on the player's choice
        
        [Header("Triggered Events")]
        public UnityEvent startDialogueEvent; // The event to trigger when the dialogue starts
        public UnityEvent endDialogueEvent; // The event to trigger when the dialogue ends

    }
}