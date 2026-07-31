using UnityEngine;

public class MegaMapArrow : MonoBehaviour
{
    public Transform player;

    public RectTransform mapRect;
    public RectTransform arrowRect;

    public BoxCollider worldBounds;

    Vector2 startArrowPosition;

    void Start()
    {
        startArrowPosition = arrowRect.anchoredPosition;
    }

    void Update()
    {
        if (player == null || mapRect == null || worldBounds == null)
            return;

        Bounds bounds = worldBounds.bounds;

        float x = Mathf.InverseLerp(
            bounds.min.x,
            bounds.max.x,
            player.position.x);

        float y = Mathf.InverseLerp(
            bounds.min.z,
            bounds.max.z,
            player.position.z);

        arrowRect.anchoredPosition = startArrowPosition + new Vector2(
     (x - 0.5f) * mapRect.rect.width,
     (y - 0.5f) * mapRect.rect.height
 );

        arrowRect.localEulerAngles = new Vector3(
            0f,
            0f,
            -player.eulerAngles.y
        );
    }
}