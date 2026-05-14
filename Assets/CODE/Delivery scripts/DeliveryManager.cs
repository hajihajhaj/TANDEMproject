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

    [Header("Messages")]
    public TMP_Text customerNameText;
    public TMP_Text customerMessageText;

    [Header("Summary")]
    public GameObject summaryPanel;
    public TMP_Text starsText;
    public TMP_Text deliveredText;
    public TMP_Text failedText;

    int totalStars;
    int deliveredCount;
    int failedCount;

    bool levelEnded;

    public static DeliveryManager instance;

    Coroutine messageRoutine;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentMainTime = mainLevelTime;

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
        if (levelEnded) return;

        UpdateMainTimer();
        UpdateCustomerTimers();
        CheckLevelEnd();
    }

    void UpdateMainTimer()
    {
        currentMainTime -= Time.deltaTime;
        if (currentMainTime < 0) currentMainTime = 0;

        int minutes = Mathf.FloorToInt(currentMainTime / 60);
        int seconds = Mathf.FloorToInt(currentMainTime % 60);

        mainTimerText.text = $"{minutes:00}:{seconds:00}";

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
                customer.timerBar.value = customer.currentTime / customer.maxTime;

            // HURRY MESSAGE (only once per update range, simple version)
            if (customer.currentTime < customer.maxTime * 0.5f &&
                customer.currentTime > customer.maxTime * 0.49f)
            {
                ShowMessage(customer.customerName, customer.hurryMessage);
            }

            // FAIL
            if (customer.currentTime <= 0)
            {
                customer.failed = true;
                failedCount++;

                ShowMessage(customer.customerName, customer.angryMessage);
            }
        }
    }

    public void CompleteDelivery(CustomerDelivery customer)
    {
        if (customer.delivered || customer.failed)
            return;

        customer.delivered = true;
        deliveredCount++;

        float percent = customer.currentTime / customer.maxTime;
        int stars = CalculateStars(percent);
        totalStars += stars;

        ShowMessage(
            customer.customerName,
            customer.successMessage + "\nStars: " + stars
        );
    }

    int CalculateStars(float percent)
    {
        if (percent >= 0.8f) return 5;
        if (percent >= 0.6f) return 4;
        if (percent >= 0.4f) return 3;
        if (percent >= 0.2f) return 2;
        return 1;
    }

    void ShowMessage(string customerName, string message)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(MessagePopup(customerName, message));
    }

    void CheckLevelEnd()
    {
        int finished = 0;

        foreach (CustomerDelivery c in customers)
        {
            if (c.delivered || c.failed)
                finished++;
        }

        if (finished >= customers.Length)
            EndLevel();
    }

    IEnumerator MessagePopup(string customerName, string message)
    {
        customerNameText.text = customerName;
        customerMessageText.text = message;

        yield return new WaitForSeconds(2f);

        customerNameText.text = "";
        customerMessageText.text = "";
    }

    void EndLevel()
    {
        if (levelEnded) return;

        levelEnded = true;
        summaryPanel.SetActive(true);

        starsText.text = "Stars: " + totalStars;
        deliveredText.text = "Delivered: " + deliveredCount;
        failedText.text = "Failed: " + failedCount;
    }
}