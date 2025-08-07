using System;
using System.Threading;
using Runtime.Services.Audio;
using Runtime.Services.UserData;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;

        public event Action<float> OnSoundVolumeChangeEvent;
        public event Action<float> OnMusicVolumeChangeEvent;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SettingsPopupData settingsPopupData = data as SettingsPopupData;

            var isSoundVolume = settingsPopupData.SoundVolumeValue;
            _soundVolumeSlider.onValueChanged.Invoke(isSoundVolume);
            _soundVolumeSlider.value = isSoundVolume;

            var isMusicVolume = settingsPopupData.MusicVolumeValue;
            _musicVolumeSlider.onValueChanged.Invoke(isMusicVolume);
            _musicVolumeSlider.value = isMusicVolume;

            _closeButton.onClick.AddListener(DestroyPopup);

            _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeToggleValueChanged);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeToggleValueChanged);

            AudioService.PlaySound(ConstAudio.OpenPopupSound);

            return base.Show(data, cancellationToken);
        }

        public override void DestroyPopup()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            Destroy(gameObject);
        }

        private void OnSoundVolumeToggleValueChanged(float value)
        {
            OnSoundVolumeChangeEvent?.Invoke(value);
        }

        private void OnMusicVolumeToggleValueChanged(float value)
        {
            OnMusicVolumeChangeEvent?.Invoke(value);
        }
    }
}