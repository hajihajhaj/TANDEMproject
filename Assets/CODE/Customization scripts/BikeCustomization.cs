using UnityEngine;
using UnityEngine.UI;

public class BikeCustomization : MonoBehaviour
{
    [Header("Bike Preview Images")]
    public GameObject[] bikeImages;

    [Header("Colors")]
    public Color[] bikeColors =
    {
        new Color32(255, 255, 0, 255),     // Yellow - Default
        new Color32(255, 32, 51, 255),     // Red
        new Color32(157, 0, 255, 255),     // Purple
        new Color32(0, 162, 255, 255)      // Blue
    };

    private int currentColor = 0;

    void Start()
    {
        ShowBike();
    }

    public void Forward()
    {
        currentColor++;

        if (currentColor >= bikeImages.Length)
        {
            currentColor = 0;
        }

        ShowBike();
    }

    public void Back()
    {
        currentColor--;

        if (currentColor < 0)
        {
            currentColor = bikeImages.Length - 1;
        }

        ShowBike();
    }

    void ShowBike()
    {
        for (int i = 0; i < bikeImages.Length; i++)
        {
            bikeImages[i].SetActive(i == currentColor);
        }
    }

    public void Select()
    {
        Color color = bikeColors[currentColor];

        PlayerPrefs.SetFloat("BikeColorR", color.r);
        PlayerPrefs.SetFloat("BikeColorG", color.g);
        PlayerPrefs.SetFloat("BikeColorB", color.b);
        PlayerPrefs.Save();

        Debug.Log("Bike color saved!");
    }
}