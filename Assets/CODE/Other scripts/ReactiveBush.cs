using UnityEngine;
using System.Collections;

public class BushReaction : MonoBehaviour
{
    public ParticleSystem leafPrefab;

    public Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);

    public float cooldown = 0.5f;

    bool canPlay = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canPlay)
            return;

        ParticleSystem leaves =
            Instantiate(
                leafPrefab,
                transform.position + spawnOffset,
                Quaternion.identity
            );

        leaves.Play();

        Destroy(
            leaves.gameObject,
            leaves.main.duration +
            leaves.main.startLifetime.constantMax
        );

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canPlay = false;

        yield return new WaitForSeconds(cooldown);

        canPlay = true;
    }
}