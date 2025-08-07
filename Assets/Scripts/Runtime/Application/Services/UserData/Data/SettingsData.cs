using System;

namespace Runtime.Services.UserData
{
    [Serializable]
    public class SettingsData
    {
        public float SoundVolumeValue = 0.5f;
        public float MusicVolumeValue = 0.5f;
        public bool IsVibration = true;
    }
}