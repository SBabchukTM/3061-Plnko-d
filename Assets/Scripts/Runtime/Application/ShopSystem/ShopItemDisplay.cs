using Runtime.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Game;

public class ShopItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private SimpleButton _purchaseButton;
    [SerializeField] private SimpleButton _useButton;
    [SerializeField] private GameObject _usedPanel;

    private bool _inAnim = false;

    private ShopItem _shopItem;
    public ShopItem ShopItem => _shopItem;

    public event Action<ShopItemDisplay> OnPurchasePressed;
    // public event Action<ShopItemDisplay> OnUsePressed;

    private void OnDestroy()
    {
        _purchaseButton.Button.onClick.RemoveAllListeners();
    }

    public void Initialize(ShopItem shopItem)
    {
        _shopItem = shopItem;

        _itemImage.sprite = _shopItem.ItemSprite;
        _priceText.text = _shopItem.ItemPrice.ToString();

        _purchaseButton.Button.onClick.AddListener(() => OnPurchasePressed?.Invoke(this));
        _useButton.Button.onClick.AddListener(() => OnPurchasePressed?.Invoke(this));
    }

    public void SetStatus(ShopItemStatus status)
    {
        //_statusText.text = status;
        var purchaseButtonActive = false;
        var useButtonActive = false;
        var usedPanelActive = false;

        switch (status)
        {
            case ShopItemStatus.PurchaseStatus:
                purchaseButtonActive = true;
                break;
            case ShopItemStatus.SelectedStatus:
                usedPanelActive = true;
                break;
            case ShopItemStatus.NotSelectedStatus:
                useButtonActive = true;
                break;
            default:
                break;
        }

        _purchaseButton.gameObject.SetActive(purchaseButtonActive);
        _useButton.gameObject.SetActive(useButtonActive);
        _usedPanel.SetActive(usedPanelActive);
    }

    public async UniTaskVoid Shake(CancellationToken token, PurchaseFailedShakeParameters purchaseFailedShakeParameters)
    {
        if (_inAnim)
            return;

        _inAnim = true;
        _purchaseButton.transform.DOShakePosition(purchaseFailedShakeParameters.ShakeDuration,
                                          purchaseFailedShakeParameters.Strength,
                                          purchaseFailedShakeParameters.Vibrato,
                                          purchaseFailedShakeParameters.Randomness,
                                          purchaseFailedShakeParameters.Snapping,
                                          purchaseFailedShakeParameters.FadeOut,
                                          purchaseFailedShakeParameters.ShakeRandomnessMode).SetLink(gameObject);

        await UniTask.WaitForSeconds(purchaseFailedShakeParameters.ShakeDuration);
        token.ThrowIfCancellationRequested();

        _inAnim = false;
    }
}
