using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BikeHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthSlider;
    public TMP_Text healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI();

        Debug.Log("Bike Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Bike Destroyed!");
        }
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