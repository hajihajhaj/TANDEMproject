using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BikeHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthSlider;
    public TMP_Text healthText;

    [Header("Respawn")]
    public Transform respawnPoint;
    public float respawnDelay = 2f;

    [Header("Wasted UI")]
    public TMP_Text wastedText;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float damageOverlayAlpha = 0.5f;
    public float damageFadeSpeed = 2f;

    Coroutine damageFadeRoutine;

    bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (wastedText != null)
        {
            wastedText.gameObject.SetActive(false);
        }

        if (damageOverlay != null)
        {
            Color c = damageOverlay.color;
            c.a = 0f;
            damageOverlay.color = c;
        }

        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        ShowDamageOverlay();

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI();

        Debug.Log("Bike Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(RespawnBike());
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI();

        Debug.Log("Bike Healed: " + currentHealth);
    }

    public void ShowDamageOverlay()
    {
        if (damageOverlay == null) return;

        if (damageFadeRoutine != null)
        {
            StopCoroutine(damageFadeRoutine);
            damageFadeRoutine = null;
        }

        Color c = damageOverlay.color;
        c.a = damageOverlayAlpha;
        damageOverlay.color = c;

        StopCoroutine(nameof(AutoHideDamageOverlay));
        StartCoroutine(nameof(AutoHideDamageOverlay));
    }

    public void HideDamageOverlay()
    {
        if (damageOverlay == null) return;

        if (damageFadeRoutine != null)
        {
            StopCoroutine(damageFadeRoutine);
        }

        damageFadeRoutine = StartCoroutine(FadeDamageOverlay());
    }

    IEnumerator AutoHideDamageOverlay()
    {
        yield return new WaitForSeconds(0.5f);

        HideDamageOverlay();
    }

    IEnumerator FadeDamageOverlay()
    {
        Color c = damageOverlay.color;

        while (c.a > 0f)
        {
            c.a -= damageFadeSpeed * Time.deltaTime;
            c.a = Mathf.Clamp01(c.a);

            damageOverlay.color = c;

            yield return null;
        }

        damageFadeRoutine = null;
    }

    IEnumerator RespawnBike()
    {
        isDead = true;

        if (wastedText != null)
        {
            wastedText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(respawnDelay);

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
        }

        currentHealth = maxHealth;
        UpdateUI();

        if (wastedText != null)
        {
            wastedText.gameObject.SetActive(false);
        }

        isDead = false;
    }

    void UpdateUI()
    {
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        if (healthText)
        {
            healthText.text = Mathf.RoundToInt(currentHealth).ToString();
        }
    }
}