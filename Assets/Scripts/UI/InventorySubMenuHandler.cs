using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace UI
{
    public class InventorySubMenuHandler : MonoBehaviour, ISubMenuHandler
    {
        [SerializeField] private Item[] allItems;
        [SerializeField] private InventoryHandler inventoryHandler;
        [SerializeField] private UIDocument uiDocument;
        private InventorySubMenu _inventorySubMenu;
        
        public event Action NavigateBackRequested;

        public void Cancel()
        {
        }

        public void Open()
        {
            _inventorySubMenu.ClearItems();
            _inventorySubMenu.ClearFacts();
            var itemAmounts = inventoryHandler.GetItemAmounts();

            foreach (var itemAmount in itemAmounts)
            {
                var item = allItems.FirstOrDefault(item => item.Kind == itemAmount.kind);
                
                if (item == null)
                    continue;

                _inventorySubMenu.AddItem(item, itemAmount.amount);
            }

            foreach (var fact in inventoryHandler.GetFacts())
                if (fact != null)
                    _inventorySubMenu.AddFact(fact);
        }

        private void Awake()
        {
            _inventorySubMenu = uiDocument.rootVisualElement.RequireElement<InventorySubMenu>("inventory-sub-menu");
        }
    }
}