using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Represents a fact as an item in the inventory.
    /// </summary>
    [CreateAssetMenu(menuName = "Inventory/Fact")]
    public class Fact : Item
    {
        [field: SerializeField, Tooltip("A list of all facts that are required to unlock this fact; can be empty")]
        public Fact[] DependsOn { get; private set; }

        private void OnValidate()
        {
            if (DependsOn == null)
            {
                DependsOn = Array.Empty<Fact>();
                return;
            }

            if (DependsOn.Length == 0)
                return;

            var hierarchy = new List<Fact> { this };
            
            if (IsCircular(hierarchy, DependsOn))
                Debug.LogError("Circular dependency detected!", this);
        }

        private static bool IsCircular([DisallowNull] List<Fact> hierarchy, Fact[] dependentFacts)
        {
            if (dependentFacts == null || dependentFacts.Length == 0)
                return false;

            foreach (var dependentFact in dependentFacts)
            {
                if (dependentFact == null)
                    continue;
                
                if (hierarchy.Contains(dependentFact))
                    return true;
                
                hierarchy.Add(dependentFact);

                if (IsCircular(hierarchy, dependentFact.DependsOn))
                    return true;

                hierarchy.Remove(dependentFact);
            }

            return false;
        }
    }
}
