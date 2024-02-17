using System.Linq;
using Dialogue;
using Inventory;
using UnityEngine;

public class DialogueInventoryCheck : MonoBehaviour
{
    [SerializeField] private InventoryHandler inventoryHandler;
    [SerializeField] private ItemKind requiredItem;
    [SerializeField] private uint requiredItemCount;
    private static int _villagersCalled = 0;
    private DialogueTrigger _dialogueTrigger;

    public void UpdateDialogIfItemsWereFound()
    {
        if (!inventoryHandler.HasAmountOfItems(requiredItem, requiredItemCount))
            return;
        
        if (requiredItem == ItemKind.MagicFlower)
            _villagersCalled++;
        
        _dialogueTrigger.hasDialogueTriggered = true;
        _dialogueTrigger.isLoopingDialogue = false;
    }

    public void UpdateDialogIfVillagersCalled()
    {
        if (_villagersCalled < 2)
            return;
        _dialogueTrigger.hasDialogueTriggered = true;
        _dialogueTrigger.isLoopingDialogue = false;
    }

    private void Awake()
    {
        var dialogueTriggers = GetComponents<DialogueTrigger>();
        _dialogueTrigger = dialogueTriggers.FirstOrDefault(
            dialogueTrigger => dialogueTrigger.GetDialogueStrings().Length == 1);
    }
}