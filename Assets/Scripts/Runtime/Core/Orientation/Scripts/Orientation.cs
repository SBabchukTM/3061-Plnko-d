using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Orientation : MonoBehaviour
{
    [SerializeField] private GameObject _hint;

    [SerializeField] private Button _fade;

    [SerializeField] private bool _isFadeClickable;

    [SerializeField] private bool _portrait;
    [SerializeField] private bool _landscape = true;

    private void Awake()
    {
        if (_isFadeClickable)
            _fade.onClick.AddListener(SetOrientation);

        if (InCorrectOrientation())
        {
            SetOrientation();
            return;
        }

        ShowHint();
    }

    private void Update()
    {
        if (InCorrectOrientation())
            SetOrientation();
    }

    private void ShowHint()
    {
        _hint.SetActive(true);
    }

    private bool InCorrectOrientation()
    {
        return IsInLandscape() || IsInPortrait();
    }

    private bool IsInLandscape()
    {
        if (!_landscape)
            return false;

        var currentOrientation = Screen.orientation;
        var correctOrientation =
            currentOrientation is ScreenOrientation.LandscapeLeft or ScreenOrientation.LandscapeRight;

        return correctOrientation;
    }

    private bool IsInPortrait()
    {
        if (!_portrait)
            return false;

        var currentOrientation = Screen.orientation;
        var correctOrientation =
            currentOrientation is ScreenOrientation.Portrait or ScreenOrientation.PortraitUpsideDown;

        return correctOrientation;
    }

    private void SetOrientation()
    {
        Screen.autorotateToPortrait = _portrait;
        Screen.autorotateToPortraitUpsideDown = _portrait;
        Screen.autorotateToLandscapeLeft = _landscape;
        Screen.autorotateToLandscapeRight = _landscape;

        if (_portrait)
            Screen.orientation = ScreenOrientation.Portrait;
        else if (_landscape)
            Screen.orientation = ScreenOrientation.LandscapeLeft;

        if (InCorrectOrientation())
            SetAutoRotation();
        else
            StartCoroutine(EnableAutoRotationAfterDelay());
    }

    private IEnumerator EnableAutoRotationAfterDelay()
    {
        yield return null;
        SetAutoRotation();
    }

    private void SetAutoRotation()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        gameObject.SetActive(false);
    }
}
