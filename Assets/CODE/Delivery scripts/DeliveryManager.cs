using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DeliveryManager : MonoBehaviour
{
    [Header("Main Timer")]
    public float mainLevelTime = 300f;

    float currentMainTime;

    public TMP_Text mainTimerText;

    [Header("Customers")]
    public CustomerDelivery[] customers;

    [Header("Message UI")]
    public GameObject messagePanel;

    public TMP_Text customerNameText;
    public TMP_Text customerMessageText;

    public Image customerImageUI;

    [Header("Summary")]
    public GameObject summaryPanel;

    public TMP_Text starsText;
    public TMP_Text deliveredText;
    public TMP_Text failedText;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip messageSound;
    public AudioClip successSound;
    public AudioClip failSound;

    int totalStars;
    int deliveredCount;
    int failedCount;

    bool levelEnded;

    Coroutine messageRoutine;

    bool messageAudioLock;

    public static DeliveryManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentMainTime = mainLevelTime;

        messagePanel.SetActive(false);
        summaryPanel.SetActive(false);

        foreach (CustomerDelivery customer in customers)
        {
            customer.currentTime = customer.maxTime;
            customer.hurryShown = false;

            if (customer.targetHouse != null)
            {
                customer.targetHouse.deliveryManager = this;
                customer.targetHouse.customer = customer;
            }

            // Hide persistent customer images at start
            if (customer.persistentImageUI != null)
            {
                customer.persistentImageUI.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (levelEnded)
            return;

        UpdateMainTimer();
        UpdateCustomerTimers();
    }

    void UpdateMainTimer()
    {
        currentMainTime -= Time.deltaTime;

        if (currentMainTime < 0)
            currentMainTime = 0;

        int minutes = Mathf.FloorToInt(currentMainTime / 60);
        int seconds = Mathf.FloorToInt(currentMainTime % 60);

        mainTimerText.text = $"{minutes:00}:{seconds:00}";

        float percent = currentMainTime / mainLevelTime;
        mainTimerText.color = GetTimerColor(percent);

        if (currentMainTime <= 0)
            EndLevel();
    }

    void UpdateCustomerTimers()
    {
        foreach (CustomerDelivery customer in customers)
        {
            if (customer.delivered || customer.failed)
                continue;

            customer.currentTime -= Time.deltaTime;

            if (customer.timerBar != null)
            {
                float percent =
                    customer.currentTime / customer.maxTime;

                customer.timerBar.value = percent;

                Image fillImage =
                    customer.timerBar.fillRect.GetComponent<Image>();

                fillImage.color = GetTimerColor(percent);
            }

            // HURRY MESSAGE
            if (!customer.hurryShown &&
                customer.currentTime <= customer.maxTime * 0.5f)
            {
                customer.hurryShown = true;

                ShowMessage(customer, customer.hurryMessage, true);
            }

            // FAIL
            if (customer.currentTime <= 0)
            {
                customer.failed = true;
                failedCount++;

                PlaySoundSafe(failSound, 0.7f);

                ShowMessage(customer, customer.angryMessage, false);
            }
        }
    }

    public void CompleteDelivery(CustomerDelivery customer)
    {
        if (customer.delivered || customer.failed)
            return;

        customer.delivered = true;
        deliveredCount++;

        float percent =
            customer.currentTime / customer.maxTime;

        int stars = CalculateStars(percent);

        totalStars += stars;

        PlaySoundSafe(successSound, 0.6f);

        // SHOW PERSISTENT CUSTOMER IMAGE
        if (customer.persistentImageUI != null)
        {
       
            Debug.Log("Showing image for " + customer.customerName);
            customer.persistentImageUI.gameObject.SetActive(true);
        }

        ShowMessage(
            customer,
            customer.successMessage + "\nStars: " + stars,
            false
        );

        StartCoroutine(CheckEndAfterMessage());
    }

    void ShowMessage(
        CustomerDelivery customer,
        string message,
        bool playSound
    )
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine =
            StartCoroutine(MessagePopup(customer, message));

        if (playSound)
            PlayMessageSound();
    }

    IEnumerator MessagePopup(CustomerDelivery customer, string message)
    {
        messagePanel.SetActive(true);

        customerNameText.text = customer.customerName;
        customerMessageText.text = message;

        customerImageUI.sprite = customer.customerImage;

        yield return new WaitForSeconds(2f);

        messagePanel.SetActive(false);
    }

    void PlayMessageSound()
    {
        if (messageAudioLock)
            return;

        StartCoroutine(MessageSoundCooldown());
    }

    IEnumerator MessageSoundCooldown()
    {
        messageAudioLock = true;

        PlaySoundSafe(messageSound, 1f);

        yield return new WaitForSeconds(0.2f);

        messageAudioLock = false;
    }

    void PlaySoundSafe(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    IEnumerator CheckEndAfterMessage()
    {
        yield return new WaitForSeconds(2.2f);

        CheckLevelEnd();
    }

    void CheckLevelEnd()
    {
        int finished = 0;

        foreach (CustomerDelivery customer in customers)
        {
            if (customer.delivered || customer.failed)
            {
                finished++;
            }
        }

        if (finished >= customers.Length)
        {
            EndLevel();
        }
    }

    void EndLevel()
    {
        if (levelEnded)
            return;

        levelEnded = true;

        summaryPanel.SetActive(true);

        starsText.text = "Stars: " + totalStars;
        deliveredText.text = "Delivered: " + deliveredCount;
        failedText.text = "Failed: " + failedCount;
    }

    int CalculateStars(float percent)
    {
        if (percent >= 0.8f) return 5;
        if (percent >= 0.6f) return 4;
        if (percent >= 0.4f) return 3;
        if (percent >= 0.2f) return 2;

        return 1;
    }

    Color GetTimerColor(float percent)
    {
        Color white = Color.white;
        Color orange = new Color(1f, 0.5f, 0f);
        Color red = Color.red;

        if (percent > 0.5f)
        {
            float t = (1f - percent) / 0.5f;
            return Color.Lerp(white, orange, t);
        }

        float t2 = (0.5f - percent) / 0.5f;
        return Color.Lerp(orange, red, t2);
    }
}