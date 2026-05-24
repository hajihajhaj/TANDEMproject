using UnityEngine;
using UnityEngine.UI;

public class CustomerMinimapIcon : MonoBehaviour
{
    [Header("References")]
    public RectTransform minimapRect;
    public RectTransform iconRect;

    public Transform target;

    public Camera minimapCamera;

    public DeliveryHouse deliveryHouse;

    [Header("Arrow")]
    public Image edgeArrow;

    [Header("Settings")]
    public float edgePadding = 12f;

    Vector2 minimapHalfSize;

    void Start()
    {
        minimapHalfSize =
            minimapRect.sizeDelta * 0.5f;

        if (edgeArrow != null)
        {
            edgeArrow.rectTransform.rotation =
                Quaternion.identity;
        }
    }

    void Update()
    {
        if (target == null ||
            minimapCamera == null)
            return;

        // HIDE AFTER DELIVERY
        if (deliveryHouse != null &&
            deliveryHouse.customer != null)
        {
            if (deliveryHouse.customer.delivered ||
                deliveryHouse.customer.failed)
            {
                iconRect.gameObject.SetActive(false);

                if (edgeArrow != null)
                    edgeArrow.gameObject.SetActive(false);

                return;
            }
        }

        Vector3 viewportPos =
            minimapCamera.WorldToViewportPoint(
                target.position
            );

        bool inView =
            viewportPos.z > 0 &&
            viewportPos.x > 0 &&
            viewportPos.x < 1 &&
            viewportPos.y > 0 &&
            viewportPos.y < 1;

        // POSITION ON MAP
        Vector2 mapPosition =
     new Vector2(
         (viewportPos.x * minimapRect.sizeDelta.x)
         - minimapHalfSize.x,

         (viewportPos.y * minimapRect.sizeDelta.y)
         - minimapHalfSize.y
     );

        float maxX =
    minimapHalfSize.x - edgePadding;

        float maxY =
            minimapHalfSize.y - edgePadding;

        mapPosition.x =
            Mathf.Clamp(
                mapPosition.x,
                -maxX,
                maxX
            );

        mapPosition.y =
            Mathf.Clamp(
                mapPosition.y,
                -maxY,
                maxY
            );

        // ICON
        if (iconRect != null)
        {
            iconRect.anchoredPosition =
                mapPosition;

            iconRect.gameObject.SetActive(true);
        }

        // EDGE ARROW
        if (edgeArrow != null)
        {
            edgeArrow.rectTransform.anchoredPosition =
                mapPosition;

            edgeArrow.gameObject.SetActive(!inView);

            edgeArrow.rectTransform.rotation =
                Quaternion.identity;
        }
    }
}