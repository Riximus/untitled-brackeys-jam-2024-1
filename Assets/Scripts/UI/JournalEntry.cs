using System;
using System.Diagnostics.CodeAnalysis;
using Inventory;
using UnityEngine.UIElements;
using Util;

public class JournalEntry : VisualElement
{
    public JournalEntry([DisallowNull] Fact fact)
    {
        if (fact == null)
            throw new ArgumentNullException(nameof(fact));
        
        UxmlUtil.LoadUxml(this);
        var itemName = this.RequireElement<Label>("item-name");
        itemName.text = fact.name;
        var itemDescription = this.RequireElement<Label>("item-description");
        itemDescription.text = fact.Description;
        var itemIcon = this.RequireElement<Image>("item-icon");

        if (fact.Icon != null)
            itemIcon.image = fact.Icon;
        else
            itemIcon.AddToClassList("disabled");
    }
}