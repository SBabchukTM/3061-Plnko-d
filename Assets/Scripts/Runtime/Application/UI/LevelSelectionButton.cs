using Runtime.UI;
using System;
using TMPro;
using UnityEngine;

public class LevelSelectionButton : SimpleButton
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _betText;

    private int _level;

    public event Action<int> OnButtonPressed;

    public void Initialize(int bet, int level, bool locked)
    {
        _level = level;
        _levelText.text = "Level " + (level + 1);
        _betText.text = bet.ToString();

        if (!locked)
        {
            Button.onClick.AddListener(() =>
            {
                PlayPressAnimation();
                OnButtonPressed?.Invoke(_level);
            });
        }
        else
            _betText.color = Color.magenta;
    }

    public void SetButtonColor(Color color) => Button.image.color = color;
}
