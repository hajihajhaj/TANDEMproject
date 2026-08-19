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
    public UnityEngine.UI.Image loadingBike;
    public Sprite[] bikeFrames;

    public float bikeFrameRate = 0.1f;

    public string sceneName;
    public GameObject promptUI;

    private bool playerInRange = false;
    private bool isLoading = false;

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

        // Give the loading screen one frame to appear.
        yield return null;

        // Start loading the next scene asynchronously.
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        // Keep the current scene active until we're ready.
        operation.allowSceneActivation = false;

        // Animate the bike independently.
        StartCoroutine(AnimateLoadingBike());

        while (!operation.isDone)
        {
            // Unity reports loading progress from 0 to 0.9.
            // Dividing by 0.9 converts it to 0 to 1.
            float progress =
                Mathf.Clamp01(operation.progress / 0.9f);

            // Convert progress to a percentage.
            if (loadingPercentage != null)
            {
                int percentage =
                    Mathf.FloorToInt(progress * 100f);

                loadingPercentage.text =
                    percentage + "%";
            }

            // When Unity reaches 0.9, the scene is ready
            // to activate.
            if (operation.progress >= 0.9f)
            {
                if (loadingPercentage != null)
                    loadingPercentage.text = "100%";

                yield return null;

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

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
}