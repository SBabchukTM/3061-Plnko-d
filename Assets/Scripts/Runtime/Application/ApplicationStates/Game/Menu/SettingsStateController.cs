using System.Threading;
using Core.StateMachine;
using Runtime.UI;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Runtime.Services.UserData;
using Core.Services.Audio;
using Runtime.Services.Audio;
using Core.UI;

namespace Runtime.Game
{
    public class SettingsStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;
        private readonly StartSettingsController _startSettingsController;

        private SettingsScreen _screen;

        public SettingsStateController(ILogger logger, IAudioService audioService, IUiService uiService, UserDataService userDataService, StartSettingsController startSettingsController) : base(logger)
        {
            _uiService = uiService;
            _audioService = audioService;
            _userDataService = userDataService;
            _startSettingsController = startSettingsController;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();

            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            UnsubscribeToEvents();
            await _uiService.HideScreen(ConstScreens.SettingsScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<SettingsScreen>(ConstScreens.SettingsScreen);
            _screen.Initialize(_userDataService.GetUserData().SettingsData);
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnSoundVolumeChangeEvent += OnChangeSoundVolume;
            _screen.OnMusicVolumeChangeEvent += OnChangeMusicVolume;
            _screen.OnInfoPressed += ShowInfoPopup;
            _screen.OnPrivacyPolicyPressed += ShowPrivacyPolicyPopup;
            _screen.OnTermsPressed += ShowTermsPopup;
        }
        private void UnsubscribeToEvents()
        {
            _screen.OnSoundVolumeChangeEvent -= OnChangeSoundVolume;
            _screen.OnMusicVolumeChangeEvent -= OnChangeMusicVolume;
            _screen.OnInfoPressed -= ShowInfoPopup;
            _screen.OnPrivacyPolicyPressed -= ShowPrivacyPolicyPopup;
            _screen.OnTermsPressed -= ShowTermsPopup;
        }

        private void OnChangeSoundVolume(float volume)
        {
            _audioService.SetVolume(Core.Services.Audio.AudioType.Sound, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.SoundVolumeValue = volume;

         //   _audioService.PlaySound(ConstAudio.PressButtonSound);
        }

        private void OnChangeMusicVolume(float volume)
        {
            _audioService.SetVolume(Core.Services.Audio.AudioType.Music, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.MusicVolumeValue = volume;

         //   _audioService.PlaySound(ConstAudio.PressButtonSound);
        }

        private void ShowInfoPopup()
        {
            var popup = _uiService.GetPopup<RulesPopup>(ConstPopups.RulesPopup);
            popup.Show(null);
        }

        private void ShowPrivacyPolicyPopup()
        {
            _uiService.ShowPopup(ConstPopups.PrivacyPolicyPopup);
        }

        private void ShowTermsPopup()
        {
            _uiService.ShowPopup(ConstPopups.TermsPopup);
        }

    }
}