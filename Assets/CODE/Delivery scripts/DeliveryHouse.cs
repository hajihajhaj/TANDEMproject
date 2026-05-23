using UnityEngine;

public class DeliveryHouse : MonoBehaviour
{
    [HideInInspector] public DeliveryManager deliveryManager;
    [HideInInspector] public CustomerDelivery customer;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Package"))
            return;

        if (deliveryManager == null || customer == null)
            return;

        deliveryManager.CompleteDelivery(customer);

     
        Destroy(other.gameObject);
    }
}