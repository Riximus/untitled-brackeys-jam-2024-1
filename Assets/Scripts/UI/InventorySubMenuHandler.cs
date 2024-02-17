using System;
using System.Diagnostics.CodeAnalysis;
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
        [DisallowNull, NotNull] private InventorySubMenu _inventorySubMenu = default!;
        [DisallowNull, NotNull] private Button _backButton = default!;

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
            _backButton = _inventorySubMenu.RequireElement<Button>("back-button");
        }

        private void OnEnable()
        {
            _backButton.clicked += OnBackButtonClicked;
        }

        private void OnDisable()
        {
            _backButton.clicked -= OnBackButtonClicked;
        }

        private void OnBackButtonClicked()
        {
            NavigateBackRequested?.Invoke();
        }
    }
}