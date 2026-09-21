using UnityEngine;
using TMPro;

public class BikeCustomization : MonoBehaviour
{
    [Header("Bike Preview Images")]
    public GameObject[] bikeImages;

    [Header("Select Button Text")]
    public TMP_Text selectButtonText;

    private int currentColor = 0;
    private int selectedColor = 0;

    void Start()
    {
        // Load the saved bike color
        selectedColor = PlayerPrefs.GetInt("BikeColor", 0);

        // Start on the saved bike color
        currentColor = selectedColor;

        ShowBike();
        UpdateSelectButton();
    }

    public void Forward()
    {
        currentColor++;

        if (currentColor >= bikeImages.Length)
        {
            currentColor = 0;
        }

        ShowBike();
        UpdateSelectButton();
    }

    public void Back()
    {
        currentColor--;

        if (currentColor < 0)
        {
            currentColor = bikeImages.Length - 1;
        }

        ShowBike();
        UpdateSelectButton();
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
        selectedColor = currentColor;

        PlayerPrefs.SetInt("BikeColor", selectedColor);
        PlayerPrefs.Save();

        UpdateSelectButton();

        Debug.Log("Bike color saved! Selected color: " + selectedColor);
    }

    void UpdateSelectButton()
    {
        if (currentColor == selectedColor)
        {
            selectButtonText.text = "Selected";
        }
        else
        {
            selectButtonText.text = "Select";
        }
    }
}