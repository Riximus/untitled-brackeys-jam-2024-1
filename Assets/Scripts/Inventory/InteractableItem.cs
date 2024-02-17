using Input;
using UnityEngine;

namespace Inventory
{
    public class InteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private InventoryHandler inventoryHandler;
        [SerializeField] private ItemKind itemKind;
        
        /// <inheritdoc />
        public void Interact()
        {
            inventoryHandler.AddToInventory(itemKind);
            Destroy(gameObject);
        }
    }
}