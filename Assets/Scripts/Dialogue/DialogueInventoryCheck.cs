using System.Linq;
using Dialogue;
using Inventory;
using UnityEngine;

public class DialogueInventoryCheck : MonoBehaviour
{
    [SerializeField] private InventoryHandler inventoryHandler;
    [SerializeField] private ItemKind requiredItem;
    [SerializeField] private uint requiredNotesheetCount;
    private DialogueTrigger _dialogueTrigger;

    public void UpdateDialogIfNotesheetsWereFound()
    {
        if (inventoryHandler.HasAmountOfItems(requiredItem, requiredNotesheetCount))
        {
            _dialogueTrigger.hasDialogueTriggered = true;
            _dialogueTrigger.isLoopingDialogue = false;
        }
    }

    private void Awake()
    {
        var dialogueTriggers = GetComponents<DialogueTrigger>();
        _dialogueTrigger = dialogueTriggers.FirstOrDefault(
            dialogueTrigger => dialogueTrigger.GetDialogueStrings().Length == 1);
    }
}