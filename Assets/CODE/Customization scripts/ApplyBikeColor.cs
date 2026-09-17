using UnityEngine;

public class ApplyBikeColor : MonoBehaviour
{
    [Header("Bike Parts")]
    public Renderer[] bikeParts;

    void Start()
    {
        float r = PlayerPrefs.GetFloat("BikeColorR", 1f);
        float g = PlayerPrefs.GetFloat("BikeColorG", 0.125f);
        float b = PlayerPrefs.GetFloat("BikeColorB", 0.2f);

        Color selectedColor = new Color(r, g, b, 1f);

        foreach (Renderer part in bikeParts)
        {
            part.material.color = selectedColor;
        }
    }
}