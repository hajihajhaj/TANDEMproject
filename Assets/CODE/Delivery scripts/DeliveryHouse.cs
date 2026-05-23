using UnityEngine;

public class DeliveryHouse : MonoBehaviour
{
    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public CustomerDelivery customer;
    [HideInInspector]
    public GameObject miniMapIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Package"))
            return;

        if (deliveryManager == null || customer == null)
            return;

        deliveryManager.CompleteDelivery(customer);

        if (miniMapIcon != null)
        {
            miniMapIcon.SetActive(false);
        }
        Destroy(other.gameObject);
    }
}