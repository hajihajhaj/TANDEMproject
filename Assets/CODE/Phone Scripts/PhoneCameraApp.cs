using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Capture")]
    public KeyCode captureKey = KeyCode.P;
    public GameObject shutterPanel;
    public float shutterFlashTime = 0.08f;

    void Update()
    {
        if (!cameraAppOpen) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ExitCameraApp();
        }

        // ?? TAKE PHOTO
        if (Input.GetKeyDown(captureKey))
        {
            StartCoroutine(TakePhoto());
        }
    }

    public void OpenCameraApp()
    {
        cameraAppOpen = true;

        phoneController.ForceOpenApp();

        mainCamera.enabled = false;

        cameraState = CameraState.Front;

        frontCamera.enabled = true;
        backCamera.enabled = false;

        currentCamera = frontCamera;

        cameraPanel.SetActive(true);
    }

    public void ExitCameraApp()
    {
        cameraAppOpen = false;

        frontCamera.enabled = false;
        backCamera.enabled = false;

        mainCamera.enabled = true;

        currentCamera = null;

        cameraPanel.SetActive(false);

        phoneController.CloseApp();
    }

    public void SwitchCamera()
    {
        if (cameraState == CameraState.Front)
        {
            cameraState = CameraState.Back;

            frontCamera.enabled = false;
            backCamera.enabled = true;

            currentCamera = backCamera;
        }
        else
        {
            cameraState = CameraState.Front;

            backCamera.enabled = false;
            frontCamera.enabled = true;

            currentCamera = frontCamera;
        }
    }

    // -------------------------
    // PHOTO SYSTEM
    // -------------------------
    IEnumerator TakePhoto()
    {
        // show shutter immediately
        if (shutterPanel != null)
            shutterPanel.SetActive(true);

        // IMPORTANT: wait until frame is fully rendered
        yield return new WaitForEndOfFrame();

        Texture2D photo = null;

        try
        {
            photo = ScreenCapture.CaptureScreenshotAsTexture();

            if (photo != null && galleryManager != null)
            {
                galleryManager.AddPhoto(photo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Photo capture failed: " + e.Message);
        }

        // ALWAYS hide shutter even if something breaks
        if (shutterPanel != null)
            shutterPanel.SetActive(false);
    }
}