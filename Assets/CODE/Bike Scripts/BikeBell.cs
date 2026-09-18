using UnityEngine;
using UnityEngine.InputSystem;

public class BikeBell : MonoBehaviour
{
    public AudioSource bellSound;

    [Header("Bell Sounds")]
    public AudioClip[] bellSounds;

    private Gamepad p1;

    void Start()
    {
        int selectedBell = PlayerPrefs.GetInt("SelectedBell", 0);

        Debug.Log("Loaded bell: " + selectedBell);

        if (bellSound != null && bellSounds != null && selectedBell >= 0 && selectedBell < bellSounds.Length)
        {
            bellSound.clip = bellSounds[selectedBell];

            Debug.Log("Bell clip changed to: " + bellSounds[selectedBell].name);
        }
        else
        {
            Debug.LogWarning("Could not load selected bell!");
        }
    }

    void Update()
    {
        // Player 1 controller
        if (p1 == null && Gamepad.all.Count > 0)
            p1 = Gamepad.all[0];

        // R2
        if (p1 != null && p1.rightTrigger.wasPressedThisFrame)
        {
            if (bellSound != null)
                bellSound.Play();
        }

        // Keyboard B
        if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
        {
            if (bellSound != null)
                bellSound.Play();
        }
    }
}