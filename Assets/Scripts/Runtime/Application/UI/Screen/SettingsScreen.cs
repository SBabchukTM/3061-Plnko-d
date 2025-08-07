using Runtime.Services.UserData;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SettingsScreen : UiScreen
    {
        [SerializeField] private SimpleButton _backButton;
        [SerializeField] private SimpleButton _infoButton;
        [SerializeField] private SimpleButton _termsButton;
        [SerializeField] private SimpleButton _privacyPolicyButton;
        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;

        public event Action OnBackPressed;
        public event Action OnInfoPressed;
        public event Action OnTermsPressed;
        public event Action OnPrivacyPolicyPressed;

        public event Action<float> OnSoundVolumeChangeEvent;
        public event Action<float> OnMusicVolumeChangeEvent;

        private void OnDestroy()
        {
            _backButton.Button.onClick.RemoveAllListeners();
            _infoButton.Button.onClick.RemoveAllListeners();
            _termsButton.Button.onClick.RemoveAllListeners();
            _privacyPolicyButton.Button.onClick.RemoveAllListeners();
            _soundVolumeSlider.onValueChanged.RemoveAllListeners();
            _musicVolumeSlider.onValueChanged.RemoveAllListeners();
        }

        public void Initialize(SettingsData data)
        {
            SetData(data);
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _backButton.Button.onClick.AddListener(() => OnBackPressed?.Invoke());
            _infoButton.Button.onClick.AddListener(() => OnInfoPressed?.Invoke());
            _termsButton.Button.onClick.AddListener(() => OnTermsPressed?.Invoke());
            _privacyPolicyButton.Button.onClick.AddListener(() => OnPrivacyPolicyPressed?.Invoke());
            _soundVolumeSlider.onValueChanged.AddListener((value) => OnSoundVolumeChangeEvent?.Invoke(value));
            _musicVolumeSlider.onValueChanged.AddListener((value) => OnMusicVolumeChangeEvent?.Invoke(value));
        }

        private void SetData(SettingsData data)
        {
            _soundVolumeSlider.value = data.SoundVolumeValue;
            _musicVolumeSlider.value = data.MusicVolumeValue;
        }
    }
}