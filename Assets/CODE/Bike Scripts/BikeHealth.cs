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

    bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (wastedText != null)
        {
            wastedText.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

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