using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Util;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(OptionsSubMenuHandler))]
    public class InGameMenuHandler : MonoBehaviour
    {
        // TODO: If there's enough time, add a custom property drawer that supports scene references
        //       https://github.com/Tymski/SceneReference/blob/master/Scripts/SceneReference.cs
        [SerializeField] private string mainMenuSceneName;
        [DisallowNull, MaybeNull] private VisualElement _menuItemContainer;
        [DisallowNull, MaybeNull] private OptionsSubMenuHandler _optionsSubMenuHandler;
        [DisallowNull, MaybeNull] private Button _inventoryButton;
        [DisallowNull, MaybeNull] private Button _optionsButton;
        [DisallowNull, MaybeNull] private Button _quitToMainMenuButton;
        [DisallowNull, MaybeNull] private Button _quitGameButton;
        [DisallowNull, MaybeNull] private OptionsSubMenu _optionsSubMenu;

        private void Awake()
        {
            _optionsSubMenuHandler = this.RequireComponent<OptionsSubMenuHandler>();
        }

        private void OnEnable()
        {
            if (_optionsSubMenuHandler == null)
                throw new InvalidOperationException($"{nameof(OnEnable)} called before {nameof(Awake)}!");

            _optionsSubMenuHandler.NavigateBackRequested += OnNavigateBackRequested;
            
            var uiDocument = this.RequireComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            if (root == null)
                throw new InvalidOperationException(
                    $"Root element of {gameObject.name}'s {nameof(UIDocument)} is null!");

            _inventoryButton = root.RequireElement<Button>("inventory-button");
            _optionsButton = root.RequireElement<Button>("options-button");
            _quitToMainMenuButton = root.RequireElement<Button>("quit-to-main-menu-button");
            _quitGameButton = root.RequireElement<Button>("quit-game-button");
            _menuItemContainer = root.RequireElement<VisualElement>("menu-item-container");
            _optionsSubMenu = root.RequireElement<OptionsSubMenu>("options-sub-menu");
            
            _inventoryButton.clicked += OnInventoryButtonClicked;
            _optionsButton.clicked += OnOptionsButtonClicked;
            _quitToMainMenuButton.clicked += OnQuitToMainMenuButtonClicked;
            _quitGameButton.clicked += OnQuitGameButtonClicked;
        }

        private void OnNavigateBackRequested()
        {
            if (_menuItemContainer == null || _optionsSubMenu == null)
                throw new InvalidOperationException(
                    $"{nameof(OnNavigateBackRequested)} called before {nameof(OnEnable)}!");
            
            _menuItemContainer.RemoveFromClassList("disabled");
            _menuItemContainer.AddToClassList("enabled");
            _optionsSubMenu.RemoveFromClassList("enabled");
            _optionsSubMenu.AddToClassList("disabled");
        }

        private void OnDisable()
        {
            if (_optionsSubMenuHandler == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} called before {nameof(Awake)}!");

            _optionsSubMenuHandler.NavigateBackRequested -= OnNavigateBackRequested;
            
            if (_inventoryButton == null
                || _optionsButton == null
                || _quitToMainMenuButton == null
                || _quitGameButton == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} was called before {nameof(OnEnable)}!");

            _inventoryButton.clicked -= OnInventoryButtonClicked;
            _optionsButton.clicked -= OnOptionsButtonClicked;
            _quitToMainMenuButton.clicked -= OnQuitToMainMenuButtonClicked;
            _quitGameButton.clicked -= OnQuitGameButtonClicked;
        }

        private void OnInventoryButtonClicked()
        {
            
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

        private void OnQuitToMainMenuButtonClicked()
        {
            // TODO: Maybe we should ask for confirmation before unloading this scene?
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
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
