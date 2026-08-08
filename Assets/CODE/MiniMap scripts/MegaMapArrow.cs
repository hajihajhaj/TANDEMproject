using UnityEngine;

public class MegaMapArrow : MonoBehaviour
{
    public Transform player;

    [Header("Mega Map")]
    public RectTransform mapRect;
    public RectTransform arrowRect;

    [Header("Mini Map")]
    public RectTransform miniMapImage;
    public RectTransform miniMapArrow;

    [Tooltip("Increase if the minimap barely moves.")]
    public float miniMapMultiplier = 10f;

    public BoxCollider worldBounds;

    Vector2 startArrowPosition;
    Vector2 startMiniMapPosition;

    Vector3 startPlayerPosition;

    void Start()
    {
        // Remember exactly where you placed the Mega Map arrow
        startArrowPosition = arrowRect.anchoredPosition;

        // Remember exactly where you placed the Mini Map image
        if (miniMapImage != null)
            startMiniMapPosition = miniMapImage.anchoredPosition;

        // Remember where the player/bike is when the game starts
        startPlayerPosition = player.position;
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

        Vector2 mapOffset = new Vector2(
            (x - 0.5f) * mapRect.rect.width,
            (y - 0.5f) * mapRect.rect.height
        );

        // Mega Map Arrow
        arrowRect.anchoredPosition = startArrowPosition + mapOffset;

        arrowRect.localEulerAngles = new Vector3(
            0f,
            0f,
            -player.eulerAngles.y
        );

        // Mini Map Image
        if (miniMapImage != null)
        {
            // Calculate where the player started on the map
            float startX = Mathf.InverseLerp(
                bounds.min.x,
                bounds.max.x,
                startPlayerPosition.x);

            float startY = Mathf.InverseLerp(
                bounds.min.z,
                bounds.max.z,
                startPlayerPosition.z);

            Vector2 startMapOffset = new Vector2(
                (startX - 0.5f) * mapRect.rect.width,
                (startY - 0.5f) * mapRect.rect.height
            );

            // Only move the minimap relative to where it started
            Vector2 movementOffset = mapOffset - startMapOffset;

            Vector2 miniOffset = movementOffset * miniMapMultiplier;

            miniMapImage.anchoredPosition =
                startMiniMapPosition - miniOffset;
        }

        // Mini Map Arrow
        if (miniMapArrow != null)
        {
            miniMapArrow.localEulerAngles = new Vector3(
                0f,
                0f,
                -player.eulerAngles.y
            );
        }
    }
}