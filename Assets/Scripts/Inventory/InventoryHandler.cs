using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class InventoryHandler : MonoBehaviour
    {
        [DisallowNull, NotNull] private readonly Dictionary<ItemKind, uint> _items = new();
        [DisallowNull, NotNull] private readonly List<Fact> _facts = new();

        public void AddToInventory(ItemKind item)
        {
            if (!_items.TryAdd(item, 1))
                _items[item] += 1;
        }
        
        public void AddMagicFlowerToInventory()
        {
            AddToInventory(ItemKind.MagicFlower);
        }

        public void AddToInventory(Fact fact)
        {
            _facts.Add(fact);
        }

        public List<(ItemKind kind, uint amount)> GetItemAmounts()
        {
            return _items
                .Select(pair => (pair.Key, pair.Value))
                .ToList();
        }

        [return: NotNull]
        public List<Fact> GetFacts()
        {
            return _facts;
        }

        public bool HasAmountOfItems(ItemKind item, uint amount)
        {
            return _items.TryGetValue(item, out var itemCount)
                   && itemCount == amount;
        }

        public bool HasFact(Fact fact)
        {
            return _facts.Contains(fact);
        }

        public void RemoveAll(ItemKind item)
        {
            _items.Remove(item);
        }
    }
}