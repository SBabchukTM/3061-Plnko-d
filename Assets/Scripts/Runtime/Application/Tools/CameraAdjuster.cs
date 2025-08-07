using System.Collections;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] private RectTransform _gameplayAreaUI;
    [SerializeField] private Transform _physicObject;

    [SerializeField] private float tableWidth;
    [SerializeField] private float tableHeight;
    [SerializeField] private Camera cam;

    public void Initial(Transform physicObject)
    {
        _physicObject = physicObject;
        cam = Camera.main;
        DetectTableSize();
        StartCoroutine(AdjustCameraCoroutine());
    }

    private IEnumerator AdjustCameraCoroutine()
    {
        Canvas.ForceUpdateCanvases();
        yield return null;
        yield return new WaitForEndOfFrame();

        if (cam == null || _gameplayAreaUI == null)
            yield break;

        float originalZ = cam.transform.position.z;

        cam.transform.position = new Vector3(0f, 0f, originalZ);

        Vector3[] worldCorners = new Vector3[4];
        _gameplayAreaUI.GetWorldCorners(worldCorners);

        float left = worldCorners[0].x;
        float right = worldCorners[2].x;
        float bottom = worldCorners[0].y;
        float top = worldCorners[1].y;

        float areaWidth = right - left;
        float areaHeight = top - bottom;

        float areaAspect = areaWidth / areaHeight;
        float tableAspect = tableWidth / tableHeight;

        if (areaAspect > tableAspect)
        {
            cam.orthographicSize = tableHeight / 1f;
        }
        else
        {
            float targetHeight = tableWidth / areaAspect;
            cam.orthographicSize = targetHeight / 1;
        }

        Vector2 centerScreen = new Vector2((left + right) / 2f, (bottom + top) / 2f);
        Vector3 worldCenter = cam.ScreenToWorldPoint(new Vector3(centerScreen.x, centerScreen.y, cam.nearClipPlane));

        cam.transform.position = new Vector3(-worldCenter.x, -worldCenter.y, originalZ);
    }


    void DetectTableSize()
    {
        if (_physicObject == null)
            return;

        BoxCollider2D collider = _physicObject.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            tableWidth = collider.size.x;
            tableHeight = collider.size.y;
            return;
        }

        SpriteRenderer sprite = _physicObject.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            tableWidth = sprite.bounds.size.x;
            tableHeight = sprite.bounds.size.y;
        }
    }
}