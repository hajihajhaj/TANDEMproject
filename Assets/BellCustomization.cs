using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BellCustomization : MonoBehaviour
{
    [Header("Bell Sounds")]
    public AudioClip[] bellSounds;

    [Header("Bell UI")]
    public GameObject[] bellOptions;

    [Header("Select Button Text")]
    public TMP_Text selectButtonText;

    [Header("Preview Audio")]
    public AudioSource previewAudio;

    private int currentBell = 0;
    private int selectedBell = 0;

    private Gamepad p1;

    void Start()
    {
        selectedBell = PlayerPrefs.GetInt("SelectedBell", 0);
        currentBell = selectedBell;

        ShowBell();
        UpdateSelectButton();
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
        UpdateSelectButton();
    }

    public void PreviousBell()
    {
        currentBell--;

        if (currentBell < 0)
            currentBell = bellOptions.Length - 1;

        ShowBell();
        UpdateSelectButton();
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
        selectedBell = currentBell;

        PlayerPrefs.SetInt("SelectedBell", selectedBell);
        PlayerPrefs.Save();

        UpdateSelectButton();

        Debug.Log("Bell sound saved! Selected bell: " + selectedBell);
    }

    void UpdateSelectButton()
    {
        if (selectButtonText != null)
        {
            if (currentBell == selectedBell)
                selectButtonText.text = "Selected";
            else
                selectButtonText.text = "Select";
        }
    }

    public void TestBell()
    {
        if (previewAudio != null && currentBell < bellSounds.Length)
        {
            previewAudio.PlayOneShot(bellSounds[currentBell]);
        }
    }
}