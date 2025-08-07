using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class MenuScreen : UiScreen
    {
        // [SerializeField] private SimpleButton _menuButton;
        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private SimpleButton _shopButton;
        [SerializeField] private SimpleButton _leaderboardButton;
        [SerializeField] private SimpleButton _settingsButton;
        [SerializeField] private SimpleButton _accountButton;
        //  [SerializeField] private TextMeshProUGUI _balanceText;

        //  public event Action OnMenuPressed;
        public event Action OnPlayPressed;
        public event Action OnShopPressed;
        public event Action OnLeaderboardPressed;
        public event Action OnSettingsPressed;
        public event Action OnAccountPressed;

        private void OnDestroy()
        {
            _playButton.Button.onClick.RemoveAllListeners();
            _shopButton.Button.onClick.RemoveAllListeners();
            _leaderboardButton.Button.onClick.RemoveAllListeners();
            _settingsButton.Button.onClick.RemoveAllListeners();
            _accountButton.Button.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            _playButton.Button.onClick.AddListener(() => OnPlayPressed?.Invoke());
            _shopButton.Button.onClick.AddListener(() => OnShopPressed?.Invoke());
            _leaderboardButton.Button.onClick.AddListener(() => OnLeaderboardPressed?.Invoke());
            _settingsButton.Button.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _accountButton.Button.onClick.AddListener(() => OnAccountPressed?.Invoke());
            //  _balanceText.text = balance.ToString();
        }
    }
}