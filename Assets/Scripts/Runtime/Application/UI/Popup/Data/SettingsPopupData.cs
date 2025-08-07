using Runtime.Services.UserData;
using Core.UI;

namespace Runtime.UI
{
    public class SettingsPopupData : BasePopupData
    {
        private float _soundVolumeValue;
        private float _musicVolumeValue;

        public float SoundVolumeValue => _soundVolumeValue;
        public float MusicVolumeValue => _musicVolumeValue;

        public SettingsPopupData(float isSoundVolume, float isMusicVolume)
        {
            _soundVolumeValue = isSoundVolume;
            _musicVolumeValue = isMusicVolume;
        }
    }
}