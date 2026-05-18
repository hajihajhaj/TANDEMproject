using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Manhole : MonoBehaviour
{
    [Header("UI")]
    public Image smallSmokeImage;
    public Image bigSmokeImage;

    [Header("Bike Health")]
    [SerializeField] private BikeHealth bikeHealth;

    [SerializeField] private float triggerDamagePerTick = 2f;
    [SerializeField] private float collisionDamagePerTick = 5f;

    [SerializeField] private float tickInterval = 1f;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.3f;

    bool inTriggerArea;
    bool inCollision;

    Coroutine damageRoutine;
    Coroutine smallFadeRoutine;
    Coroutine bigFadeRoutine;

    void Start()
    {
        if (smallSmokeImage != null)
        {
            smallSmokeImage.gameObject.SetActive(false);

            Color c = smallSmokeImage.color;
            c.a = 0f;
            smallSmokeImage.color = c;
        }

        if (bigSmokeImage != null)
        {
            bigSmokeImage.gameObject.SetActive(false);

            Color c = bigSmokeImage.color;
            c.a = 0f;
            bigSmokeImage.color = c;
        }

        // AUTO FIND BIKE HEALTH IF NOT ASSIGNED
        if (bikeHealth == null)
        {
            bikeHealth = FindObjectOfType<BikeHealth>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inTriggerArea = true;

        UpdateDamageRoutine();

        if (smallSmokeImage != null)
        {
            StartFade(
                ref smallFadeRoutine,
                smallSmokeImage,
                1f
            );
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inTriggerArea = false;

        UpdateDamageRoutine();

        if (smallSmokeImage != null)
        {
            StartFade(
                ref smallFadeRoutine,
                smallSmokeImage,
                0f,
                true
            );
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        inCollision = true;

        UpdateDamageRoutine();

        if (bigSmokeImage != null)
        {
            StartFade(
                ref bigFadeRoutine,
                bigSmokeImage,
                1f
            );
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        inCollision = false;

        UpdateDamageRoutine();

        if (bigSmokeImage != null)
        {
            StartFade(
                ref bigFadeRoutine,
                bigSmokeImage,
                0f,
                true
            );
        }
    }

    void UpdateDamageRoutine()
    {
        if (inTriggerArea || inCollision)
        {
            if (damageRoutine == null)
            {
                damageRoutine =
                    StartCoroutine(DamageLoop());
            }
        }
        else
        {
            if (damageRoutine != null)
            {
                StopCoroutine(damageRoutine);
                damageRoutine = null;
            }
        }
    }

    IEnumerator DamageLoop()
    {
        while (inTriggerArea || inCollision)
        {
            if (bikeHealth != null)
            {
                float damage =
                    inCollision
                    ? collisionDamagePerTick
                    : triggerDamagePerTick;

                bikeHealth.TakeDamage(damage);
            }

            yield return new WaitForSeconds(tickInterval);
        }

        damageRoutine = null;
    }

    void StartFade(
        ref Coroutine routine,
        Image img,
        float targetAlpha,
        bool disableOnEnd = false
    )
    {
        if (img == null)
            return;

        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(
            FadeImage(
                img,
                targetAlpha,
                disableOnEnd
            )
        );
    }

    IEnumerator FadeImage(
        Image img,
        float targetAlpha,
        bool disableOnEnd
    )
    {
        img.gameObject.SetActive(true);

        Color c = img.color;

        float startAlpha = c.a;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float t =
                fadeDuration > 0f
                ? time / fadeDuration
                : 1f;

            c.a =
                Mathf.Lerp(
                    startAlpha,
                    targetAlpha,
                    t
                );

            img.color = c;

            yield return null;
        }

        c.a = targetAlpha;
        img.color = c;

        if (disableOnEnd &&
            Mathf.Approximately(targetAlpha, 0f))
        {
            img.gameObject.SetActive(false);
        }
    }
}