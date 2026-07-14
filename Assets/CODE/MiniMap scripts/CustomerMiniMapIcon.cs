using UnityEngine;

public class CustomerMinimapIcon : MonoBehaviour
{
    public Transform target;
    public RectTransform iconRect;

    public Vector2 worldMin;
    public Vector2 worldMax;

    public RectTransform mapImage;

    void Start()
    {
        float x = Mathf.InverseLerp(
            worldMin.x,
            worldMax.x,
            target.position.x);

        float y = Mathf.InverseLerp(
            worldMin.y,
            worldMax.y,
            target.position.z);

        iconRect.anchoredPosition = new Vector2(
            (x - .5f) * mapImage.rect.width,
            (y - .5f) * mapImage.rect.height
        );
    }
}