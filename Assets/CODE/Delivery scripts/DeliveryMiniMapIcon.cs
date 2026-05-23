using UnityEngine;
using UnityEngine.UI;

public class DeliveryMiniMapIcon : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    public Transform target;

    [Header("MiniMap")]
    public RectTransform miniMapRect;
    public RectTransform iconRect;

    [Header("Settings")]
    public float mapWorldSize = 120f;

    // lets icon go slightly outside minimap
    public float edgeOffset = 25f;

    void Update()
    {
        if (player == null ||
            target == null ||
            miniMapRect == null)
            return;

        Vector3 offset =
            target.position - player.position;

        float x =
            offset.x / mapWorldSize;

        float y =
            offset.z / mapWorldSize;

        float mapWidth =
            miniMapRect.rect.width * 0.5f;

        float mapHeight =
            miniMapRect.rect.height * 0.5f;

        Vector2 position =
            new Vector2(
                x * mapWidth,
                y * mapHeight
            );

        bool insideMap =
            Mathf.Abs(position.x) <= mapWidth &&
            Mathf.Abs(position.y) <= mapHeight;

        // HOUSE IS INSIDE MAP
        if (insideMap)
        {
            iconRect.anchoredPosition =
                position;
        }
        else
        {
            // CLAMP TO EDGE
            Vector2 dir =
                position.normalized;

            float clampedX =
                dir.x * (mapWidth + edgeOffset);

            float clampedY =
                dir.y * (mapHeight + edgeOffset);

            iconRect.anchoredPosition =
                new Vector2(
                    clampedX,
                    clampedY
                );
        }
    }
}