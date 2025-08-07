using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class LeaderboardScreen : UiScreen
    {
        [SerializeField] private SimpleButton _backButton;
        [SerializeField] private RectTransform _recordsParent;

        public event Action OnBackPressed;

        private void OnDestroy()
        {
            _backButton.Button.onClick.RemoveAllListeners();
        }

        public void Initialize(List<LeaderboardRecordDisplay> records)
        {
            ParentRecords(records);
            SubscribeToEvents();
        }

        private void ParentRecords(List<LeaderboardRecordDisplay> records)
        {
            foreach (var record in records)
                record.transform.SetParent(_recordsParent, false);
        }

        private void SubscribeToEvents()
        {
            _backButton.Button.onClick.AddListener(() => OnBackPressed?.Invoke());
        }
    }
}