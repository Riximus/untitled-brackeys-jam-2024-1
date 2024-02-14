using Input;
using UnityEngine;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Interacting with dialogue trigger");
        }
    }
}