using System;
using UnityEngine;

public class PyramidController : MonoBehaviour
{
    [SerializeField] private PlinkoSlot[] _slots;
    [SerializeField] private BallController _ball;
    [SerializeField] private Transform _ballSpawnPos;
    [SerializeField] private GameObject[] _gameObjects;

    public Transform PyramidTransform;

    public event Action<PlinkoSlotType, float> OnGameEnded;
    public event Action OnPlayerStartedGame;

    private void Start()
    {
        foreach (var slot in _slots)
            slot.OnBallCollision += NotityOnCollision;

        _ball.OnPlayerStartedGame += NotifyOnGameStarted;
    }

    private void OnDestroy()
    {
        foreach (var slot in _slots)
            slot.OnBallCollision -= NotityOnCollision;

        _ball.OnPlayerStartedGame -= NotifyOnGameStarted;
    }

    public void SetSlotsData(LevelConfig levelConfig)
    {
        int middleIndex = _slots.Length / 2 + (_slots.Length % 2 == 0 ? 0 : 1);
        for (int i = 0; i < middleIndex; i++)
        {
            _slots[i].SetSlotData(levelConfig.SlotRewardsType, levelConfig.SlotRewards[i]);
            _slots[_slots.Length - 1 - i].SetSlotData(levelConfig.SlotRewardsType, levelConfig.SlotRewards[i]);
        }
    }

    public void SetBallSprite(Sprite sprite) => _ball.SetSprite(sprite);

    public void StartGame()
    {
        foreach (var go in _gameObjects)
            go.SetActive(true);

        _ball.StartGame(_ballSpawnPos.position);
    }

    public void EndGame()
    {
        foreach (var go in _gameObjects)
            go.SetActive(false);
    }

    private void NotityOnCollision(PlinkoSlotType slotType, float reward) => OnGameEnded?.Invoke(slotType, reward);
    private void NotifyOnGameStarted() => OnPlayerStartedGame?.Invoke();
}
