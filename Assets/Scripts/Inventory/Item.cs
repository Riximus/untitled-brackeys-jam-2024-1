using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Represents an item in the inventory.
    /// </summary>
    [CreateAssetMenu(menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [field: SerializeField, Tooltip("Kind of this item")]
        public ItemKind Kind { get; private set; }
        
        [field: SerializeField, Tooltip("Short, descriptive name of this item")]
        public string Name { get; private set; }

        [field: SerializeField, Tooltip("Full description of this item")]
        public string Description { get; private set; }
        
        [field: SerializeField, Tooltip("Small icon that's displayed alongside this item")]
        public Texture Icon { get; private set; }
    }
}