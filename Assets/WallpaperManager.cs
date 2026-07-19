using UnityEngine;

public class WallpaperManager : MonoBehaviour
{
    [Header("Wallpaper Objects")]
    public GameObject defaultWallpaper;
    public GameObject starWallpaper;
    public GameObject bikeWallpaper;

    void Start()
    {
        LoadWallpaper();
    }

    public void SetDefault()
    {
        ChangeWallpaper(defaultWallpaper);
        PlayerPrefs.SetInt("Wallpaper", -1);
        PlayerPrefs.Save();
    }

    public void SetStar()
    {
        ChangeWallpaper(starWallpaper);
        PlayerPrefs.SetInt("Wallpaper", 0);
        PlayerPrefs.Save();
    }

    public void SetBike()
    {
        ChangeWallpaper(bikeWallpaper);
        PlayerPrefs.SetInt("Wallpaper", 1);
        PlayerPrefs.Save();
    }

    private void ChangeWallpaper(GameObject wallpaper)
    {
        defaultWallpaper.SetActive(false);
        starWallpaper.SetActive(false);
        bikeWallpaper.SetActive(false);

        wallpaper.SetActive(true);
    }

    private void LoadWallpaper()
    {
        int savedWallpaper = PlayerPrefs.GetInt("Wallpaper", -1);

        switch (savedWallpaper)
        {
            case -1:
                ChangeWallpaper(defaultWallpaper);
                break;

            case 0:
                ChangeWallpaper(starWallpaper);
                break;

            case 1:
                ChangeWallpaper(bikeWallpaper);
                break;

            default:
                ChangeWallpaper(defaultWallpaper);
                break;
        }
    }
}