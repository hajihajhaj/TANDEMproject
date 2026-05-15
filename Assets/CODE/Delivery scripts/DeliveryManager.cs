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

    int totalStars;
    int deliveredCount;
    int failedCount;

    bool levelEnded;

    Coroutine messageRoutine;

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

            if (customer.targetHouse != null)
            {
                customer.targetHouse.deliveryManager = this;
                customer.targetHouse.customer = customer;
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
        {
            EndLevel();
        }
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
            if (customer.currentTime < customer.maxTime * 0.5f &&
                customer.currentTime > customer.maxTime * 0.49f)
            {
                ShowMessage(customer, customer.hurryMessage);
            }

            // FAIL
            if (customer.currentTime <= 0)
            {
                customer.failed = true;
                failedCount++;

                ShowMessage(customer, customer.angryMessage);
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

        ShowMessage(
            customer,
            customer.successMessage + "\nStars: " + stars
        );

        StartCoroutine(CheckEndAfterMessage());
    }

    IEnumerator CheckEndAfterMessage()
    {
        yield return new WaitForSeconds(2.2f);

        CheckLevelEnd();
    }

    int CalculateStars(float percent)
    {
        if (percent >= 0.8f) return 5;
        if (percent >= 0.6f) return 4;
        if (percent >= 0.4f) return 3;
        if (percent >= 0.2f) return 2;

        return 1;
    }

    void ShowMessage(CustomerDelivery customer, string message)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine =
            StartCoroutine(MessagePopup(customer, message));
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

    // (WHITE → ORANGE → RED)
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
        else
        {
            float t = (0.5f - percent) / 0.5f;
            return Color.Lerp(orange, red, t);
        }
    }
}