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

    bool achievementFinished = false;

    Coroutine messageRoutine;

    bool messageAudioLock;

    public static DeliveryManager instance;

    bool successMessageShowing = false;

    CustomerDelivery pendingCustomer;
    string pendingMessage;
    bool pendingPlaySound;


    // =========================================================
    // ACHIEVEMENT FINISHED
    // =========================================================

    public void ShowSummaryAfterAchievement()
    {
        achievementFinished = true;

        levelEnded = false;

        Debug.Log(
            "Achievement finished - showing summary."
        );

        EndLevel();
    }


    // =========================================================
    // AWAKE
    // =========================================================

    void Awake()
    {
        instance = this;
    }


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.ResetDeliveries();
        }

        currentMainTime = mainLevelTime;

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }

        if (summaryPanel != null)
        {
            summaryPanel.SetActive(false);
        }

        UpdateCoinUI();

        UpdateStarsUI();

        foreach (CustomerDelivery customer in customers)
        {
            if (customer == null)
                continue;

            customer.currentTime =
                customer.maxTime;

            customer.hurryShown = false;

            if (customer.targetHouse != null)
            {
                customer.targetHouse.deliveryManager =
                    this;

                customer.targetHouse.customer =
                    customer;
            }

            if (customer.persistentImages != null)
            {
                foreach (
                    GameObject image
                    in customer.persistentImages
                )
                {
                    if (image != null)
                    {
                        image.SetActive(false);
                    }
                }
            }
        }
    }


    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (levelEnded)
            return;

        UpdateMainTimer();

        UpdateCustomerTimers();
    }


    // =========================================================
    // MAIN TIMER
    // =========================================================

    void UpdateMainTimer()
    {
        currentMainTime -= Time.deltaTime;

        if (currentMainTime < 0)
        {
            currentMainTime = 0;
        }

        int minutes =
            Mathf.FloorToInt(
                currentMainTime / 60
            );

        int seconds =
            Mathf.FloorToInt(
                currentMainTime % 60
            );

        if (mainTimerText != null)
        {
            mainTimerText.text =
                $"{minutes:00}:{seconds:00}";

            float percent =
                currentMainTime /
                mainLevelTime;

            mainTimerText.color =
                GetTimerColor(percent);
        }

        if (currentMainTime <= 0)
        {
            EndLevel();
        }
    }


    // =========================================================
    // CUSTOMER TIMERS
    // =========================================================

    void UpdateCustomerTimers()
    {
        foreach (CustomerDelivery customer in customers)
        {
            if (customer == null)
                continue;

            if (customer.delivered ||
                customer.failed)
            {
                continue;
            }

            customer.currentTime -=
                Time.deltaTime;

            if (customer.timerBar != null)
            {
                float percent =
                    customer.currentTime /
                    customer.maxTime;

                customer.timerBar.value =
                    percent;

                Image fillImage =
                    customer.timerBar.fillRect
                    .GetComponent<Image>();

                if (fillImage != null)
                {
                    fillImage.color =
                        GetTimerColor(percent);
                }
            }

            // HURRY MESSAGE
            if (
                !customer.hurryShown &&
                customer.currentTime <=
                customer.maxTime * 0.5f
            )
            {
                customer.hurryShown = true;

                ShowMessage(
                    customer,
                    customer.hurryMessage,
                    true,
                    false
                );
            }

            // FAILED
            if (customer.currentTime <= 0)
            {
                customer.failed = true;

                failedCount++;

                PlaySoundSafe(
                    failSound,
                    0.7f
                );

                ShowMessage(
                    customer,
                    customer.angryMessage,
                    false,
                    false
                );
            }
        }
    }


    // =========================================================
    // COMPLETE DELIVERY
    // =========================================================

    public void CompleteDelivery(
        CustomerDelivery customer
    )
    {
        if (customer == null)
            return;

        if (
            customer.delivered ||
            customer.failed
        )
        {
            return;
        }

        customer.delivered = true;

        deliveredCount++;

        Debug.Log(
            "Delivery completed. Total delivered: " +
            deliveredCount +
            "/" +
            customers.Length
        );

        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.AddDelivery();
        }

        float percent =
            customer.currentTime /
            customer.maxTime;

        int stars =
            CalculateStars(percent);

        customer.earnedStars =
            stars;

        if (stars == 5)
        {
            PlayFiveStarConfetti();
        }

        customer.deliveryTimeTaken =
            customer.maxTime -
            customer.currentTime;

        totalStars += stars;

        UpdateStarsUI();

        // COINS
        int coinMultiplier =
            (int)customer.difficulty;

        int earnedCoins =
            stars * coinMultiplier;

        totalCoins += earnedCoins;

        UpgradeData.totalCoins =
            totalCoins;

        UpdateCoinUI();

        // SUCCESS SOUND
        PlaySoundSafe(
            successSound,
            0.6f
        );

        // PERSISTENT CUSTOMER IMAGES
        if (customer.persistentImages != null)
        {
            foreach (
                GameObject image
                in customer.persistentImages
            )
            {
                if (image != null)
                {
                    image.SetActive(true);
                }
            }
        }

        // SUCCESS MESSAGE
        ShowMessage(
            customer,
            customer.successMessage,
            false,
            true
        );

        if (rewardCoinsText != null)
        {
            rewardCoinsText.text =
                "+" + earnedCoins;
        }

        // =====================================================
        // CHECK IF THIS WAS THE LAST HOUSE
        // =====================================================

        if (AllCustomersDelivered())
        {
            Debug.Log(
                "LAST DELIVERY COMPLETED!"
            );

            StartCoroutine(
                ShowAchievementAfterMessage()
            );
        }
        else
        {
            StartCoroutine(
                CheckEndAfterMessage()
            );
        }
    }


    // =========================================================
    // CHECK IF ALL CUSTOMERS ARE DELIVERED
    // =========================================================

    bool AllCustomersDelivered()
    {
        foreach (CustomerDelivery customer in customers)
        {
            if (customer == null)
                continue;

            if (!customer.delivered)
            {
                return false;
            }
        }

        return true;
    }


    // =========================================================
    // COIN UI
    // =========================================================

    void UpdateCoinUI()
    {
        if (totalCoinsText != null)
        {
            totalCoinsText.text =
                totalCoins.ToString();
        }
    }


    // =========================================================
    // STAR UI
    // =========================================================

    void UpdateStarsUI()
    {
        if (totalStarsText != null)
        {
            totalStarsText.text =
                totalStars.ToString();
        }
    }


    // =========================================================
    // SHOW MESSAGE
    // =========================================================

    void ShowMessage(
        CustomerDelivery customer,
        string message,
        bool playSound,
        bool showRewards = false
    )
    {
        if (
            successMessageShowing &&
            !showRewards
        )
        {
            pendingCustomer =
                customer;

            pendingMessage =
                message;

            pendingPlaySound =
                playSound;

            return;
        }

        if (messageRoutine != null)
        {
            StopCoroutine(
                messageRoutine
            );
        }

        messageRoutine =
            StartCoroutine(
                MessagePopup(
                    customer,
                    message,
                    showRewards
                )
            );

        if (playSound)
        {
            PlayMessageSound();
        }
    }


    // =========================================================
    // MESSAGE POPUP
    // =========================================================

    IEnumerator MessagePopup(
        CustomerDelivery customer,
        string message,
        bool showRewards
    )
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }

        successMessageShowing =
            showRewards;

        if (messageAnimator != null)
        {
            messageAnimator.Play(
                "messagesupanddown",
                0,
                0f
            );
        }

        if (rewardCoinsText != null)
        {
            rewardCoinsText.gameObject
                .SetActive(showRewards);
        }

        if (rewardStarImages != null)
        {
            for (
                int i = 0;
                i < rewardStarImages.Length;
                i++
            )
            {
                if (rewardStarImages[i] != null)
                {
                    rewardStarImages[i].SetActive(
                        showRewards &&
                        i < customer.earnedStars
                    );
                }
            }
        }

        if (customerNameText != null)
        {
            customerNameText.text =
                customer.customerName;
        }

        if (customerMessageText != null)
        {
            customerMessageText.text =
                message;
        }

        if (customerImageUI != null)
        {
            customerImageUI.sprite =
                customer.customerImage;
        }

        // Keep success message on screen
        // for 5 seconds.
        yield return new WaitForSeconds(5f);

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }

        successMessageShowing = false;

        if (rewardCoinsText != null)
        {
            rewardCoinsText.gameObject
                .SetActive(false);
        }

        if (rewardStarImages != null)
        {
            for (
                int i = 0;
                i < rewardStarImages.Length;
                i++
            )
            {
                if (rewardStarImages[i] != null)
                {
                    rewardStarImages[i]
                        .SetActive(false);
                }
            }
        }

        // Show any queued message
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


    // =========================================================
    // MESSAGE SOUND
    // =========================================================

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

        if (
            NotificationSoundManager
                .CurrentNotificationSound != null
        )
        {
            PlaySoundSafe(
                NotificationSoundManager
                    .CurrentNotificationSound,
                1f
            );
        }

        yield return new WaitForSeconds(0.2f);

        messageAudioLock = false;
    }


    // =========================================================
    // SAFE SOUND
    // =========================================================

    void PlaySoundSafe(
        AudioClip clip,
        float volume
    )
    {
        if (
            audioSource != null &&
            clip != null
        )
        {
            audioSource.PlayOneShot(
                clip,
                volume
            );
        }
    }


    // =========================================================
    // CHECK END AFTER NORMAL DELIVERY
    // =========================================================

    IEnumerator CheckEndAfterMessage()
    {
        yield return new WaitForSeconds(5f);

        CheckLevelEnd();
    }


    // =========================================================
    // SHOW ACHIEVEMENT AFTER FINAL MESSAGE
    // =========================================================

    IEnumerator ShowAchievementAfterMessage()
    {
        // Wait until the final delivery message
        // has finished.
        yield return new WaitForSeconds(5f);

        if (currentMainTime <= 0)
        {
            EndLevel();

            yield break;
        }

        if (achievementFinished)
        {
            yield break;
        }

        achievementFinished = true;

        // Pause the level while the achievement
        // popup is showing.
        levelEnded = true;

        Debug.Log(
            "SHOWING ACHIEVEMENT FOR FINAL DELIVERY"
        );

        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance
                .UnlockAchievement();
        }

        if (AchievementPopupUI.Instance != null)
        {
            AchievementPopupUI.Instance.Show();
        }
        else
        {
            Debug.LogWarning(
                "AchievementPopupUI.Instance is NULL!"
            );

            ShowSummaryAfterAchievement();
        }
    }


    // =========================================================
    // CHECK LEVEL END
    // =========================================================

    void CheckLevelEnd()
    {
        int finished = 0;

        foreach (
            CustomerDelivery customer
            in customers
        )
        {
            if (customer == null)
                continue;

            if (
                customer.delivered ||
                customer.failed
            )
            {
                finished++;
            }
        }

        int actualCustomers = 0;

        foreach (
            CustomerDelivery customer
            in customers
        )
        {
            if (customer != null)
            {
                actualCustomers++;
            }
        }

        if (finished >= actualCustomers)
        {
            EndLevel();
        }
    }


    // =========================================================
    // END LEVEL
    // =========================================================

    void EndLevel()
    {
        if (levelEnded)
            return;

        levelEnded = true;

        bool completedAllDeliveries =
            AllCustomersDelivered() &&
            currentMainTime > 0;

        // Backup achievement check.
        // This makes sure the achievement still appears
        // if another end-level check happens first.
        if (
            !achievementFinished &&
            completedAllDeliveries &&
            AchievementManager.Instance != null &&
            AchievementPopupUI.Instance != null
        )
        {
            achievementFinished = true;

            AchievementManager.Instance
                .UnlockAchievement();

            AchievementPopupUI.Instance.Show();

            return;
        }

        ShowSummary();
    }


    // =========================================================
    // SHOW SUMMARY
    // =========================================================

    void ShowSummary()
    {
        if (summaryPanel != null)
        {
            summaryPanel.SetActive(true);
        }

        if (firstSelectedButton != null)
        {
            EventSystem.current
                .SetSelectedGameObject(null);

            EventSystem.current
                .SetSelectedGameObject(
                    firstSelectedButton.gameObject
                );
        }

        if (deliveredText != null)
        {
            deliveredText.text =
                deliveredCount.ToString();
        }

        if (summaryCoinsText != null)
        {
            summaryCoinsText.text =
                totalCoins.ToString();
        }

        float timeTaken =
            mainLevelTime -
            currentMainTime;

        int minutes =
            Mathf.FloorToInt(
                timeTaken / 60
            );

        int seconds =
            Mathf.FloorToInt(
                timeTaken % 60
            );

        if (totalTimeTakenText != null)
        {
            totalTimeTakenText.text =
                $"{minutes:00}:{seconds:00}";
        }

        if (deliveriesSummaryText != null)
        {
            int actualCustomers = 0;

            foreach (
                CustomerDelivery customer
                in customers
            )
            {
                if (customer != null)
                {
                    actualCustomers++;
                }
            }

            deliveriesSummaryText.text =
                deliveredCount +
                "/" +
                actualCustomers;
        }

        float averageStars = 0f;

        int actualCustomerCount = 0;

        foreach (
            CustomerDelivery customer
            in customers
        )
        {
            if (customer == null)
                continue;

            actualCustomerCount++;

            averageStars +=
                customer.earnedStars;
        }

        if (actualCustomerCount > 0)
        {
            averageStars /=
                actualCustomerCount;
        }

        float averageOutOf3 =
            (averageStars / 5f) * 3f;

        int roundedAverage =
            Mathf.RoundToInt(
                averageOutOf3
            );

        string sceneName =
            SceneManager
                .GetActiveScene()
                .name;

        int currentBest =
            PlayerPrefs.GetInt(
                sceneName + "_Stars",
                0
            );

        if (roundedAverage > currentBest)
        {
            PlayerPrefs.SetInt(
                sceneName + "_Stars",
                roundedAverage
            );

            PlayerPrefs.Save();
        }

        if (averageStarsText != null)
        {
            averageStarsText.text =
                roundedAverage + "/3";
        }

        if (averageStarImages != null)
        {
            for (
                int i = 0;
                i < averageStarImages.Length;
                i++
            )
            {
                if (averageStarImages[i] != null)
                {
                    averageStarImages[i]
                        .SetActive(
                            i < roundedAverage
                        );
                }
            }
        }

        // CUSTOMER SUMMARY
        if (customerSummaryUI != null)
        {
            for (
                int i = 0;
                i < customerSummaryUI.Length;
                i++
            )
            {
                if (i >= customers.Length)
                    continue;

                if (customers[i] == null)
                    continue;

                CustomerDelivery customer =
                    customers[i];

                DeliverySummaryUI ui =
                    customerSummaryUI[i];

                if (ui.customerImage != null)
                {
                    ui.customerImage.sprite =
                        customer.customerImage;
                }

                if (ui.customerNameText != null)
                {
                    ui.customerNameText.text =
                        customer.customerName;
                }

                int mins =
                    Mathf.FloorToInt(
                        customer.deliveryTimeTaken /
                        60
                    );

                int secs =
                    Mathf.FloorToInt(
                        customer.deliveryTimeTaken %
                        60
                    );

                if (ui.timeText != null)
                {
                    ui.timeText.text =
                        $"{mins:00}:{secs:00}";
                }

                if (ui.starImages != null)
                {
                    for (
                        int s = 0;
                        s < ui.starImages.Length;
                        s++
                    )
                    {
                        if (ui.starImages[s] != null)
                        {
                            ui.starImages[s]
                                .SetActive(
                                    s <
                                    customer.earnedStars
                                );
                        }
                    }
                }
            }
        }
    }


    // =========================================================
    // RESTART
    // =========================================================

    public void RestartLevel()
    {
        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }


    // =========================================================
    // REMAINING TIME PERCENT
    // =========================================================

    public float GetRemainingTimePercent()
    {
        return currentMainTime /
               mainLevelTime;
    }


    // =========================================================
    // CALCULATE STARS
    // =========================================================

    int CalculateStars(float percent)
    {
        if (percent >= 0.8f)
            return 5;

        if (percent >= 0.6f)
            return 4;

        if (percent >= 0.4f)
            return 3;

        if (percent >= 0.2f)
            return 2;

        return 1;
    }


    // =========================================================
    // TIMER COLOR
    // =========================================================

    Color GetTimerColor(float percent)
    {
        Color white =
            Color.white;

        Color orange =
            new Color(
                1f,
                0.5f,
                0f
            );

        Color red =
            Color.red;

        if (percent > 0.5f)
        {
            float t =
                (1f - percent) /
                0.5f;

            return Color.Lerp(
                white,
                orange,
                t
            );
        }

        float t2 =
            (0.5f - percent) /
            0.5f;

        return Color.Lerp(
            orange,
            red,
            t2
        );
    }


    // =========================================================
    // FIVE STAR CONFETTI
    // =========================================================

    void PlayFiveStarConfetti()
    {
        Debug.Log(
            "CONFETTI FUNCTION CALLED"
        );

        if (confettiPrefab == null)
        {
            Debug.Log(
                "NO CONFETTI PREFAB"
            );

            return;
        }

        if (celebrationCamera == null)
        {
            Debug.Log(
                "NO CAMERA"
            );

            return;
        }

        GameObject left =
            Instantiate(
                confettiPrefab,
                celebrationCamera.transform
            );

        left.transform.localPosition =
            leftConfettiPos;

        foreach (
            ParticleSystem ps
            in left.GetComponentsInChildren<
                ParticleSystem
            >()
        )
        {
            ps.Play();
        }

        GameObject right =
            Instantiate(
                confettiPrefab,
                celebrationCamera.transform
            );

        right.transform.localPosition =
            rightConfettiPos;

        foreach (
            ParticleSystem ps
            in right.GetComponentsInChildren<
                ParticleSystem
            >()
        )
        {
            ps.Play();
        }

        Destroy(
            left,
            5f
        );

        Destroy(
            right,
            5f
        );
    }
}
