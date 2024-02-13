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
    [RequireComponent(typeof(InventorySubMenuHandler))]
    public class InGameMenuHandler : MonoBehaviour
    {
        // TODO: If there's enough time, add a custom property drawer that supports scene references
        //       https://github.com/Tymski/SceneReference/blob/master/Scripts/SceneReference.cs
        [SerializeField] private string mainMenuSceneName;
        [SerializeField, DisallowNull, MaybeNull] private PauseManager pauseManager;
        [DisallowNull, MaybeNull] private VisualElement _menuItemContainer;
        [DisallowNull, MaybeNull] private OptionsSubMenuHandler _optionsSubMenuHandler;
        [DisallowNull, MaybeNull] private InventorySubMenuHandler _inventorySubMenuHandler;
        [DisallowNull, MaybeNull] private VisualElement _root;
        [DisallowNull, MaybeNull] private VisualElement _backgroundPanel;
        [DisallowNull, MaybeNull] private Button _inventoryButton;
        [DisallowNull, MaybeNull] private Button _optionsButton;
        [DisallowNull, MaybeNull] private Button _continueGameButton;
        [DisallowNull, MaybeNull] private Button _quitToMainMenuButton;
        [DisallowNull, MaybeNull] private Button _quitGameButton;
        [DisallowNull, MaybeNull] private OptionsSubMenu _optionsSubMenu;
        [DisallowNull, MaybeNull] private InventorySubMenu _inventorySubMenu;

        public void Show()
        {
            if (pauseManager == null)
                throw new InvalidOperationException(
                    $"{nameof(pauseManager)} field in {nameof(InGameMenuHandler)} component on game object {gameObject.name} was not set!");
            if (_backgroundPanel == null)
                throw new InvalidOperationException($"{nameof(Show)} was called before {nameof(Awake)}!");
            
            pauseManager.Pause();
            _backgroundPanel.RemoveFromClassList("disabled");
            _backgroundPanel.AddToClassList("enabled");
        }

        public void Hide()
        {
            if (_backgroundPanel == null)
                throw new InvalidOperationException($"{nameof(Hide)} was called before {nameof(Awake)}!");

            _backgroundPanel.RemoveFromClassList("enabled");
            _backgroundPanel.AddToClassList("disabled");
            
            if (_optionsSubMenuHandler != null)
                _optionsSubMenuHandler.Cancel();

            if (_inventorySubMenuHandler != null)
                _inventorySubMenuHandler.Cancel();

            OnNavigateBackRequested();
            
            if (pauseManager == null)
                throw new InvalidOperationException(
                    $"{nameof(pauseManager)} field in {nameof(InGameMenuHandler)} component on game object {gameObject.name} was not set!");

            pauseManager.Resume();
        }

        private void Awake()
        {
            _optionsSubMenuHandler = this.RequireComponent<OptionsSubMenuHandler>();
            _inventorySubMenuHandler = this.RequireComponent<InventorySubMenuHandler>();
            var uiDocument = this.RequireComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            _root = root
                ?? throw new InvalidOperationException(
                    $"{nameof(UIDocument)} component on {gameObject.name} has no root element!");
            _backgroundPanel = root.RequireElement<VisualElement>("background-panel");
        }

        private void OnEnable()
        {
            if (_optionsSubMenuHandler == null || _inventorySubMenuHandler == null || _root == null)
                throw new InvalidOperationException($"{nameof(OnEnable)} called before {nameof(Awake)}!");

            _optionsSubMenuHandler.NavigateBackRequested += OnNavigateBackRequested;
            _inventorySubMenuHandler.NavigateBackRequested += OnNavigateBackRequested;
            _inventoryButton = _root.RequireElement<Button>("inventory-button");
            _optionsButton = _root.RequireElement<Button>("options-button");
            _continueGameButton = _root.RequireElement<Button>("continue-game-button");
            _quitToMainMenuButton = _root.RequireElement<Button>("quit-to-main-menu-button");
            _quitGameButton = _root.RequireElement<Button>("quit-game-button");
            _menuItemContainer = _root.RequireElement<VisualElement>("menu-item-container");
            _optionsSubMenu = _root.RequireElement<OptionsSubMenu>("options-sub-menu");
            _inventorySubMenu = _root.RequireElement<InventorySubMenu>("inventory-sub-menu");
            _inventoryButton.clicked += OnInventoryButtonClicked;
            _optionsButton.clicked += OnOptionsButtonClicked;
            _continueGameButton.clicked += OnContinueGameButtonClicked;
            _quitToMainMenuButton.clicked += OnQuitToMainMenuButtonClicked;
            _quitGameButton.clicked += OnQuitGameButtonClicked;
        }

        private void OnNavigateBackRequested()
        {
            if (_menuItemContainer == null || _optionsSubMenu == null || _inventorySubMenu == null)
                throw new InvalidOperationException(
                    $"{nameof(OnNavigateBackRequested)} called before {nameof(OnEnable)}!");
            
            _menuItemContainer.RemoveFromClassList("disabled");
            _menuItemContainer.AddToClassList("enabled");
            _optionsSubMenu.RemoveFromClassList("enabled");
            _optionsSubMenu.AddToClassList("disabled");
            _inventorySubMenu.RemoveFromClassList("enabled");
            _inventorySubMenu.AddToClassList("disabled");
        }

        private void OnDisable()
        {
            if (_optionsSubMenuHandler == null || _inventorySubMenuHandler == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} called before {nameof(Awake)}!");

            _optionsSubMenuHandler.NavigateBackRequested -= OnNavigateBackRequested;
            _inventorySubMenuHandler.NavigateBackRequested -= OnNavigateBackRequested;
            
            if (_inventoryButton == null
                || _optionsButton == null
                || _continueGameButton == null
                || _quitToMainMenuButton == null
                || _quitGameButton == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} was called before {nameof(OnEnable)}!");

            _inventoryButton.clicked -= OnInventoryButtonClicked;
            _optionsButton.clicked -= OnOptionsButtonClicked;
            _continueGameButton.clicked -= OnContinueGameButtonClicked;
            _quitToMainMenuButton.clicked -= OnQuitToMainMenuButtonClicked;
            _quitGameButton.clicked -= OnQuitGameButtonClicked;
        }

        private void OnInventoryButtonClicked()
        {
            if (_menuItemContainer == null || _inventorySubMenu == null)
                throw new InvalidOperationException(
                    $"{nameof(OnInventoryButtonClicked)} called before {nameof(OnEnable)}!");
            
            _menuItemContainer.RemoveFromClassList("enabled");
            _menuItemContainer.AddToClassList("disabled");
            _inventorySubMenu.RemoveFromClassList("disabled");
            _inventorySubMenu.AddToClassList("enabled");
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

        private void OnContinueGameButtonClicked()
        {
            Hide();
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
