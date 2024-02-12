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
    public class InGameMenuHandler : MonoBehaviour
    {
        // TODO: If there's enough time, add a custom property drawer that supports scene references
        //       https://github.com/Tymski/SceneReference/blob/master/Scripts/SceneReference.cs
        [SerializeField] private string mainMenuSceneName;
        [DisallowNull, MaybeNull] private Button _inventoryButton;
        [DisallowNull, MaybeNull] private Button _optionsButton;
        [DisallowNull, MaybeNull] private Button _quitToMainMenuButton;
        [DisallowNull, MaybeNull] private Button _quitGameButton;

        private void OnEnable()
        {
            var uiDocument = this.RequireComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            if (root == null)
                throw new InvalidOperationException(
                    $"Root element of {gameObject.name}'s {nameof(UIDocument)} is null!");

            _inventoryButton = root.RequireElement<Button>("inventory-button");
            _optionsButton = root.RequireElement<Button>("options-button");
            _quitToMainMenuButton = root.RequireElement<Button>("quit-to-main-menu-button");
            _quitGameButton = root.RequireElement<Button>("quit-game-button");
            
            _inventoryButton.clicked += OnInventoryButtonClicked;
            _optionsButton.clicked += OnOptionsButtonClicked;
            _quitToMainMenuButton.clicked += OnQuitToMainMenuButtonClicked;
            _quitGameButton.clicked += OnQuitGameButtonClicked;
        }

        private void OnDisable()
        {
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
