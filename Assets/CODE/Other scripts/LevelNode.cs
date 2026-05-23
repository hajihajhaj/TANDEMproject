using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour
{
    [Header("Stars")]
    public GameObject[] starImages;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public Slider loadingBar;


    public string sceneName;
    public GameObject promptUI;

    private bool playerInRange = false;

    void Start()
    {
        LoadStars();
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
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
        loadingScreen.SetActive(true);

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress =
                Mathf.Clamp01(
                    operation.progress / 0.9f
                );

            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }

            yield return null;
        }
    }
}