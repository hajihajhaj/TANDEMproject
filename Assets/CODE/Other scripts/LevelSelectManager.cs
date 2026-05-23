using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera;
    public Camera levelSelectCamera;

    [Header("Movement")]
    public TwoPlayerMovement playerMovement;
    public BikeMovementLevelSelection bikeMovement;

    [Header("UI")]
    public GameObject enterPromptUI;

    private bool playerNearTrigger = false;
    private bool inLevelSelect = false;

    void Start()
    {
        levelSelectCamera.gameObject.SetActive(false);

        if (bikeMovement != null)
            bikeMovement.enabled = false;
    }

    void Update()
    {
        // ENTER MAP
        if (playerNearTrigger &&
            !inLevelSelect &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            EnterLevelSelect();
        }

        // EXIT MAP
        if (inLevelSelect &&
            Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            ExitLevelSelect();
        }
    }

    void EnterLevelSelect()
    {
        inLevelSelect = true;

        mainCamera.gameObject.SetActive(false);
        levelSelectCamera.gameObject.SetActive(true);

        playerMovement.canMove = false;

        bikeMovement.enabled = true;

        if (enterPromptUI != null)
            enterPromptUI.SetActive(false);
    }

    void ExitLevelSelect()
    {
        inLevelSelect = false;

        levelSelectCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        playerMovement.canMove = true;

        bikeMovement.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearTrigger = true;

            if (enterPromptUI != null)
                enterPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearTrigger = false;

            if (!inLevelSelect && enterPromptUI != null)
                enterPromptUI.SetActive(false);
        }
    }
}