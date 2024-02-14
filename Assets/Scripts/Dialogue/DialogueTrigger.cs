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
        [SerializeField] private List<DialogueString> dialogueStrings = new List<DialogueString>();
        [SerializeField] private Transform npcTransform;

        private bool _hasSpoken = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_hasSpoken)
            {
                other.gameObject.GetComponent<DialogueManager>().DialogueStart(dialogueStrings, npcTransform);
                _hasSpoken = true;
            }
        }

        public void Interact()
        {
            Debug.Log("Interacting with dialogue trigger");
        }
    }
    
    [System.Serializable]
    public class DialogueString
    {
        public string text; // The text to display what the NPC is saying
        public bool isEndOfDialogue; // Whether or not this is the end of the dialogue
        
        [Header("Branch")]
        public bool isQuestion; // Whether or not this is a question
        
        // Trying out these ones to see
        public string[] answerOptions; // The options for the player to choose from
        public DialogueString[] nextDialogue; // The next dialogue to display based on the player's choice

        public string answerOption1;
        public string answerOption2;
        
        public DialogueString nextDialogue1; // The next dialogue to display based on answerOption1
        public DialogueString nextDialogue2; // The next dialogue to display based on answerOption2
        
        [Header("Triggered Events")]
        public UnityEvent startDialogueEvent; // The event to trigger when the dialogue starts
        public UnityEvent endDialogueEvent; // The event to trigger when the dialogue ends

    }
}