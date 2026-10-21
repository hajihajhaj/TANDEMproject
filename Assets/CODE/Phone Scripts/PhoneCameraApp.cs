
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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

    [Header("UI Elements to Hide While Camera Is Open")]
    public List<GameObject> uiElementsToHide = new List<GameObject>();

    private Dictionary<GameObject, bool> originalUIStates =
        new Dictionary<GameObject, bool>();

    private bool uiElementsHidden = false;

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

    Quaternion frontStartRotation;
    Quaternion backStartRotation;

    Vector3 frontStartPosition;

    public float selfieLookHeight = 2.8f;
    public float selfieTilt = -8f;

    public TandemBikeController bikeController;

    void Start()
    {
        frontStartPosition = frontCamera.transform.localPosition;

        frontStartRotation = frontCamera.transform.localRotation;
        backStartRotation = backCamera.transform.localRotation;
    }

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
            p2Triangle = Gamepad.all[1].buttonNorth.wasPressedThisFrame;
            p2R3 = Gamepad.all[1].rightStickButton.wasPressedThisFrame;
            p2Circle = Gamepad.all[1].buttonEast.wasPressedThisFrame;
            p2Look = Gamepad.all[1].rightStick.ReadValue();
        }

        // Keyboard input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float lookX = mouseX;
        float lookY = mouseY;

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
                bikeController.phoneCameraTurnInput = p2Look.x;
            }

            pitch -= p2Look.y * lookSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -35f, 35f);

            backCamera.transform.localRotation =
                Quaternion.Euler(pitch, 0f, 0f);
        }

        // FRONT CAMERA
        else if (currentCamera == frontCamera)
        {
            float frontLimit = 90f;

            yaw += lookX * lookSpeed * Time.deltaTime;

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

            pitch -= lookY * lookSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -25f, 25f);

            float radius = Mathf.Abs(frontStartPosition.z);

            float yawRad = yaw * Mathf.Deg2Rad;
            float pitchRad = pitch * Mathf.Deg2Rad;

            frontCamera.transform.localPosition = new Vector3(
                Mathf.Sin(yawRad) * Mathf.Cos(pitchRad) * radius,
                frontStartPosition.y + Mathf.Sin(pitchRad) * radius,
                Mathf.Cos(yawRad) * Mathf.Cos(pitchRad) * radius
            );

            frontCamera.transform.LookAt(
                bikeController.transform.position +
                Vector3.up * selfieLookHeight
            );

            frontCamera.transform.Rotate(
                selfieTilt, 0f, 0f, Space.Self
            );
        }

        // Switch camera
        if (Input.GetKeyDown(KeyCode.C) || p2R3)
        {
            SwitchCamera();
        }

        // Exit camera app
        if (Input.GetKeyDown(KeyCode.Backspace) || p2Circle)
        {
            ExitCameraApp();
        }

        // Take photo
        if (Input.GetKeyDown(captureKey) || p2Triangle)
        {
            StartCoroutine(TakePhoto());
        }
    }

    // -------------------------
    // HIDE AND RESTORE UI
    // -------------------------

    void HideUIElements()
    {
        if (uiElementsHidden)
            return;

        originalUIStates.Clear();

        foreach (GameObject obj in uiElementsToHide)
        {
            if (obj == null)
                continue;

            // Save the object's current active state
            if (!originalUIStates.ContainsKey(obj))
            {
                originalUIStates.Add(obj, obj.activeSelf);
            }

            // Hide without destroying or resetting the object
            obj.SetActive(false);
        }

        uiElementsHidden = true;
    }

    void RestoreUIElements()
    {
        if (!uiElementsHidden)
            return;

        foreach (KeyValuePair<GameObject, bool> entry in originalUIStates)
        {
            if (entry.Key != null)
            {
                // Restore its original active state
                entry.Key.SetActive(entry.Value);
            }
        }

        originalUIStates.Clear();
        uiElementsHidden = false;
    }

    // -------------------------
    // OPEN CAMERA APP
    // -------------------------

    public void OpenCameraApp()
    {
        bikeController.phoneCameraOpen = true;

        // Hide selected UI elements
        HideUIElements();

        cameraAppOpen = true;

        cameraState = CameraState.Front;

        frontCamera.enabled = true;
        backCamera.enabled = false;

        currentCamera = frontCamera;

        yaw = 0f;
        pitch = 0f;

        cameraPanel.SetActive(true);
    }

    // -------------------------
    // EXIT CAMERA APP
    // -------------------------

    public void ExitCameraApp()
    {
        bikeController.phoneCameraTurnInput = 0f;
        bikeController.phoneCameraOpen = false;
        cameraAppOpen = false;

        frontCamera.enabled = false;
        backCamera.enabled = false;

        currentCamera = null;

        cameraPanel.SetActive(false);

        // Restore selected UI elements to their original states
        RestoreUIElements();

        // Return to phone home UI
        phoneController.ReturnToHome();
    }

    // -------------------------
    // SWITCH CAMERA
    // -------------------------

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

            frontCamera.transform.localPosition = frontStartPosition;
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
        }
    }

    // -------------------------
    // PHOTO SYSTEM
    // -------------------------

    IEnumerator TakePhoto()
    {
        // Stop if storage is full
        if (galleryManager != null &&
            galleryManager.IsGalleryFull())
        {
            galleryManager.ShowFullPopup();
            yield break;
        }

        // Shutter flash
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
            new Rect(
                0, 0,
                renderTexture.width,
                renderTexture.height
            ),
            0,
            0
        );

        photo.Apply();

        RenderTexture.active = currentRT;

        if (galleryManager != null)
        {
            galleryManager.AddPhoto(photo);
        }

        // Remove shutter
        if (shutterPanel != null)
            shutterPanel.SetActive(false);
    }
}
