using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class PhoneCameraApp : MonoBehaviour
{
    [Header("Scene Cameras")]
    public Camera mainCamera;
    public Camera frontCamera;
    public Camera backCamera;

    [Header("Gallery")]
    public GalleryManager galleryManager;

    public PhoneController phoneController;

    private enum CameraState
    {
        Front,
        Back
    }

    private CameraState cameraState;
    private Camera currentCamera;

    private bool cameraAppOpen = false;

    [Header("UI")]
    public GameObject cameraPanel;

    [Header("Render Texture")]
    public RenderTexture renderTexture;

    [Header("Capture")]
    public KeyCode captureKey = KeyCode.P;
    public GameObject shutterPanel;
    public float shutterFlashTime = 0.08f;

    [Header("Look Settings")]
    public float lookSpeed = 150f;

    float pitch;
    float yaw;

    public float frontOrbitDistance = 2f;
    public float frontOrbitHeight = 1.5f;
    public float frontOrbitLimit = 45f;

    Vector3 frontCameraStartPosition;

    Quaternion frontStartRotation;
    Quaternion backStartRotation;

    Vector3 frontStartPosition;


    public TandemBikeController bikeController;

    void Update()
    {
        if (!cameraAppOpen)
            return;

        bool p2Triangle = false;
        bool p2R3 = false;
        bool p2Circle = false;
        Vector2 p2Look = Vector2.zero;

        if (Gamepad.all.Count > 1)
        {
            p2Triangle = Gamepad.all[1].buttonNorth.wasPressedThisFrame;      // Triangle
            p2R3 = Gamepad.all[1].rightStickButton.wasPressedThisFrame;        // R3
            p2Circle = Gamepad.all[1].buttonEast.wasPressedThisFrame;          // Circle/B
            p2Look = Gamepad.all[1].rightStick.ReadValue();                    // Right Stick
        }

        // Keyboard input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Use mouse by default
        float lookX = mouseX;
        float lookY = mouseY;

        // If Player 2 is using the right stick, use that instead
        if (Gamepad.all.Count > 1)
        {
            if (p2Look.sqrMagnitude > 0.001f)
            {
                lookX = p2Look.x;
                lookY = p2Look.y;
            }
        }

        // BACK CAMERA
        if (currentCamera == backCamera)
        {
            if (Gamepad.all.Count > 1)
            {
                // Player 2 right stick controls bike rotation
                bikeController.phoneCameraTurnInput = p2Look.x;
            }

            // Only control vertical look
            pitch -= p2Look.y * lookSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -35f, 35f);

            // Keep camera aligned with bike rotation
            backCamera.transform.localRotation =
                Quaternion.Euler(pitch, 0f, 0f);
        }

        // FRONT CAMERA
        else if (currentCamera == frontCamera)
        {
            float frontLimit = 90f;

            yaw += lookX * lookSpeed * Time.deltaTime;


            // Once camera reaches the side limit, rotate the bike
            if (yaw > frontLimit)
            {
                float overflow = yaw - frontLimit;
                yaw = frontLimit;

                bikeController.RotateFromPhone(overflow);
            }
            else if (yaw < -frontLimit)
            {
                float overflow = yaw + frontLimit;
                yaw = -frontLimit;

                bikeController.RotateFromPhone(overflow);
            }


            yaw = Mathf.Clamp(yaw, -frontLimit, frontLimit);


            // Move camera sideways around the bike
            float sideMovement = Mathf.Sin(yaw * Mathf.Deg2Rad);


            frontCamera.transform.localPosition =
                frontStartPosition +
                new Vector3(
                    sideMovement * 2f,
                    0f,
                    0f
                );


            // Keep the camera facing the player
            frontCamera.transform.localRotation =
                frontStartRotation;
        }

        // Switch camera (C or R3)
        if (Input.GetKeyDown(KeyCode.C) || p2R3)
        {
            SwitchCamera();
        }

        // Exit (Backspace or Circle)
        if (Input.GetKeyDown(KeyCode.Backspace) || p2Circle)
        {
            ExitCameraApp();
        }

        // Take photo (P or Triangle)
        if (Input.GetKeyDown(captureKey) || p2Triangle)
        {
            StartCoroutine(TakePhoto());
        }
    }
    public void OpenCameraApp()
    {
        bikeController.phoneCameraOpen = true;

        cameraAppOpen = true;

        cameraState = CameraState.Front;

        frontCamera.enabled = true;
        backCamera.enabled = false;

        currentCamera = frontCamera;

        // SAVE ORIGINAL CAMERA ROTATIONS
        frontStartRotation = frontCamera.transform.localRotation;
        backStartRotation = backCamera.transform.localRotation;

        frontStartPosition = frontCamera.transform.localPosition;

        frontCameraStartPosition = frontCamera.transform.localPosition;

        yaw = 0f;
        pitch = 0f;

        cameraPanel.SetActive(true);
    }

    public void ExitCameraApp()
    {
        bikeController.phoneCameraTurnInput = 0f;
        bikeController.phoneCameraOpen = false;
        cameraAppOpen = false;

        frontCamera.enabled = false;
        backCamera.enabled = false;


        currentCamera = null;

        cameraPanel.SetActive(false);

        // ? IMPORTANT FIX:
        // Do NOT close the whole phone or app system
        // Just tell controller to return to phone home UI
        phoneController.ReturnToHome();
    }

    public void SwitchCamera()
    {
        if (cameraState == CameraState.Front)
        {
            cameraState = CameraState.Back;

            frontCamera.enabled = false;
            backCamera.enabled = true;

            currentCamera = backCamera;

            yaw = 0f;
            pitch = 0f;

            frontCamera.transform.localPosition = frontCameraStartPosition;
            frontCamera.transform.localRotation = frontStartRotation;
            
            backCamera.transform.localRotation = backStartRotation;
        }
        else
        {
            bikeController.phoneCameraTurnInput = 0f;

            cameraState = CameraState.Front;

            backCamera.enabled = false;
            frontCamera.enabled = true;

            currentCamera = frontCamera;

            yaw = 0f;
            pitch = 0f;
            //frontCamera.transform.localRotation = frontStartRotation;
        }
    }

    // -------------------------
    // PHOTO SYSTEM
    // -------------------------
    IEnumerator TakePhoto()
    {
        // STOP if storage full
        if (galleryManager != null &&
     galleryManager.IsGalleryFull())
        {
            galleryManager.ShowFullPopup();
            yield break;
        }
        // shutter flash
        if (shutterPanel != null)
            shutterPanel.SetActive(true);

        yield return new WaitForEndOfFrame();

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture.active = renderTexture;

        currentCamera.Render();

        Texture2D photo = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            TextureFormat.RGB24,
            false
        );

        photo.ReadPixels(
            new Rect(0, 0, renderTexture.width, renderTexture.height),
            0,
            0
        );

        photo.Apply();

        RenderTexture.active = currentRT;

        if (galleryManager != null)
        {
            galleryManager.AddPhoto(photo);
        }

        // remove shutter
        if (shutterPanel != null)
            shutterPanel.SetActive(false);
    }
}