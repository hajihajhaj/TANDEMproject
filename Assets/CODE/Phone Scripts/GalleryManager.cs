using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GalleryManager : MonoBehaviour
{
    [Header("Gallery")]
    public GameObject galleryPanel;

    [Header("Photo Slots")]
    public Image[] photoSlots;

    [Header("Fullscreen")]
    public GameObject fullscreenPanel;
    public Image fullscreenImage;

    private List<Texture2D> photos = new List<Texture2D>();

    private bool[] occupiedSlots;

    private int currentPhotoIndex = -1;

    [Header("Navigation")]
    public Button[] photoButtons;
    public Button startingButton;

    public ScrollRect scrollRect;
    public int visibleRows = 2;

    private int currentIndex = 0;

    bool dpadInUse = false;

    // -------------------------

    void Start()
    {
        occupiedSlots = new bool[photoSlots.Length];

        // make all slots invisible initially
        for (int i = 0; i < photoSlots.Length; i++)
        {
            photoSlots[i].color = new Color(1, 1, 1, 0);
        }
    }


    void Update()
    {
        if (galleryPanel.activeSelf)
        {
            HandleNavigation();

            // close fullscreen only
            if (fullscreenPanel.activeSelf &&
                Input.GetKeyDown(KeyCode.Backspace))
            {
                CloseFullscreen();
            }

            // delete current photo
            if (fullscreenPanel.activeSelf &&
                Input.GetKeyDown(KeyCode.X))
            {
                DeleteCurrentPhoto();
            }
        }
    }

   void UpdateScroll()
{
    Canvas.ForceUpdateCanvases();

    RectTransform selected =
        photoButtons[currentIndex].GetComponent<RectTransform>();

    RectTransform content =
        scrollRect.content;

    float contentHeight = content.rect.height;
    float viewportHeight =
        scrollRect.viewport.rect.height;

    float targetY =
        Mathf.Abs(selected.anchoredPosition.y);

    float normalized =
        1 - Mathf.Clamp01(
            targetY / (contentHeight - viewportHeight)
        );

    scrollRect.verticalNormalizedPosition = normalized;
}

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

        // LEFT
        if (left && currentIndex % 2 == 1)
        {
            currentIndex--;
            SelectButton(currentIndex);
        }

        // RIGHT
        if (right && currentIndex % 2 == 0)
        {
            if (currentIndex + 1 < photoButtons.Length)
            {
                currentIndex++;
                SelectButton(currentIndex);
            }
        }

        // UP
        if (up && currentIndex >= 2)
        {
            currentIndex -= 2;

            SelectButton(currentIndex);

          
        }

        // DOWN
        if (down && currentIndex + 2 < photoButtons.Length)
        {
            currentIndex += 2;

            SelectButton(currentIndex);

           
        }
    }

    // -------------------------
    void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(
            photoButtons[index].gameObject
        );

        UpdateScroll();
    }

    
    // -------------------------
    public void OpenGallery()
    {
        galleryPanel.SetActive(true);

        scrollRect.verticalNormalizedPosition = 1f;

        // default selection
        if (startingButton != null)
        {
            EventSystem.current.SetSelectedGameObject(
                startingButton.gameObject
            );

            // sync index with button array
            for (int i = 0; i < photoButtons.Length; i++)
            {
                if (photoButtons[i] == startingButton)
                {
                    currentIndex = i;
                    break;
                }
            }
        }
        else
        {
            currentIndex = 0;
            SelectButton(currentIndex);
        }
    }

    public void CloseGallery()
    {
        fullscreenPanel.SetActive(false);
        galleryPanel.SetActive(false);
    }

    // -------------------------
    public void AddPhoto(Texture2D texture)
    {
        if (texture == null) return;

        int emptyIndex = -1;

        // find first empty slot
        for (int i = 0; i < occupiedSlots.Length; i++)
        {
            if (!occupiedSlots[i])
            {
                emptyIndex = i;
                break;
            }
        }

        // gallery full
        if (emptyIndex == -1)
        {
            Debug.Log("Gallery Full");
            return;
        }

        // store photo
        if (emptyIndex >= photos.Count)
        {
            photos.Add(texture);
        }
        else
        {
            photos[emptyIndex] = texture;
        }

        occupiedSlots[emptyIndex] = true;

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        photoSlots[emptyIndex].sprite = sprite;
        photoSlots[emptyIndex].color = Color.white;

        int capturedIndex = emptyIndex;

        Button btn = photoSlots[emptyIndex].GetComponent<Button>();

        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(() =>
        {
            OpenPhoto(capturedIndex);
        });
    }

    // -------------------------
    public void OpenPhoto(int index)
    {
        if (index < 0 || index >= photos.Count)
            return;

        currentPhotoIndex = index;

        Texture2D texture = photos[index];

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        fullscreenImage.sprite = sprite;

        fullscreenPanel.SetActive(true);
    }

    public void DeleteCurrentPhoto()
    {
        if (currentPhotoIndex < 0 ||
            currentPhotoIndex >= photoSlots.Length)
            return;

        occupiedSlots[currentPhotoIndex] = false;

        if (currentPhotoIndex < photos.Count)
        {
            photos[currentPhotoIndex] = null;
        }

        // clear slot visually
        photoSlots[currentPhotoIndex].sprite = null;

        photoSlots[currentPhotoIndex].color =
            new Color(1, 1, 1, 0);

        fullscreenPanel.SetActive(false);
    }

    // -------------------------
    public void CloseFullscreen()
    {
        fullscreenPanel.SetActive(false);
    }
}