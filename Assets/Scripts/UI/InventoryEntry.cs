using System;
using System.Diagnostics.CodeAnalysis;
using Inventory;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace UI
{
    public class InventoryEntry : VisualElement
    {
        public InventoryEntry([DisallowNull] Item item, uint amount)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var container = new VisualElement
            {
                name = "container",
                style = { flexGrow = 1, flexDirection = FlexDirection.Row }
            };
            Add(container);
            var imageContainer = new VisualElement { name = "image-container", style = { flexGrow = 0 } };
            container.Add(imageContainer);
            var itemIcon = new Image { name = "item-icon" };
            imageContainer.Add(itemIcon);
            var textContainer = new VisualElement { name = "text-container", style = { flexGrow = 1 } };
            container.Add(textContainer);
            textContainer.Add(new Label
            {
                tabIndex = -1,
                text = "{item.name}",
                parseEscapeSequences = true,
                displayTooltipWhenElided = true,
                name = "item-name",
                style = { unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold), fontSize = 14 }
            });
            textContainer.Add(new Label
            {
                tabIndex = -1,
                text = "{item.description}",
                parseEscapeSequences = true,
                displayTooltipWhenElided = true,
                name = "item-description"
            });
            var itemName = this.RequireElement<Label>("item-name");
            itemName.text = $"{item.Name} ({amount})";
            var itemDescription = this.RequireElement<Label>("item-description");
            itemDescription.text = item.Description;

            if (item.Icon != null)
                itemIcon.image = item.Icon;
            else
                itemIcon.AddToClassList("disabled");
        }
    }
}