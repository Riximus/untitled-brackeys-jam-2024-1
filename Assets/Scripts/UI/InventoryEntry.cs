using System;
using System.Diagnostics.CodeAnalysis;
using Inventory;
using UnityEngine.UIElements;
using Util;

public class InventoryEntry : VisualElement
{
    public InventoryEntry([DisallowNull] Item item, uint amount)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        
        UxmlUtil.LoadUxml(this);
        var itemName = this.RequireElement<Label>("item-name");
        itemName.text = $"{item.Name} ({amount})";
        var itemDescription = this.RequireElement<Label>("item-description");
        itemDescription.text = item.Description;
        var itemIcon = this.RequireElement<Image>("item-icon");

        if (item.Icon != null)
            itemIcon.image = item.Icon;
        else
            itemIcon.AddToClassList("disabled");
    }
}