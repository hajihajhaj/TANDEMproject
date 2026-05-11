using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GalleryManager : MonoBehaviour
{
    [Header("Gallery")]
    public GameObject galleryPanel;
    public Transform contentParent;
    public GameObject photoPrefab;

    [Header("Fullscreen")]
    public GameObject fullscreenPanel;
    public Image fullscreenImage;

    public int maxPhotos = 20;

    public List<Texture2D> photos = new List<Texture2D>();

    private Texture2D currentSelectedPhoto;

    public void OpenGallery()
    {
        galleryPanel.SetActive(true);
    }

    public void CloseGallery()
    {
        fullscreenPanel.SetActive(false);
        galleryPanel.SetActive(false);
    }

    public void AddPhoto(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("Tried to add null photo");
            return;
        }

        photos.Add(texture);

        GameObject newPhoto = Instantiate(photoPrefab, contentParent);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        newPhoto.GetComponent<Image>().sprite = sprite;

        newPhoto.GetComponent<Button>()
            .onClick.AddListener(() => OpenPhoto(texture));
    }

    public void OpenPhoto(Texture2D texture)
    {
        currentSelectedPhoto = texture;

        fullscreenPanel.SetActive(true);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        fullscreenImage.sprite = sprite;
    }

    public void CloseFullscreen()
    {
        fullscreenPanel.SetActive(false);
    }

    public void DeleteCurrentPhoto()
    {
        if (currentSelectedPhoto == null) return;

        photos.Remove(currentSelectedPhoto);

        RefreshGallery();

        fullscreenPanel.SetActive(false);
    }

    void RefreshGallery()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (Texture2D texture in photos)
        {
            GameObject newPhoto = Instantiate(photoPrefab, contentParent);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            newPhoto.GetComponent<Image>().sprite = sprite;

            newPhoto.GetComponent<Button>()
                .onClick.AddListener(() => OpenPhoto(texture));
        }
    }
}