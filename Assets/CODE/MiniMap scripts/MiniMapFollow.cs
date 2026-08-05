using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public RectTransform miniMapImage;
    public RectTransform megaMapArrow;

    void Update()
    {
        Debug.Log("Arrow: " + megaMapArrow.name + " POS: " + megaMapArrow.anchoredPosition);
    }
}