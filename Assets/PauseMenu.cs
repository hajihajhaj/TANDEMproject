using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pausePanel;

    [Header("Help")]
    public GameObject helpPanel;

    [Header("Pause Sounds")]
    public AudioSource audioSource;
    public AudioClip pauseOpenSound;
    public AudioClip pauseCloseSound;

    [Header("Home Scene")]
    public string homeSceneName = "SampleScene";

    [Header("Music")]
    public MusicControl musicControl;

    [Header("Controller Navigation")]
    public GameObject firstPauseButton;
    public GameObject firstHelpButton;

    private bool isPaused = false;

    // Stores the AudioSources that were playing when the game was paused
    private AudioSource[] pausedAudioSources;


    void Start()
    {
        pausePanel.SetActive(false);
        helpPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }


    void Update()
    {
        // Keyboard - ESC
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }


        // PS5 Controller - OPTIONS button
        if (Gamepad.current != null &&
            Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePause();
        }
    }


    // =========================
    // TOGGLE PAUSE
    // =========================

    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }


    // =========================
    // PAUSE
    // =========================

    public void Pause()
    {
        isPaused = true;

        pausePanel.SetActive(true);
        helpPanel.SetActive(false);


        // Tell MusicControl that the game is paused
        // BEFORE pausing the AudioSources
        if (musicControl != null)
        {
            musicControl.SetPaused(true);
        }


        Time.timeScale = 0f;


        // Find all AudioSources currently in the scene
        pausedAudioSources = FindObjectsByType<AudioSource>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );


        // Pause every AudioSource that is currently playing
        foreach (AudioSource source in pausedAudioSources)
        {
            // Don't pause the PauseManager's own AudioSource
            if (source != audioSource && source.isPlaying)
            {
                source.Pause();
            }
        }


        // Play pause opening sound
        if (audioSource != null &&
            pauseOpenSound != null)
        {
            audioSource.PlayOneShot(pauseOpenSound);
        }


        // Select the first pause button
        if (EventSystem.current != null &&
            firstPauseButton != null)
        {
            EventSystem.current.SetSelectedGameObject(
                firstPauseButton
            );
        }
    }


    // =========================
    // RESUME
    // =========================

    public void Resume()
    {
        isPaused = false;

        pausePanel.SetActive(false);
        helpPanel.SetActive(false);


        Time.timeScale = 1f;


        // Tell MusicControl that the game is no longer paused
        // BEFORE resuming the AudioSources
        if (musicControl != null)
        {
            musicControl.SetPaused(false);
        }


        // Resume only the AudioSources that were playing
        // when we paused
        if (pausedAudioSources != null)
        {
            foreach (AudioSource source in pausedAudioSources)
            {
                if (source != null)
                {
                    source.UnPause();
                }
            }
        }


        // Play pause closing sound
        if (audioSource != null &&
            pauseCloseSound != null)
        {
            audioSource.PlayOneShot(pauseCloseSound);
        }


        // Clear controller selection
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }


    // =========================
    // HELP
    // =========================

    public void OpenHelp()
    {
        pausePanel.SetActive(false);
        helpPanel.SetActive(true);


        // Select the first Help button
        if (EventSystem.current != null &&
            firstHelpButton != null)
        {
            EventSystem.current.SetSelectedGameObject(
                firstHelpButton
            );
        }
    }


    public void CloseHelp()
    {
        helpPanel.SetActive(false);
        pausePanel.SetActive(true);


        // Select the first Pause button
        if (EventSystem.current != null &&
            firstPauseButton != null)
        {
            EventSystem.current.SetSelectedGameObject(
                firstPauseButton
            );
        }
    }


    // =========================
    // HOME
    // =========================

    public void GoHome()
    {
        // Make sure the game isn't paused
        Time.timeScale = 1f;


        // Tell MusicControl the game is no longer paused
        if (musicControl != null)
        {
            musicControl.SetPaused(false);
        }


        SceneManager.LoadScene(homeSceneName);
    }
}