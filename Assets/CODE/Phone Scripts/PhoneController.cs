using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PhoneController : MonoBehaviour
{
    [Header("Phone")]
    public GameObject phoneUI;

    [Header("Apps")]
    public Button[] appButtons;

    [Header("Opened App Panels")]
    public GameObject[] appPanels;

    int currentIndex = 0;

    bool phoneOpen = false;
    bool appOpen = false;

    bool dpadInUse = false;

    void Start()
    {
        phoneUI.SetActive(false);

        foreach (GameObject panel in appPanels)
        {
            panel.SetActive(false);
        }
    }

    void Update()
    {
        HandlePhoneToggle();

        if (!phoneOpen) return;

        if (!appOpen)
        {
            HandleNavigation();
            HandleAppOpen();
        }
        else
        {
            HandleBack();
        }
    }

    // -------------------------
    // OPEN / CLOSE PHONE
    // -------------------------
    void HandlePhoneToggle()
    {
        bool p2DpadUp = false;
        bool p2Circle = false;

        if (Gamepad.all.Count > 1)
        {
            p2DpadUp = Gamepad.all[1].dpad.up.wasPressedThisFrame;
            p2Circle = Gamepad.all[1].buttonEast.wasPressedThisFrame;
        }

        // TAB or DPad Up
        if (Input.GetKeyDown(KeyCode.Tab) || p2DpadUp)
        {
            phoneOpen = true;
            phoneUI.SetActive(true);

            SelectButton(currentIndex);
        }

        // BACKSPACE or O
        if ((Input.GetKeyDown(KeyCode.Backspace) || p2Circle) && !appOpen)
        {
            phoneOpen = false;
            phoneUI.SetActive(false);
        }
    }

    // -------------------------
    // NAVIGATION
    // -------------------------
    void HandleNavigation()
    {
        bool left = Input.GetKeyDown(KeyCode.LeftArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);
        bool up = Input.GetKeyDown(KeyCode.UpArrow);
        bool down = Input.GetKeyDown(KeyCode.DownArrow);

        if (Gamepad.all.Count > 1)
        {
            Vector2 dpad = Gamepad.all[1].dpad.ReadValue();

            // reset dpad lock
            if (Mathf.Abs(dpad.x) < 0.5f && Mathf.Abs(dpad.y) < 0.5f)
            {
                dpadInUse = false;
            }

            if (!dpadInUse)
            {
                if (dpad.x < -0.5f)
                {
                    left = true;
                    dpadInUse = true;
                }

                if (dpad.x > 0.5f)
                {
                    right = true;
                    dpadInUse = true;
                }

                if (dpad.y > 0.5f)
                {
                    up = true;
                    dpadInUse = true;
                }

                if (dpad.y < -0.5f)
                {
                    down = true;
                    dpadInUse = true;
                }
            }
        }

        // Layout:
        //
        // 0 1
        // 2 3

        // LEFT
        if (left)
        {
            if (currentIndex % 2 == 1)
            {
                currentIndex--;
                SelectButton(currentIndex);
            }
        }

        // RIGHT
        if (right)
        {
            if (currentIndex % 2 == 0)
            {
                currentIndex++;
                SelectButton(currentIndex);
            }
        }

        // UP
        if (up)
        {
            if (currentIndex >= 2)
            {
                currentIndex -= 2;
                SelectButton(currentIndex);
            }
        }

        // DOWN
        if (down)
        {
            if (currentIndex <= 1)
            {
                currentIndex += 2;
                SelectButton(currentIndex);
            }
        }
    }

    void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(appButtons[index].gameObject);
    }

    // -------------------------
    // OPEN APP
    // -------------------------
    void HandleAppOpen()
    {
        bool p2X = false;

        if (Gamepad.all.Count > 1)
        {
            p2X = Gamepad.all[1].buttonSouth.wasPressedThisFrame;
        }

        // ENTER or X
        if (Input.GetKeyDown(KeyCode.Return) || p2X)
        {
            appPanels[currentIndex].SetActive(true);
            appOpen = true;
        }
    }

    // -------------------------
    // BACK / CLOSE APP
    // -------------------------
    void HandleBack()
    {
        bool p2Circle = false;

        if (Gamepad.all.Count > 1)
        {
            p2Circle = Gamepad.all[1].buttonEast.wasPressedThisFrame;
        }

        // BACKSPACE or O
        if (Input.GetKeyDown(KeyCode.Backspace) || p2Circle)
        {
            appPanels[currentIndex].SetActive(false);
            appOpen = false;

            SelectButton(currentIndex);
        }
    }
}