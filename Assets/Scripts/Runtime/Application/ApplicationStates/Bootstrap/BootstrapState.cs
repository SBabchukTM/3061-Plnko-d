using System.Threading;
using Runtime.Services.UserData;
using Runtime.UI;
using Core;
using Core.Services.ScreenOrientation;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = Core.ILogger;

namespace Runtime.GameStateMachine
{
    public class BootstrapState : StateController
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IUiService _uiService;
        private readonly ISettingProvider _settingProvider;
        private readonly UserDataService _userDataService;
        private readonly AudioSettingsBootstrapController _audioSettingsBootstrapController;

        public BootstrapState(IAssetProvider assetProvider,
            IUiService uiService,
            ILogger logger,
            ISettingProvider settingProvider,
            UserDataService userDataService,
            AudioSettingsBootstrapController audioSettingsBootstrapController) : base(logger)
        {
            _assetProvider = assetProvider;
            _uiService = uiService;
            _settingProvider = settingProvider;
            _userDataService = userDataService;
            _audioSettingsBootstrapController = audioSettingsBootstrapController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            Input.multiTouchEnabled = false;

            _userDataService.Initialize();
            await _assetProvider.Initialize();
            await _uiService.Initialize();
            await _settingProvider.Initialize();
            _uiService.ShowScreen(ConstScreens.SplashScreen, cancellationToken).Forget();
            await _audioSettingsBootstrapController.Run(CancellationToken.None);
            UpdateSession();

            GoTo<GameState>().Forget();
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.SplashScreen);
        }

        private void UpdateSession()
        {
            _userDataService.GetUserData().GameData.SessionNumber++;
            _userDataService.SaveUserData();
        }
    }
}