using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Header("Settings Pages")]
    public GameObject mainMenu;
    public GameObject phoneColours;
    public GameObject wallpapers;
    public GameObject notificationSounds;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        phoneColours.SetActive(false);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(false);
    }

    public void ShowPhoneColours()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(true);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(false);
    }

    public void ShowWallpapers()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(false);
        wallpapers.SetActive(true);
        notificationSounds.SetActive(false);
    }

    public void ShowNotificationSounds()
    {
        mainMenu.SetActive(false);
        phoneColours.SetActive(false);
        wallpapers.SetActive(false);
        notificationSounds.SetActive(true);
    }
}