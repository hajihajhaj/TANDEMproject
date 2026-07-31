using UnityEngine;

public class DeliveryPackage : MonoBehaviour
{
    [Header("Impact VFX")]
    public GameObject impactPrefab;
    public float destroyDelay = 2f;

    [Header("Impact SFX")]
    public AudioClip impactSFX;
    [Range(0f, 1f)]
    public float impactVolume = 1f;

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

        ContactPoint hit = collision.contacts[0];

        // Spawn VFX
        if (impactPrefab != null)
        {
            GameObject impact = Instantiate(
                impactPrefab,
                hit.point,
                Quaternion.LookRotation(hit.normal)
            );

            Destroy(impact, destroyDelay);
        }

        // Play SFX
        if (impactSFX != null)
        {
            AudioSource.PlayClipAtPoint(
                impactSFX,
                hit.point,
                impactVolume
            );
        }
    }
}