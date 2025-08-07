using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Core.Services.Audio;
using Runtime.Services.Audio;

public class BallController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _userLaunchForce = 5;
    [SerializeField] private float _collisionForceMax = 5;
    [SerializeField] private float _collSoundCD = 0.15f;

    private bool _falling = false;
    private bool _touchEnded = false;
    private Vector3 _startTouchPos;
    private Vector3 _endTouchPos;
    private float _lastCollSoundTime = 0;

    public event Action OnPlayerStartedGame;

    private IAudioService _audioService;

    [Inject]
    private void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }

    private void Update()
    {
        if (_falling || !AnyInput())
            return;

        Touch touch = Input.GetTouch(0);
        GetTouchStartPos(touch);
        GetTouchEndPosition(touch);
        LaunchBallOnTouchEnd();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Time.time > _lastCollSoundTime + _collSoundCD)
        {
            _audioService.PlaySound(ConstAudio.CollisionSound);
            _lastCollSoundTime = Time.time;
        }

        _rb.AddForce(UnityEngine.Random.value * _collisionForceMax * UnityEngine.Random.insideUnitCircle);
    }

    public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;    

    public void StartGame(Vector3 position)
    {
        transform.position = position;
        _rb.isKinematic = true;
        _falling = false;
        _touchEnded = false;
    }

    private bool AnyInput() => Input.touchCount > 0;

    private void GetTouchStartPos(Touch touch)
    {
        if (touch.phase == TouchPhase.Began && !IsPointerOverUIElement())
            _startTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
    }

    private void GetTouchEndPosition(Touch touch)
    {
        if(touch.phase == TouchPhase.Ended && !IsPointerOverUIElement())
        {
            _endTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
            _touchEnded = true;
        }
    }

    private void LaunchBallOnTouchEnd()
    {
        if (!_touchEnded)
            return;

        _falling = true;
        _rb.isKinematic = false;
        _rb.AddForce((_endTouchPos - _startTouchPos).normalized * _userLaunchForce);
        OnPlayerStartedGame?.Invoke();
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }

    private List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
