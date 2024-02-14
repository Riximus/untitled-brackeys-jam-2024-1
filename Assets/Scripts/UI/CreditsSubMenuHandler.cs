using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace UI
{
    /// <summary>
    /// Handles showing the credits using the <see cref="CreditMention"/> assets.
    /// </summary>
    public class CreditsSubMenuHandler : MonoBehaviour, ISubMenuHandler
    {
        [SerializeField, Tooltip("Should contain all credit mentions for this project")]
        private CreditMention[] creditMentions;

        [DisallowNull, MaybeNull] private VisualElement _content;
        [DisallowNull, MaybeNull] private Button _back;

        /// <summary>
        /// Navigates to the containing menu, if the back button was clicked.
        /// </summary>
        public event Action NavigateBackRequested;

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <remarks>
        /// The credits menu has no mutable state, so it doesn't need to cancel anything.
        /// </remarks>
        public void Cancel()
        {
        }

        private void OnEnable()
        {
            var uiDocument = this.RequireComponent<UIDocument>();
            var root = uiDocument.rootVisualElement.Q<CreditsSubMenu>();
            
            if (root == null)
                throw new InvalidOperationException(
                    $"Root element of {gameObject.name}'s {nameof(UIDocument)} is null!");

            _content = root.RequireElement<VisualElement>("content");
            _back = root.RequireElement<Button>("back-button");
            _back.clicked += OnBackClicked;
        }

        private void Start()
        {
            if (_content == null)
                throw new InvalidOperationException($"{nameof(Start)} was called before {nameof(OnEnable)}!");
            
            foreach (var creditMention in creditMentions ?? Array.Empty<CreditMention>())
            {
                if (creditMention == null)
                    continue;
                
                var mentionContainer = new VisualElement();
                mentionContainer.AddToClassList("mention-container");
                var titleText = string.Join(", ", creditMention.Roles);
                var creditedName = new Label(creditMention.CreditedName);
                creditedName.AddToClassList("mention-name");
                mentionContainer.Add(creditedName);
                var title = new Label(titleText);
                title.AddToClassList("mention-title");
                mentionContainer.Add(title);
                var linkContainer = new VisualElement();

                foreach (var linkText in creditMention.SocialMediaLinks)
                {
                    if (linkText == null)
                        continue;

                    var socialMediaLink = new Label(linkText);
                    socialMediaLink.AddToClassList("mention-link");
                    linkContainer.Add(socialMediaLink);
                }
                

                mentionContainer.Add(linkContainer);
                _content.Add(mentionContainer);
            }
        }

        private void OnDisable()
        {
            if (_back == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} was called before {nameof(OnEnable)}!");

            _back.clicked -= OnBackClicked;
        }

        private void OnBackClicked()
        {
            NavigateBackRequested?.Invoke();
        }
    }
}