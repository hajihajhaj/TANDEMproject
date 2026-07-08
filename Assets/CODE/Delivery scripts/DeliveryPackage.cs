using UnityEngine;

public class DeliveryPackage : MonoBehaviour
{
    [Header("Impact VFX")]
    public GameObject impactPrefab;
    public float destroyDelay = 2f;

    private bool hasImpacted = false;

    void OnCollisionEnter(Collision collision)
    {
        // Ignore the bike
        if (collision.gameObject.CompareTag("Player"))
            return;

        // Already played once
        if (hasImpacted)
            return;

        hasImpacted = true;

        if (impactPrefab != null)
        {
            ContactPoint hit = collision.contacts[0];

            GameObject impact = Instantiate(
                impactPrefab,
                hit.point,
                Quaternion.LookRotation(hit.normal)
            );

            Destroy(impact, destroyDelay);
        }
    }
}