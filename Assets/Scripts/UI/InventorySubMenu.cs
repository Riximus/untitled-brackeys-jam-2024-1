using System.Diagnostics.CodeAnalysis;
using Inventory;
using UnityEngine.UIElements;
using Util;

namespace UI
{
    /// <summary>
    /// Displays a list of all obtained facts.
    /// </summary>
    public class InventorySubMenu : VisualElement
    {
        [DisallowNull, NotNull] private readonly VisualElement _itemContainer;
        [DisallowNull, NotNull] private readonly VisualElement _factContainer;

        public InventorySubMenu()
        {
            UxmlUtil.LoadUxml(this);
            _itemContainer = this.RequireElement<VisualElement>("item-list");
            _factContainer = this.RequireElement<VisualElement>("fact-list");
        }

        public void AddItem([DisallowNull] Item item, uint amount)
        {
            _itemContainer.Add(new InventoryEntry(item, amount));
        }
        
        public void ClearItems()
        {
            _itemContainer.Clear();
        }

        public void AddFact([DisallowNull] Fact fact)
        {
            _factContainer.Add(new JournalEntry(fact));
        }

        public void ClearFacts()
        {
            _factContainer.Clear();
        }

        /// <summary>
        /// Required class to allow <see cref="InventorySubMenu"/> to be used as a visual element in UXML files.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<InventorySubMenu>
        {
        }
    }
}