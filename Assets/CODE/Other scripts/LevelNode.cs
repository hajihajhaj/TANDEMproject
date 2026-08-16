using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class LevelNode : MonoBehaviour
{
    [Header("Stars")]
    public GameObject[] starImages;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public TMP_Text loadingPercentage;

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

        // Give Unity one frame to display the loading screen.
        yield return null;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        float displayedProgress = 0f;
        float previousRealProgress = 0f;

        while (!operation.isDone)
        {
            // Unity's real loading progress.
            float realProgress =
                Mathf.Clamp01(operation.progress / 0.9f);

            // How much the real loading progress changed.
            float progressChange =
                realProgress - previousRealProgress;

            previousRealProgress = realProgress;

            // If loading is actively progressing, move the
            // displayed percentage toward it.
            if (realProgress > displayedProgress)
            {
                // Determine speed based on how quickly the
                // actual scene is loading.
                float speed = Mathf.Max(
                    progressChange / Time.deltaTime,
                    0.05f
                );

                displayedProgress = Mathf.MoveTowards(
                    displayedProgress,
                    realProgress,
                    speed * Time.deltaTime
                );
            }

            // Update the percentage.
            if (loadingPercentage != null)
            {
                int percentage =
                    Mathf.FloorToInt(displayedProgress * 100f);

                loadingPercentage.text =
                    percentage + "%";
            }

            yield return null;
        }

        // Scene is ready — smoothly finish the last bit.
        while (displayedProgress < 1f)
        {
            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                1f,
                2f * Time.deltaTime
            );

            if (loadingPercentage != null)
            {
                int percentage =
                    Mathf.FloorToInt(displayedProgress * 100f);

                loadingPercentage.text =
                    percentage + "%";
            }

            yield return null;
        }

        if (loadingPercentage != null)
            loadingPercentage.text = "100%";
    }
}