using System;
using TMPro;
using UnityEngine;

public class PlinkoSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rewardText;

    private PlinkoSlotType _slotType;
    private float _reward;

    public event Action<PlinkoSlotType, float> OnBallCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnBallCollision?.Invoke(_slotType, _reward);
    }

    public void SetSlotData(PlinkoSlotType slotType, float reward)
    {
        _slotType = slotType;
        _reward = reward;
        _rewardText.text = slotType == PlinkoSlotType.Reward ? $"+{_reward}" : "x" + reward.ToString("F2");
    }
}

public enum PlinkoSlotType
{
    Multiplier,
    Reward
}
