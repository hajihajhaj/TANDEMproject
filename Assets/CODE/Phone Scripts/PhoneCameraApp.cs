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

    [Header("Render Texture")]
    public RenderTexture renderTexture;

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

        if (Input.GetKeyDown(captureKey))
        {
            StartCoroutine(TakePhoto());
        }
    }

    public void OpenCameraApp()
    {
        cameraAppOpen = true;


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