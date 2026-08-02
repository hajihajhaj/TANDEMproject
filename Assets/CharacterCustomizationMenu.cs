using UnityEngine;

public class CharacterCustomizationMenu : MonoBehaviour
{
    [Header("Cameras")]
    public Camera gameplayCamera;
    public Camera customizationCamera;

    [Header("UI")]
    public GameObject customizationUI;

    [Header("Unsaved Popup")]
    public GameObject unsavedPopup;

    [Header("Players")]
    public CharacterCustomization player1Customization;
    public CharacterCustomization player2Customization;

    [Header("Disable Movement")]
    public MonoBehaviour[] playerMovementScripts;

    void Start()
    {
        if (customizationCamera != null)
            customizationCamera.gameObject.SetActive(false);

        if (customizationUI != null)
            customizationUI.SetActive(false);

        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);
    }

    public void OpenMenu()
    {
        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(false);

        if (customizationCamera != null)
            customizationCamera.gameObject.SetActive(true);

        if (customizationUI != null)
            customizationUI.SetActive(true);

        // Always hide the popup when opening
        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        foreach (MonoBehaviour movement in playerMovementScripts)
        {
            if (movement != null)
                movement.enabled = false;
        }
    }

    public void AttemptCloseMenu()
    {
        if (player1Customization.IsDone() &&
            player2Customization.IsDone())
        {
            CloseMenu();
        }
        else
        {
            if (unsavedPopup != null)
                unsavedPopup.SetActive(true);
        }
    }

    public void SaveAndClose()
    {
        player1Customization.SaveCustomization();
        player2Customization.SaveCustomization();

        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        CloseMenu();
    }

    public void CloseWithoutSaving()
    {
        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        CloseMenu();
    }

    public void CancelClose()
    {
        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);
    }

    public void CloseMenu()
    {
        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(true);

        if (customizationCamera != null)
            customizationCamera.gameObject.SetActive(false);

        if (customizationUI != null)
            customizationUI.SetActive(false);

        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        foreach (MonoBehaviour movement in playerMovementScripts)
        {
            if (movement != null)
                movement.enabled = true;
        }
    }
}