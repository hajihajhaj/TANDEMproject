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

    [Header("Player Movement")]
    public TwoPlayerMovement playerMovement;


    void Start()
    {
        // Make sure customization starts closed
        if (customizationCamera != null)
            customizationCamera.gameObject.SetActive(false);

        if (customizationUI != null)
            customizationUI.SetActive(false);

        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        // Make sure players can move when the game starts
        if (playerMovement != null)
            playerMovement.canMove = true;
    }


    //=========================================
    // OPEN CUSTOMIZATION
    //=========================================

    public void OpenMenu()
    {
        Debug.Log("OPENING CHARACTER CUSTOMIZATION");

        // Stop player movement
        if (playerMovement != null)
            playerMovement.canMove = false;

        // Hide gameplay camera
        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(false);

        // Show customization camera
        if (customizationCamera != null)
            customizationCamera.gameObject.SetActive(true);

        // Show customization UI
        if (customizationUI != null)
            customizationUI.SetActive(true);

        // Hide unsaved popup
        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);
    }


    //=========================================
    // X BUTTON
    //=========================================

    public void CloseMenu()
    {
        Debug.Log("CLOSING CHARACTER CUSTOMIZATION");

        // Hide customization UI
        if (customizationUI != null)
            customizationUI.SetActive(false);

        // Hide customization camera
        if (customizationCamera != null)
            customizationCamera.gameObject.SetActive(false);

        // Show gameplay camera
        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(true);

        // Hide popup
        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        // IMPORTANT:
        // Allow both players to move again
        if (playerMovement != null)
        {
            playerMovement.canMove = true;

            Debug.Log("PLAYER MOVEMENT ENABLED");
            Debug.Log("canMove = " + playerMovement.canMove);
        }
        else
        {
            Debug.LogError("TwoPlayerMovement is NOT assigned!");
        }
    }


    //=========================================
    // X BUTTON WITH UNSAVED CHECK
    //=========================================

    public void AttemptCloseMenu()
    {
        // If both players saved, close normally
        if (player1Customization != null &&
            player2Customization != null &&
            player1Customization.IsDone() &&
            player2Customization.IsDone())
        {
            CloseMenu();
        }
        else
        {
            // Show unsaved changes popup
            if (unsavedPopup != null)
                unsavedPopup.SetActive(true);
        }
    }


    //=========================================
    // SAVE AND CLOSE
    //=========================================

    public void SaveAndClose()
    {
        Debug.Log("SAVING BOTH PLAYERS");

        if (player1Customization != null)
            player1Customization.SaveCustomization();

        if (player2Customization != null)
            player2Customization.SaveCustomization();

        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        CloseMenu();
    }


    //=========================================
    // CLOSE WITHOUT SAVING
    //=========================================

    public void CloseWithoutSaving()
    {
        Debug.Log("CLOSING WITHOUT SAVING");

        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);

        CloseMenu();
    }


    //=========================================
    // CANCEL POPUP
    //=========================================

    public void CancelClose()
    {
        if (unsavedPopup != null)
            unsavedPopup.SetActive(false);
    }
}