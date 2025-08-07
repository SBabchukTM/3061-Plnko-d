using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class GameplayScreen : UiScreen
    {
        private const float FadeAnimTime = 0.5f;

        [SerializeField] private SimpleButton _backButton;
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private TextMeshProUGUI _betText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private CanvasGroup _gameStartedNotifyCanvas;
        [SerializeField] private CanvasGroup _beforeGameStartedNotifyCanvas;
        [SerializeField] private CameraAdjuster _cameraAdjuster;

        public event Action OnBackPressed;

        public void Initialize(Transform physicObject)
        {
            _cameraAdjuster.Initial(physicObject);
            _backButton.Button.onClick.AddListener(() => OnBackPressed?.Invoke());
        }

        public async UniTaskVoid ChangeHelpMessage()
        {
            _beforeGameStartedNotifyCanvas.DOFade(0, FadeAnimTime);
            await UniTask.WaitForSeconds(FadeAnimTime);
            _gameStartedNotifyCanvas.DOFade(1, FadeAnimTime);
        }

        public void DisableBackButton() => _backButton.Button.interactable = false;
        public void DisableHelpMessage() => _beforeGameStartedNotifyCanvas.alpha = 0;

        public void SetBalance(int balance) => _balanceText.text = balance.ToString();
        public void SetBet(int bet) => _betText.text = bet.ToString();
        public void SetLevel(int level) => _levelText.text = "Level " + (level + 1);
    }
}