using System;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace UI
{
    /// <summary>
    /// Handles the options sub menu in the in-game menu and the main menu.
    /// </summary>
    /// <remarks>
    /// This component assumes that the content of the <c>OptionsSubMenu.uxml</c> asset are loaded in this
    /// <see cref="GameObject"/>'s <see cref="UIDocument"/> component.
    /// </remarks>
    [RequireComponent(typeof(UIDocument))]
    public class OptionsSubMenuHandler : MonoBehaviour, ISubMenuHandler
    {
        private const string PlayerPrefMasterVolume = "Master Volume";
        private const float DefaultMasterVolume = 0.6f;
        private Slider _masterVolume;
        private Button _saveChanges;
        private Button _discardChanges;
        private Button _back;

        public event Action NavigateBackRequested;

        public void Cancel()
        {
            OnDiscardChangesClicked();
        }

        private void OnEnable()
        {
            var uiDocument = this.RequireComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            if (root == null)
                throw new InvalidOperationException(
                    $"Root element of {gameObject.name}'s {nameof(UIDocument)} is null!");
            
            _masterVolume = root.RequireElement<Slider>("master-volume-slider");
            _saveChanges = root.RequireElement<Button>("save-changes-button");
            _discardChanges = root.RequireElement<Button>("discard-changes-button");
            _back = root.RequireElement<Button>("back-button");

            _saveChanges.clicked += OnSaveChangesClicked;
            _discardChanges.clicked += OnDiscardChangesClicked;
            _back.clicked += OnBackClicked;
            _masterVolume.RegisterValueChangedCallback(OnMasterVolumeChanged);
            
            OnDiscardChangesClicked();
        }

        private void OnDisable()
        {
            if (_saveChanges == null || _discardChanges == null || _back == null)
                throw new InvalidOperationException($"{nameof(OnDisable)} called before {nameof(OnEnable)}!");

            _saveChanges.clicked -= OnSaveChangesClicked;
            _discardChanges.clicked -= OnDiscardChangesClicked;
            _back.clicked -= OnBackClicked;
        }

        private void OnMasterVolumeChanged(ChangeEvent<float> changeEvent)
        {
            // TODO: We should preview the volume change.
            //       Don't forget to reset the preview when OnDiscardChangesClicked is called.
            DisplayChanged();
        }

        private void OnSaveChangesClicked()
        {
            if (_masterVolume != null)
                PlayerPrefs.SetFloat(PlayerPrefMasterVolume, _masterVolume.value);
            else
                Debug.LogErrorFormat(this, "UI field {0} was null!", nameof(_masterVolume));
            
            DisplayUnchanged();
        }

        private void OnDiscardChangesClicked()
        {
            if (_masterVolume != null)
            {
                var masterVolumeValue = PlayerPrefs.GetFloat(PlayerPrefMasterVolume, DefaultMasterVolume);
                _masterVolume.SetValueWithoutNotify(masterVolumeValue);
            }
            else
            {
                Debug.LogErrorFormat(this, "UI field {0} was null!", nameof(_masterVolume));
            }

            DisplayUnchanged();
        }

        private void DisplayChanged()
        {
            if (_saveChanges == null || _discardChanges == null)
                throw new InvalidOperationException($"{nameof(DisplayChanged)} called before {nameof(OnEnable)}!");
            
            _saveChanges.SetEnabled(true);
            _discardChanges.SetEnabled(true);
        }

        private void DisplayUnchanged()
        {
            if (_saveChanges == null || _discardChanges == null)
                throw new InvalidOperationException($"{nameof(DisplayUnchanged)} called before {nameof(OnEnable)}!");

            _saveChanges.SetEnabled(false);
            _discardChanges.SetEnabled(false);
        }

        private void OnBackClicked()
        {
            NavigateBackRequested?.Invoke();
        }
    }
}
