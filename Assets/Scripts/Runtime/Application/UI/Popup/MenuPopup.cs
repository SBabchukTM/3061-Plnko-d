using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class MenuPopup : BasePopup
    {
        [SerializeField] private Button _accountButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _rulesButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _closeButton;

        public event Action OnAccountPressed;
        public event Action OnSettingsPressed;
        public event Action OnLeaderboardPressed;
        public event Action OnRulesPressed;
        public event Action OnShopPressed;
        public event Action OnClosePressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SubscribeToEvents();
            return base.Show(data, cancellationToken);
        }

        private void OnDestroy()
        {
            _accountButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _leaderboardButton.onClick.RemoveAllListeners();
            _rulesButton.onClick.RemoveAllListeners();
            _shopButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }

        private void SubscribeToEvents()
        {
            _accountButton.onClick.AddListener(() => OnAccountPressed?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _leaderboardButton.onClick.AddListener(() => OnLeaderboardPressed?.Invoke());
            _rulesButton.onClick.AddListener(() => OnRulesPressed?.Invoke());
            _shopButton.onClick.AddListener(() => OnShopPressed?.Invoke());
            _closeButton.onClick.AddListener(() => OnClosePressed?.Invoke());
        }
    }
}