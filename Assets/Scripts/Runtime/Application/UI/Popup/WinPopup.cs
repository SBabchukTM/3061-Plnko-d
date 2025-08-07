using Runtime.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class WinPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI _betAmountText;
        [SerializeField] private TextMeshProUGUI _winAmountText;
        [SerializeField] private GameObject _cantPlayGO;

        [SerializeField] private SimpleButton _homeButton;
        [SerializeField] private SimpleButton _continueButton;

        public event Action OnHomePressed;
        public event Action OnContinuePressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.Button.onClick.AddListener(() => OnHomePressed?.Invoke());
            _continueButton.Button.onClick.AddListener(() => OnContinuePressed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetData(int bet, int win)
        {
            _betAmountText.text = bet.ToString();
            _winAmountText.text = win.ToString();
        }

        public void LockContinue()
        {
            _cantPlayGO.SetActive(true);
            _continueButton.Button.interactable = false;
            _continueButton.Button.image.color = Color.gray;
        }
    }
}