using UnityEngine;
using UnityEngine.InputSystem;

public class BikeBell : MonoBehaviour
{
    public AudioSource bellSound;

    private Gamepad p1;

    void Update()
    {
        // Always lock Player 1 to first connected gamepad
        if (p1 == null && Gamepad.all.Count > 0)
            p1 = Gamepad.all[0];

        if (p1 == null) return;

        // Player 1 R2
        if (p1.rightTrigger.wasPressedThisFrame)
        {
            if (bellSound != null)
                bellSound.Play();
        }
    }
}