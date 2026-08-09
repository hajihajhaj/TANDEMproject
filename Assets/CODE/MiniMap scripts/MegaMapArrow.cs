using UnityEngine;

public class MegaMapArrow : MonoBehaviour
{
    public Transform player;

    [Header("Mega Map")]
    public RectTransform mapRect;
    public RectTransform arrowRect;

    [Tooltip("Controls the Mega Map arrow's left/right movement.")]
    public float megaMapXMultiplier = 10f;

    [Tooltip("Controls the Mega Map arrow's up/down movement.")]
    public float megaMapYMultiplier = 10f;

    [Header("Mini Map")]
    public RectTransform miniMapImage;
    public RectTransform miniMapArrow;

    [Tooltip("Controls how much the Mini Map moves.")]
    public float miniMapMultiplier = 10f;

    public BoxCollider worldBounds;

    Vector3 lastPlayerPosition;

    void Start()
    {
        // Remember where the player starts.
        if (player != null)
            lastPlayerPosition = player.position;
    }

    void Update()
    {
        if (player == null || worldBounds == null)
            return;

        Bounds bounds = worldBounds.bounds;

        // =========================================
        // PLAYER MOVEMENT
        // =========================================

        Vector3 playerMovement =
            player.position - lastPlayerPosition;

        lastPlayerPosition = player.position;

        // =========================================
        // NORMALIZED WORLD MOVEMENT
        // =========================================

        float normalizedMovementX =
            playerMovement.x / bounds.size.x;

        float normalizedMovementY =
            playerMovement.z / bounds.size.z;

        // =========================================
        // MEGA MAP ARROW
        // =========================================

        if (arrowRect != null && mapRect != null)
        {
            float megaX =
                normalizedMovementX *
                mapRect.rect.width *
                megaMapXMultiplier;

            float megaY =
                normalizedMovementY *
                mapRect.rect.height *
                megaMapYMultiplier;

            arrowRect.anchoredPosition += new Vector2(
                megaX,
                megaY
            );

            // Rotate Mega Map arrow
            arrowRect.localEulerAngles = new Vector3(
                0f,
                0f,
                -player.eulerAngles.y
            );
        }

        // =========================================
        // MINI MAP IMAGE
        // =========================================

        if (miniMapImage != null)
        {
            Vector2 miniMapMovement = new Vector2(
                normalizedMovementX *
                miniMapImage.rect.width *
                miniMapMultiplier,

                normalizedMovementY *
                miniMapImage.rect.height *
                miniMapMultiplier
            );

            // Move the map opposite the player.
            miniMapImage.anchoredPosition -= miniMapMovement;
        }

        // =========================================
        // MINI MAP ARROW
        // =========================================

        if (miniMapArrow != null)
        {
            // Don't move the arrow.
            // Only rotate it.
            miniMapArrow.localEulerAngles = new Vector3(
                0f,
                0f,
                -player.eulerAngles.y
            );
        }
    }
}