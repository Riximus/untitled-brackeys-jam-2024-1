using System;
using System.Diagnostics.CodeAnalysis;
using Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class JournalEntry : VisualElement
    {
        public JournalEntry([DisallowNull] Fact fact)
        {
            if (fact == null)
                throw new ArgumentNullException(nameof(fact));

            var container = new VisualElement
            {
                name = "container", style =
                {
                    flexGrow = 1,
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row)
                }
            };
            Add(container);
            var imageContainer = new VisualElement { name = "image-container", style = { flexGrow = 1 } };
            container.Add(imageContainer);
            var itemIcon = new Image { name = "item-icon" };
            imageContainer.Add(itemIcon);
            var textContainer = new VisualElement { name = "text-container", style = { flexGrow = 1 } };
            container.Add(textContainer);
            var itemName = new Label
            {
                tabIndex = -1, text = fact.name, parseEscapeSequences = true, displayTooltipWhenElided = true,
                name = "item-name",
                style =
                {
                    fontSize = 14,
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold)
                }
            };
            textContainer.Add(itemName);
            var itemDescription = new Label
            {
                tabIndex = -1, text = fact.Description, parseEscapeSequences = true, displayTooltipWhenElided = true,
                name = "item-description"
            };
            textContainer.Add(itemDescription);

            if (fact.Icon != null)
                itemIcon.image = fact.Icon;
            else
                itemIcon.AddToClassList("disabled");
        }
    }
}