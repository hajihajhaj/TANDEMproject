using UnityEngine;

public class BikeCustomization : MonoBehaviour
{
    [Header("Bike Parts")]
    public Renderer[] bikeParts;

    [Header("Colors")]
    public Color red = new Color32(255, 32, 51, 255);
    public Color purple = new Color32(157, 0, 255, 255);
    public Color blue = new Color32(0, 162, 255, 255);

    private Color selectedColor;

    void Start()
    {
        selectedColor = red;
    }

    public void Red()
    {
        selectedColor = red;
    }

    public void Purple()
    {
        selectedColor = purple;
    }

    public void Blue()
    {
        selectedColor = blue;
    }

    public void Select()
    {
        foreach (Renderer part in bikeParts)
        {
            part.material.color = selectedColor;
        }
    }
}