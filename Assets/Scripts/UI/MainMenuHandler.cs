using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(OptionsSubMenuHandler))]
    public class MainMenuHandler : MonoBehaviour
    {
        [DisallowNull, MaybeNull] private VisualElement _menuItemContainer;
        [DisallowNull, MaybeNull] private OptionsSubMenuHandler _optionsSubMenuHandler;
        [DisallowNull, MaybeNull] private CreditsSubMenuHandler _creditsSubMenuHandler;
        [DisallowNull, MaybeNull] private VisualElement _root;
        [DisallowNull, MaybeNull] private Button _creditsButton;
        [DisallowNull, MaybeNull] private Button _optionsButton;
        [DisallowNull, MaybeNull] private Button _quitGameButton;
        [DisallowNull, MaybeNull] private OptionsSubMenu _optionsSubMenu;
        [DisallowNull, MaybeNull] private CreditsSubMenu _creditsSubMenu;

        private void Awake()
        {
            _optionsSubMenuHandler = this.RequireComponent<OptionsSubMenuHandler>();
            _creditsSubMenuHandler = this.RequireComponent<CreditsSubMenuHandler>();
            var uiDocument = this.RequireComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            _root = root
                    ?? throw new InvalidOperationException(
                        $"{nameof(UIDocument)} component on {gameObject.name} has no root element!");
        }

        private void OnEnable()
        {
            if (_optionsSubMenuHandler == null || _creditsSubMenuHandler == null || _root == null)
                throw new InvalidOperationException($"{nameof(OnEnable)} called before {nameof(Awake)}!");

            _optionsSubMenuHandler.NavigateBackRequested += OnNavigateBackRequested;
            _creditsSubMenuHandler.NavigateBackRequested += OnNavigateBackRequested;
            _creditsButton = _root.RequireElement<Button>("credits-button");
            _optionsButton = _root.RequireElement<Button>("options-button");
            _quitGameButton = _root.RequireElement<Button>("quit-game-button");
            _menuItemContainer = _root.RequireElement<VisualElement>("menu-item-container");
            _optionsSubMenu = _root.RequireElement<OptionsSubMenu>("options-sub-menu");
            _creditsSubMenu = _root.RequireElement<CreditsSubMenu>("credits-sub-menu");
            _creditsButton.clicked += OnCreditsButtonClicked;
            _optionsButton.clicked += OnOptionsButtonClicked;
            _quitGameButton.clicked += OnQuitGameButtonClicked;
        }

        private void OnNavigateBackRequested()
        {
            if (_menuItemContainer == null || _optionsSubMenu == null || _creditsSubMenu == null)
                throw new InvalidOperationException(
                    $"{nameof(OnNavigateBackRequested)} called before {nameof(OnEnable)}!");
            
            _menuItemContainer.RemoveFromClassList("disabled");
            _menuItemContainer.AddToClassList("enabled");
            _optionsSubMenu.RemoveFromClassList("enabled");
            _optionsSubMenu.AddToClassList("disabled");
            _creditsSubMenu.RemoveFromClassList("enabled");
            _creditsSubMenu.AddToClassList("disabled");
        }

        private void OnDisable()
        {
            if (_optionsSubMenuHandler == null || _creditsSubMenuHandler == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} called before {nameof(Awake)}!");

            _optionsSubMenuHandler.NavigateBackRequested -= OnNavigateBackRequested;
            _creditsSubMenuHandler.NavigateBackRequested -= OnNavigateBackRequested;
            
            if (_creditsButton == null
                || _optionsButton == null
                || _quitGameButton == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} was called before {nameof(OnEnable)}!");

            _creditsButton.clicked -= OnCreditsButtonClicked;
            _optionsButton.clicked -= OnOptionsButtonClicked;
            _quitGameButton.clicked -= OnQuitGameButtonClicked;
        }

        private void OnCreditsButtonClicked()
        {
            if (_menuItemContainer == null || _creditsSubMenu == null)
                throw new InvalidOperationException(
                    $"{nameof(OnCreditsButtonClicked)} called before {nameof(OnEnable)}!");
            
            _menuItemContainer.RemoveFromClassList("enabled");
            _menuItemContainer.AddToClassList("disabled");
            _creditsSubMenu.RemoveFromClassList("disabled");
            _creditsSubMenu.AddToClassList("enabled");
        }

        private void OnOptionsButtonClicked()
        {
            if (_menuItemContainer == null || _optionsSubMenu == null)
                throw new InvalidOperationException(
                    $"{nameof(OnOptionsButtonClicked)} called before {nameof(OnEnable)}!");
            
            _menuItemContainer.RemoveFromClassList("enabled");
            _menuItemContainer.AddToClassList("disabled");
            _optionsSubMenu.RemoveFromClassList("disabled");
            _optionsSubMenu.AddToClassList("enabled");
        }

        public static void OnQuitGameButtonClicked()
        {
            // TODO: Maybe we should ask for confirmation before closing the application?
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}