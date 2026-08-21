using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour
{
    [Header("Stars")]
    public GameObject[] starImages;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public TMP_Text loadingPercentage;

    // Bike loading screen stuff temporarily disabled for testing.
    // public Image loadingBike;
    // public Sprite[] bikeFrames;

    // public float bikeFrameRate = 0.1f;

    public float minimumLoadingTime = 3f;

    public string sceneName;
    public GameObject promptUI;

    private bool playerInRange = false;
    private bool isLoading = false;

    // Bike animation coroutine temporarily disabled.
    // private Coroutine bikeAnimationCoroutine;

    void Start()
    {
        LoadStars();
    }

    void Update()
    {
        if (isLoading)
            return;

        bool startPressed = false;

        // Keyboard
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            startPressed = true;
        }

        // Both controllers
        foreach (Gamepad pad in Gamepad.all)
        {
            if (pad.buttonSouth.wasPressedThisFrame)
            {
                startPressed = true;
                break;
            }
        }

        if (playerInRange && startPressed)
        {
            StartCoroutine(LoadLevelAsync());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bike"))
        {
            playerInRange = true;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bike"))
        {
            playerInRange = false;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    void LoadStars()
    {
        int savedStars =
            PlayerPrefs.GetInt(sceneName + "_Stars", 0);

        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].SetActive(i < savedStars);
        }
    }

    IEnumerator LoadLevelAsync()
    {
        isLoading = true;

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        if (loadingPercentage != null)
            loadingPercentage.text = "0%";

        // Bike animation temporarily disabled for testing.
        /*
        if (bikeAnimationCoroutine != null)
            StopCoroutine(bikeAnimationCoroutine);

        bikeAnimationCoroutine = StartCoroutine(AnimateLoadingBike());
        */

        // Give Unity a frame to display the loading screen.
        yield return null;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        float displayedProgress = 0f;
        float elapsedTime = 0f;

        while (operation.progress < 0.9f ||
               elapsedTime < minimumLoadingTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // Actual Unity loading progress.
            float realProgress =
                Mathf.Clamp01(operation.progress / 0.9f);

            // Smoothly move the displayed number toward
            // the actual loading progress.
            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                realProgress,
                0.5f * Time.unscaledDeltaTime
            );

            if (loadingPercentage != null)
            {
                int percentage =
                    Mathf.FloorToInt(displayedProgress * 100f);

                loadingPercentage.text = percentage + "%";
            }

            yield return null;
        }

        // Smoothly finish 0-100 if necessary.
        while (displayedProgress < 1f)
        {
            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                1f,
                0.5f * Time.unscaledDeltaTime
            );

            if (loadingPercentage != null)
            {
                int percentage =
                    Mathf.FloorToInt(displayedProgress * 100f);

                loadingPercentage.text = percentage + "%";
            }

            yield return null;
        }

        if (loadingPercentage != null)
            loadingPercentage.text = "100%";

        yield return new WaitForSecondsRealtime(0.2f);

        // Bike animation temporarily disabled.
        /*
        if (bikeAnimationCoroutine != null)
        {
            StopCoroutine(bikeAnimationCoroutine);
            bikeAnimationCoroutine = null;
        }
        */

        // Allow Unity to activate the loaded scene.
        operation.allowSceneActivation = true;
    }

    /*
    IEnumerator AnimateLoadingBike()
    {
        if (loadingBike == null ||
            bikeFrames == null ||
            bikeFrames.Length == 0)
        {
            yield break;
        }

        int currentFrame = 0;

        while (true)
        {
            loadingBike.sprite = bikeFrames[currentFrame];

            currentFrame++;

            if (currentFrame >= bikeFrames.Length)
                currentFrame = 0;

            yield return new WaitForSecondsRealtime(bikeFrameRate);
        }
    }
    */
}