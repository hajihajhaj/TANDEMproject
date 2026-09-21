using UnityEngine;

public class ApplyBikeColor : MonoBehaviour
{
    [Header("Bike Parts")]
    public Renderer[] bikeParts;


[Header("Bike Colors")]
    public Color[] bikeColors =
{
    new Color32(223, 255, 19, 255),     // Yellow #DFFF13
    new Color32(255, 32, 51, 255),      // Red #FF2033
    new Color32(157, 0, 255, 255),      // Purple #9D00FF
    new Color32(0, 162, 255, 255)       // Blue #00A2FF
};

    void Start()
    {
        UpdateBikeColor();
    }

    public void UpdateBikeColor()
    {
        int selectedColor = PlayerPrefs.GetInt("BikeColor", 0);

        if (selectedColor < 0 || selectedColor >= bikeColors.Length)
        {
            selectedColor = 0;
        }

        Color color = bikeColors[selectedColor];

        foreach (Renderer part in bikeParts)
        {
            if (part != null)
            {
                part.material.color = color;
            }
        }
    }

}
