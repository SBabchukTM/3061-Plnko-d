using System.Threading;
using Runtime.Services.UserData;
using Runtime.UI;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using Runtime.Services.Audio;

namespace Runtime.Game
{
    public sealed class StartSettingsController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        public StartSettingsController(IUiService uiService, UserDataService userDataService, IAudioService audioService)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            SettingsPopup settingsPopup = _uiService.GetPopup<SettingsPopup>(ConstPopups.SettingsPopup);

            settingsPopup.OnSoundVolumeChangeEvent += OnChangeSoundVolume;
            settingsPopup.OnMusicVolumeChangeEvent += OnChangeMusicVolume;

            var userData = _userDataService.GetUserData();

            var soundVolumeValue = userData.SettingsData.SoundVolumeValue;
            var musicVolumeValue = userData.SettingsData.MusicVolumeValue;

            CurrentState = ControllerState.Complete;
            return UniTask.CompletedTask;
        }

        private void OnChangeSoundVolume(float volume)
        {
            _audioService.SetVolume(Core.Services.Audio.AudioType.Sound, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.SoundVolumeValue = volume;

            _audioService.PlaySound(ConstAudio.PressButtonSound);
        }

        private void OnChangeMusicVolume(float volume)
        {
            _audioService.SetVolume(Core.Services.Audio.AudioType.Music, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.MusicVolumeValue = volume;

            _audioService.PlaySound(ConstAudio.PressButtonSound);
        }
    }
}