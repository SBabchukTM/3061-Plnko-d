using System.Threading;
using Core.StateMachine;
using Runtime.UI;
using Cysharp.Threading.Tasks;
using ILogger = Core.ILogger;
using Zenject;
using Core;
using Runtime.Services;
using Runtime.Services.UserData;
using UnityEngine;
using System.Linq;
using Core.UI;
using Core.Services.Audio;
using Runtime.Services.Audio;

namespace Runtime.Game
{
    public class GameplayStateController : StateController, IInitializable
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly PyramidController _pyramidController;

        private GameplayScreen _screen;
        private GameLevelsConfig _gameLevelsConfig;
        private ShopConfig _shopConfig;
        private UserInventory _userInventory;

        private bool _gameEnded = false;
        private int _level = 0;
        private int _bet;

        public GameplayStateController(ILogger logger, IUiService uiService, IAudioService audioService, IAssetProvider assetProvider, UserDataService userDataService, PyramidController pyramidController) : base(logger)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
            _audioService = audioService;
            _userDataService = userDataService;
            _pyramidController = pyramidController;
        }

        public async void Initialize()
        {
            _gameLevelsConfig = await _assetProvider.Load<GameLevelsConfig>(ConstConfigs.GameLevelsConfig);
            _shopConfig = await _assetProvider.Load<ShopConfig>(ConstConfigs.ShopItemsConfig);
            _userInventory = _userDataService.GetUserData().UserInventory;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            _bet = _gameLevelsConfig.LevelConfigs[_level].Bet;
            _gameEnded = false;

            CreateScreen();
            SubscribeToEvents();
            StartGame();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _pyramidController.OnGameEnded -= ProcessGameEnd;
            _pyramidController.OnPlayerStartedGame -= ProcessGameStart;
            _pyramidController.EndGame();
            await _uiService.HideScreen(ConstScreens.GameplayScreen);
        }

        public void SetLevel(int level) => _level = level;

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<GameplayScreen>(ConstScreens.GameplayScreen);
            _screen.Initialize(_pyramidController.PyramidTransform);
            _screen.ShowAsync().Forget();
            _screen.SetBalance(_userInventory.Balance);
            _screen.SetBet(_bet);
            _screen.SetLevel(_level);
            if (_level != 0)
            {
                _screen.DisableHelpMessage();
            }

        }

        private void SubscribeToEvents()
        {
            _pyramidController.OnGameEnded += ProcessGameEnd;
            _pyramidController.OnPlayerStartedGame += ProcessGameStart;
            _screen.OnBackPressed += async () => await GoTo<LevelSelectionStateController>();
        }

        private void StartGame()
        {
            _pyramidController.SetBallSprite(GetSelectedSprite());
            _pyramidController.SetSlotsData(_gameLevelsConfig.LevelConfigs[_level]);
            _pyramidController.StartGame();
        }

        private Sprite GetSelectedSprite()
        {
            return _shopConfig.ShopItems.Where(x => x.ItemID == _userInventory.UsedGameItemID).First().ItemSprite;
        }

        private int CalculateWinAmount(PlinkoSlotType slotType, float winning)
        {
            if (slotType == PlinkoSlotType.Reward)
                return Mathf.RoundToInt(winning);
            else
                return Mathf.RoundToInt(_bet * winning);
        }

        private async void ShowGameEndPopup(int bet, int win, bool lockContinue)
        {
            Time.timeScale = 0;

            WinPopup winPopup = await _uiService.ShowPopup(ConstPopups.WinPopup) as WinPopup;
            winPopup.SetData(bet, win);
            if (lockContinue)
                winPopup.LockContinue();

            winPopup.OnContinuePressed += async () =>
            {
                Time.timeScale = 1;
                winPopup.DestroyPopup();
                await GoTo<GameplayStateController>();
            };

            winPopup.OnHomePressed += async () =>
            {
                Time.timeScale = 1;
                winPopup.DestroyPopup();
                await GoTo<MenuStateController>();
            };
        }

        private void ProcessGameEnd(PlinkoSlotType slotType, float winning)
        {
            if (_gameEnded)
                return;

            _gameEnded = true;

            int winAmount = CalculateWinAmount(slotType, winning);
            _userInventory.Balance += winAmount;

            if (_bet > winAmount)
                _audioService.PlaySound(ConstAudio.LoseSound);
            else
                _audioService.PlaySound(ConstAudio.VictorySound);

            ShowGameEndPopup(_bet, winAmount, _userInventory.Balance < _bet);
        }


        private void ProcessGameStart()
        {
            if (_level == 0)
            {
                _screen.ChangeHelpMessage().Forget();
            }
            else
            {
                _screen.DisableHelpMessage();
            }
            _screen.DisableBackButton();

            _userInventory.Balance -= _bet;
            _screen.SetBalance(_userInventory.Balance);
        }
    }
}