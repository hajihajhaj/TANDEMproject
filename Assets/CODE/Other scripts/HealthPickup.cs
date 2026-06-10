using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float healAmount = 35f;
    public float respawnTime = 15f;

    [Header("Audio")]
    public AudioClip disappearSound;

    private Collider pickupCollider;
    private Renderer pickupRenderer;

    private void Start()
    {
        pickupCollider = GetComponent<Collider>();
        pickupRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        BikeHealth bikeHealth = other.GetComponentInParent<BikeHealth>();

        if (bikeHealth == null)
            return;

        // Heal the bike (will never go above max health)
        bikeHealth.Heal(healAmount);

        if (disappearSound != null)
        {
            AudioSource.PlayClipAtPoint(disappearSound, transform.position);
        }

        StartCoroutine(RespawnPickup());
    }

    private IEnumerator RespawnPickup()
    {
        pickupCollider.enabled = false;

        if (pickupRenderer != null)
            pickupRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        pickupCollider.enabled = true;

        if (pickupRenderer != null)
            pickupRenderer.enabled = true;
    }
}