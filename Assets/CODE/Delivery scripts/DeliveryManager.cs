using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[System.Serializable]
public class DeliverySummaryUI
{
    public Image customerImage;
    public TMP_Text customerNameText;
    public TMP_Text timeText;

    public GameObject[] starImages;
}

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

    public Animator messageAnimator;

    public TMP_Text customerNameText;
    public TMP_Text customerMessageText;

    [Header("Delivery Reward UI")]
    public TMP_Text rewardCoinsText;

    public GameObject[] rewardStarImages;

    public Image customerImageUI;

    [Header("Summary")]
    public GameObject summaryPanel;
    
    public TMP_Text deliveredText;

    [Header("Summary Buttons")]
    public Button homeButton;
    public Button restartButton;
    public Button firstSelectedButton;

    [Header("Level Summary Extra")]

    public TMP_Text totalTimeTakenText;

    public TMP_Text deliveriesSummaryText;

    public TMP_Text averageStarsText;

    public GameObject[] averageStarImages;

    public DeliverySummaryUI[] customerSummaryUI;

    [Header("Coins")]
    public TMP_Text totalCoinsText;
    public TMP_Text summaryCoinsText;

    public TMP_Text totalStarsText;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip messageSound;
    public AudioClip successSound;
    public AudioClip failSound;

    [Header("5 Star Celebration")]
    public GameObject confettiPrefab;

    public Camera celebrationCamera;

    public Vector3 leftConfettiPos =
        new Vector3(-2f, 1f, 5f);

    public Vector3 rightConfettiPos =
        new Vector3(2f, 1f, 5f);

    public float confettiDestroyTime = 4f;


    int totalStars;
    int totalCoins;

    int deliveredCount;
    int failedCount;

    bool levelEnded;

    Coroutine messageRoutine;

    bool messageAudioLock;

    public static DeliveryManager instance;

    bool successMessageShowing = false;

    CustomerDelivery pendingCustomer;
    string pendingMessage;
    bool pendingPlaySound;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentMainTime = mainLevelTime;

        messagePanel.SetActive(false);
        summaryPanel.SetActive(false);

        UpdateCoinUI();

        UpdateStarsUI();

        foreach (CustomerDelivery customer in customers)
        {
            customer.currentTime = customer.maxTime;
            customer.hurryShown = false;

            if (customer.targetHouse != null)
            {
                customer.targetHouse.deliveryManager = this;
                customer.targetHouse.customer = customer;
            }

            if (customer.persistentImages != null)
            {
                foreach (GameObject image in customer.persistentImages)
                {
                    if (image != null)
                    {
                        image.SetActive(false);
                    }
                }
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

        mainTimerText.text =
            $"{minutes:00}:{seconds:00}";

        float percent =
            currentMainTime / mainLevelTime;

        mainTimerText.color =
            GetTimerColor(percent);

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
                    customer.currentTime /
                    customer.maxTime;

                customer.timerBar.value = percent;

                Image fillImage =
                    customer.timerBar.fillRect
                    .GetComponent<Image>();

                fillImage.color =
                    GetTimerColor(percent);
            }

            // HURRY MESSAGE
            if (!customer.hurryShown &&
                customer.currentTime <=
                customer.maxTime * 0.5f)
            {
                customer.hurryShown = true;

                ShowMessage(
     customer,
     customer.hurryMessage,
     true,
     false
 );
            }

            // FAIL
            if (customer.currentTime <= 0)
            {
                customer.failed = true;

                failedCount++;

                PlaySoundSafe(failSound, 0.7f);

                ShowMessage(
     customer,
     customer.angryMessage,
     false,
     false
 );
            }
        }
    }

    public void CompleteDelivery(CustomerDelivery customer)
    {
        if (customer.delivered || customer.failed)
            return;

        customer.delivered = true;

        deliveredCount++;

        AchievementManager.Instance.AddDelivery();



        float percent =
            customer.currentTime /
            customer.maxTime;

        int stars =
            CalculateStars(percent);

        customer.earnedStars = stars;

        if (stars == 5)
        {
            PlayFiveStarConfetti();
        }

        customer.deliveryTimeTaken =
            customer.maxTime - customer.currentTime;

        totalStars += stars;

        UpdateStarsUI();

        // COINS
        int coinMultiplier =
            (int)customer.difficulty;

        int earnedCoins =
            stars * coinMultiplier;

        totalCoins += earnedCoins;
        UpgradeData.totalCoins = totalCoins;

        UpdateCoinUI();

        PlaySoundSafe(successSound, 0.6f);

        // SHOW PERSISTENT IMAGES
        if (customer.persistentImages != null)
        {
            foreach (GameObject image
                in customer.persistentImages)
            {
                if (image != null)
                {
                    image.SetActive(true);
                }
            }
        }
        ShowMessage(
    customer,
    customer.successMessage,
    false,
    true
);

        rewardCoinsText.text = "+" + earnedCoins;

        StartCoroutine(CheckEndAfterMessage());
    }

    void UpdateCoinUI()
    {
        if (totalCoinsText != null)
        {
            totalCoinsText.text =
      totalCoins.ToString();
        }
    }

    void UpdateStarsUI()
    {
        if (totalStarsText != null)
        {
            totalStarsText.text =
    totalStars.ToString();
        }
    }

    void ShowMessage(
    CustomerDelivery customer,
    string message,
    bool playSound,
    bool showRewards = false
)
    {
        // Don't let hurry/angry messages interrupt a success popup
        if (successMessageShowing && !showRewards)
        {
            pendingCustomer = customer;
            pendingMessage = message;
            pendingPlaySound = playSound;
            return;
        }

        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine =
            StartCoroutine(
                MessagePopup(customer, message, showRewards)
            );

        if (playSound)
            PlayMessageSound();
    }

    IEnumerator MessagePopup(
     CustomerDelivery customer,
     string message,
     bool showRewards
 )
    {
        messagePanel.SetActive(true);
        successMessageShowing = showRewards;

        if (messageAnimator != null)
        {
            messageAnimator.Play("messagesupanddown", 0, 0f);
        }

        rewardCoinsText.gameObject.SetActive(showRewards);

        for (int i = 0; i < rewardStarImages.Length; i++)
        {
            rewardStarImages[i].SetActive(
                showRewards &&
                i < customer.earnedStars
            );
        }

        customerNameText.text =
            customer.customerName;

        customerMessageText.text =
            message;

        customerImageUI.sprite =
            customer.customerImage;

        yield return new WaitForSeconds(5f);

        messagePanel.SetActive(false);
        successMessageShowing = false;

        rewardCoinsText.gameObject.SetActive(false);

        for (int i = 0; i < rewardStarImages.Length; i++)
        {
            rewardStarImages[i]
                .SetActive(false);
        }

        // Show delayed message after success popup finishes
        if (pendingCustomer != null)
        {
            ShowMessage(
                pendingCustomer,
                pendingMessage,
                pendingPlaySound,
                false
            );

            pendingCustomer = null;
            pendingMessage = "";
            pendingPlaySound = false;
        }

    }

    void PlayMessageSound()
    {
        if (messageAudioLock)
            return;

        StartCoroutine(
            MessageSoundCooldown()
        );
    }

    IEnumerator MessageSoundCooldown()
    {
        messageAudioLock = true;

        PlaySoundSafe(messageSound, 1f);

        yield return new WaitForSeconds(0.2f);

        messageAudioLock = false;
    }

    void PlaySoundSafe(
        AudioClip clip,
        float volume
    )
    {
        if (audioSource != null &&
            clip != null)
        {
            audioSource.PlayOneShot(
                clip,
                volume
            );
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

        foreach (CustomerDelivery customer
            in customers)
        {
            if (customer.delivered ||
                customer.failed)
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

        // Select button for controller navigation
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
        }


        // DELIVERIES
        deliveredText.text =
            deliveredCount.ToString();

       

        // TOTAL COINS
        if (summaryCoinsText != null)
        {
            summaryCoinsText.text =
                totalCoins.ToString();
        }

        // TOTAL TIME TAKEN
        float timeTaken =
            mainLevelTime - currentMainTime;

        int minutes =
            Mathf.FloorToInt(timeTaken / 60);

        int seconds =
            Mathf.FloorToInt(timeTaken % 60);

        totalTimeTakenText.text =
            $"{minutes:00}:{seconds:00}";

        // DELIVERIES SUMMARY
        deliveriesSummaryText.text =
            deliveredCount + "/" + customers.Length;

        // AVERAGE STARS OUT OF 3
        float averageStars =
            (float)totalStars / customers.Length;

        // convert 5-star scale to 3-star scale
        float averageOutOf3 =
            (averageStars / 5f) * 3f;

        // round whole number only
        int roundedAverage =
            Mathf.RoundToInt(averageOutOf3);

        string sceneName =
    SceneManager.GetActiveScene().name;

        int currentBest =
            PlayerPrefs.GetInt(sceneName + "_Stars", 0);

        if (roundedAverage > currentBest)
        {
            PlayerPrefs.SetInt(
                sceneName + "_Stars",
                roundedAverage
            );

            PlayerPrefs.Save();
        }

        averageStarsText.text =
            roundedAverage + "/3";

        // SHOW STAR IMAGES
        for (int i = 0; i < averageStarImages.Length; i++)
        {
            averageStarImages[i]
                .SetActive(i < roundedAverage);
        }

        // CUSTOMER DELIVERY SUMMARY
        for (int i = 0; i < customerSummaryUI.Length; i++)
        {
            if (i >= customers.Length)
                continue;

            CustomerDelivery customer =
                customers[i];

            DeliverySummaryUI ui =
                customerSummaryUI[i];

            // IMAGE
            ui.customerImage.sprite =
                customer.customerImage;

            // NAME
            ui.customerNameText.text =
                customer.customerName;

            // TIME
            int mins =
                Mathf.FloorToInt(
                    customer.deliveryTimeTaken / 60);

            int secs =
                Mathf.FloorToInt(
                    customer.deliveryTimeTaken % 60);

            ui.timeText.text =
                $"{mins:00}:{secs:00}";

            // STARS
            for (int s = 0; s < ui.starImages.Length; s++)
            {
                ui.starImages[s]
                    .SetActive(
                        s < customer.earnedStars
                    );
            }
        }
    }

    public void RestartLevel()
    {
        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }

    public float GetRemainingTimePercent()
    {
        return currentMainTime /
               mainLevelTime;
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

        Color orange =
            new Color(1f, 0.5f, 0f);

        Color red = Color.red;

        if (percent > 0.5f)
        {
            float t =
                (1f - percent) / 0.5f;

            return Color.Lerp(
                white,
                orange,
                t
            );
        }

        float t2 =
            (0.5f - percent) / 0.5f;

        return Color.Lerp(
            orange,
            red,
            t2
        );
    }

    void PlayFiveStarConfetti()
    {
        Debug.Log("CONFETTI FUNCTION CALLED");

        if (confettiPrefab == null)
        {
            Debug.Log("NO CONFETTI PREFAB");
            return;
        }

        if (celebrationCamera == null)
        {
            Debug.Log("NO CAMERA");
            return;
        }

        // LEFT
        GameObject left =
            Instantiate(
                confettiPrefab,
                celebrationCamera.transform
            );

        left.transform.localPosition =
            leftConfettiPos;

        foreach (ParticleSystem ps in left.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }

        // RIGHT
        GameObject right =
            Instantiate(
                confettiPrefab,
                celebrationCamera.transform
            );

        right.transform.localPosition =
            rightConfettiPos;

        foreach (ParticleSystem ps in right.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }

        Destroy(left, 5f);
        Destroy(right, 5f);
    }

}