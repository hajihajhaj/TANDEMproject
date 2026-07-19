using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;

public class PhoneController : MonoBehaviour
{
    [Header("Phone")]
    public GameObject phoneUI;
    public Vector2 closedPosition;
    public Vector2 openPosition;

    public float slideSpeed = 0.3f;

    [Header("Apps")]
    public Button[] appButtons;

    [Header("Opened App Panels")]
    public GameObject[] appPanels;

    [Header("Home Screen")]
    public GameObject homeScreen;

    int currentIndex = 0;

    bool phoneOpen = false;
    bool appOpen = false;

    bool dpadInUse = false;

    Coroutine phoneAnimation;
    bool isAnimating;

    void Start()
    {
        phoneUI.SetActive(true);
        phoneUI.GetComponent<RectTransform>().anchoredPosition = closedPosition;

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
    // PHONE TOGGLE
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

        if (Input.GetKeyDown(KeyCode.Tab) || p2DpadUp)
        {
            if (!phoneOpen && !isAnimating)
            {
                phoneOpen = true;

                StartPhoneAnimation(openPosition);

                SelectButton(currentIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || p2Circle)
        {
            if (!appOpen)
            {
                if (!isAnimating)
                {
                    phoneOpen = false;

                    StartPhoneAnimation(closedPosition);

                    CloseAllApps();
                }
            }
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

            if (Mathf.Abs(dpad.x) < 0.5f && Mathf.Abs(dpad.y) < 0.5f)
                dpadInUse = false;

            if (!dpadInUse)
            {
                if (dpad.x < -0.5f) { left = true; dpadInUse = true; }
                if (dpad.x > 0.5f) { right = true; dpadInUse = true; }
                if (dpad.y > 0.5f) { up = true; dpadInUse = true; }
                if (dpad.y < -0.5f) { down = true; dpadInUse = true; }
            }
        }

        if (left && currentIndex % 2 == 1) { currentIndex--; SelectButton(currentIndex); }
        if (right && currentIndex % 2 == 0) { currentIndex++; SelectButton(currentIndex); }
        if (up && currentIndex >= 2) { currentIndex -= 2; SelectButton(currentIndex); }
        if (down && currentIndex <= 1) { currentIndex += 2; SelectButton(currentIndex); }
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
            p2X = Gamepad.all[1].buttonSouth.wasPressedThisFrame;

        if (Input.GetKeyDown(KeyCode.Return) || p2X)
        {
            // CAMERA APP
            if (currentIndex == 2)
            {
                FindObjectOfType<PhoneCameraApp>().OpenCameraApp();
                appOpen = true;
                return;
            }

            int panelIndex = currentIndex;

            if (currentIndex > 2)
                panelIndex--;

            homeScreen.SetActive(false);
            appPanels[panelIndex].SetActive(true);
            appOpen = true;
        }
    }

    // -------------------------
    // BACK
    // -------------------------
    void HandleBack()
    {
        bool p2Circle = false;

        if (Gamepad.all.Count > 1)
            p2Circle = Gamepad.all[1].buttonEast.wasPressedThisFrame;

        if (Input.GetKeyDown(KeyCode.Backspace) || p2Circle)
        {
            SettingsMenu settings = FindObjectOfType<SettingsMenu>();

            // If we're inside a Settings sub-page, go back to the Settings main menu.
            if (settings != null && settings.IsOnSubPage())
            {
                settings.OpenMainMenu();
                return;
            }

            // Otherwise close the current app.
            if (appOpen)
            {
                CloseAllApps();
                appOpen = false;
            }
        }
    }


    public void CloseApp()
    {
        appOpen = false;

        foreach (GameObject panel in appPanels)
        {
            panel.SetActive(false);
        }

        // return to phone HOME (but DO NOT force open UI)
        SelectButton(currentIndex);
    }

    // -------------------------
    // CENTRAL RESET (IMPORTANT FIX)
    // -------------------------
    void CloseAllApps()
    {
        foreach (GameObject panel in appPanels)
        {
            panel.SetActive(false);
        }

        homeScreen.SetActive(true);

        FindObjectOfType<PhoneCameraApp>()?.ExitCameraApp();
    }

    public void ReturnToHome()
    {
        appOpen = false;

        foreach (GameObject panel in appPanels)
        {
            panel.SetActive(false);
        }

        homeScreen.SetActive(true);

        SelectButton(currentIndex);
    }

    void StartPhoneAnimation(Vector2 targetPosition)
    {
        if (phoneAnimation != null)
            StopCoroutine(phoneAnimation);

        phoneAnimation = StartCoroutine(
            SlidePhone(targetPosition)
        );
    }


    IEnumerator SlidePhone(Vector2 targetPosition)
    {
        isAnimating = true;

        RectTransform rect =
            phoneUI.GetComponent<RectTransform>();

        Vector2 startPosition = rect.anchoredPosition;

        float time = 0;

        while (time < slideSpeed)
        {
            time += Time.deltaTime;

            float t = time / slideSpeed;

            rect.anchoredPosition =
                Vector2.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            yield return null;
        }

        rect.anchoredPosition = targetPosition;

        isAnimating = false;
    }
}