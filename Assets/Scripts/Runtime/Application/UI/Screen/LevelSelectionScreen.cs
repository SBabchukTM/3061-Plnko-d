using System;
using UnityEngine;

namespace Runtime.UI
{
    public class LevelSelectionScreen : UiScreen
    {
        [SerializeField] private Color SelectColor;
        [SerializeField] private Color DeselectColor;
        [SerializeField] private Color LockedColor;

        [SerializeField] private SimpleButton _backButton;
        [SerializeField] private SimpleButton _playButton;

        [SerializeField] private LevelSelectionButton[] _levelSelectionButtons;

        public event Action OnBackPressed;
        public event Action<int> OnPlayPressed;

        private int _selectedLevel;

        private void OnDestroy()
        {
            _backButton.Button.onClick.RemoveAllListeners();
            _playButton.Button.onClick.RemoveAllListeners();
        }

        public void Initialize(GameLevelsConfig gameLevelsConfig, int balance)
        {
            _selectedLevel = 0;

            InitializeButton(gameLevelsConfig, balance);
            SubscribeToEvents();
        }

        private void InitializeButton(GameLevelsConfig gameLevelsConfig, int balance)
        {
            _levelSelectionButtons[0].SetButtonColor(SelectColor);

            for (int i = 0; i < _levelSelectionButtons.Length; i++)
            {
                int entryFee = gameLevelsConfig.LevelConfigs[i].Bet;
                bool locked = balance < entryFee;

                var button = _levelSelectionButtons[i];

                button.Initialize(entryFee, i, locked);
                button.OnButtonPressed += ProcessButtonClick;

                if (locked)
                    button.SetButtonColor(LockedColor);
            }
        }

        private void SubscribeToEvents()
        {
            _backButton.Button.onClick.AddListener(() => OnBackPressed?.Invoke());
            _playButton.Button.onClick.AddListener(() => OnPlayPressed?.Invoke(_selectedLevel));
        }

        private void ProcessButtonClick(int level)
        {
            _levelSelectionButtons[_selectedLevel].SetButtonColor(DeselectColor);
            _selectedLevel = level;
            _levelSelectionButtons[_selectedLevel].SetButtonColor(SelectColor);
        }
    }
}