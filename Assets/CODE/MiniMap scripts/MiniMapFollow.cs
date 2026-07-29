using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public RectTransform mapImage;

    [Tooltip("Drag your Streets Separated object (with the Box Collider) here.")]
    public BoxCollider worldBounds;

    void LateUpdate()
    {
        if (player == null || mapImage == null || worldBounds == null)
            return;

        Bounds bounds = worldBounds.bounds;

        // Convert player world position to 0-1 range
        float normalizedX = Mathf.InverseLerp(
            bounds.min.x,
            bounds.max.x,
            player.position.x);

        float normalizedY = Mathf.InverseLerp(
            bounds.min.z,
            bounds.max.z,
            player.position.z);

        // Convert to map coordinates
        float mapX = (normalizedX - 0.5f) * mapImage.rect.width;
        float mapY = (normalizedY - 0.5f) * mapImage.rect.height;

        // Move map so player stays centered
        mapImage.anchoredPosition = new Vector2(-mapX, -mapY);
    }
}