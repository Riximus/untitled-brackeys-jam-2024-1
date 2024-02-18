using System.Diagnostics.CodeAnalysis;
using Inventory;
using UnityEngine;
using UnityEngine.UIElements;

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
            Add(new Label
            {
                tabIndex = -1,
                text = "Inventory",
                parseEscapeSequences = true,
                displayTooltipWhenElided = true,
                name = "title",
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.UpperCenter),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                    fontSize = 26
                }
            });
            _itemContainer = new VisualElement
            {
                style = { flexGrow = 1 }
            };
            Add(_itemContainer);
            _itemContainer.Add(new Label
            {
                tabIndex = -1,
                text = "Items",
                parseEscapeSequences = true,
                displayTooltipWhenElided = true,
                name = "sub-title",
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.UpperCenter),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                    fontSize = 18
                }
            });
            _itemContainer.Add(new ScrollView { name = "item-list" });
            _factContainer = new VisualElement { style = { flexGrow = 1 } };
            Add(_factContainer);
            _factContainer.Add(new Label
            {
                tabIndex = -1, text = "Facts", parseEscapeSequences = true, displayTooltipWhenElided = true,
                name = "sub-title"
            });
            _factContainer.Add(new ScrollView { name = "fact-list" });
            Add(new Button { text = "Back", parseEscapeSequences = true, displayTooltipWhenElided = true, name = "back-button"});
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