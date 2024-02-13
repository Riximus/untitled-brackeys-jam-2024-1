using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Credits/Mention")]
    public class CreditMention : ScriptableObject
    {
        [SerializeField,
         Tooltip("Represents the roles this person had during development"),
         DisallowNull,
         MaybeNull]
        private string[] _roles;

        [SerializeField,
         Tooltip("Represents the social media links of the credited person"),
         DisallowNull,
         MaybeNull]
        private string[] _socialMediaLinks;
    
        [field: SerializeField, Tooltip("Represents the name of the credited person")]
        public string CreditedName { get; private set; }

        [NotNull]
        public string[] Roles => _roles ?? Array.Empty<string>();
    
        [NotNull]
        public string[] SocialMediaLinks => _socialMediaLinks ?? Array.Empty<string>();
    }
}
