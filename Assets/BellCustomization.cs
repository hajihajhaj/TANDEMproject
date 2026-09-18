using UnityEngine;
using UnityEngine.InputSystem;

public class BellCustomization : MonoBehaviour
{
    [Header("Bell Sounds")]
    public AudioClip[] bellSounds;

    [Header("Bell UI")]
    public GameObject[] bellOptions;

    [Header("Preview Audio")]
    public AudioSource previewAudio;

    private int currentBell = 0;
    private Gamepad p1;

    void Start()
    {
        currentBell = PlayerPrefs.GetInt("SelectedBell", 0);
        ShowBell();
    }

    void Update()
    {
        // Get Player 1 controller
        if (p1 == null && Gamepad.all.Count > 0)
            p1 = Gamepad.all[0];

        // R2 = Hear Sound
        if (p1 != null && p1.rightTrigger.wasPressedThisFrame)
        {
            TestBell();
        }

        // B = Hear Sound
        if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
        {
            TestBell();
        }
    }

    public void NextBell()
    {
        currentBell++;

        if (currentBell >= bellOptions.Length)
            currentBell = 0;

        ShowBell();
    }

    public void PreviousBell()
    {
        currentBell--;

        if (currentBell < 0)
            currentBell = bellOptions.Length - 1;

        ShowBell();
    }

    void ShowBell()
    {
        for (int i = 0; i < bellOptions.Length; i++)
        {
            bellOptions[i].SetActive(i == currentBell);
        }
    }

    public void SelectBell()
    {
        PlayerPrefs.SetInt("SelectedBell", currentBell);
        PlayerPrefs.Save();

        Debug.Log("Bell sound saved!");
    }

    public void TestBell()
    {
        if (previewAudio != null && currentBell < bellSounds.Length)
        {
            previewAudio.PlayOneShot(bellSounds[currentBell]);
        }
    }
}