using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player;

    public RectTransform miniMapImage;
    public RectTransform miniMapArrow;

    public BoxCollider worldBounds;

    Vector2 imageStart;

    void Start()
    {
        imageStart = miniMapImage.anchoredPosition;
    }

    void LateUpdate()
    {
        Bounds bounds = worldBounds.bounds;

        float x = Mathf.InverseLerp(bounds.min.x, bounds.max.x, player.position.x);
        float y = Mathf.InverseLerp(bounds.min.z, bounds.max.z, player.position.z);

        miniMapImage.anchoredPosition = imageStart - new Vector2(
            (x - 0.5f) * miniMapImage.rect.width,
            (y - 0.5f) * miniMapImage.rect.height
        );

        miniMapArrow.localEulerAngles = new Vector3(
            0,
            0,
            -player.eulerAngles.y
        );
    }
}