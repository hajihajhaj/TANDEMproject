using UnityEngine;

public class DeliveryPackage : MonoBehaviour
{
    [Header("Impact VFX")]
    public GameObject impactPrefab;

    public float destroyDelay = 2f;

    void OnCollisionEnter(Collision collision)
    {
        // Ignore the bike
        if (collision.gameObject.CompareTag("Player"))
            return;

        if (impactPrefab != null)
        {
            ContactPoint hit = collision.contacts[0];

            GameObject impact =
                Instantiate(
                    impactPrefab,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

            Destroy(impact, destroyDelay);
        }
    }
}