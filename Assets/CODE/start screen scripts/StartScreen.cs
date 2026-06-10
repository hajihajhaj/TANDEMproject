using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.xKey.wasPressedThisFrame)
        {
            StartGame();
        }

        if (Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            StartGame();
            Debug.Log("Button South Pressed");
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}