using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player;

    public RectTransform mapImage;

    public Vector2 worldMin = new Vector2(-765f, -398f);
    public Vector2 worldMax = new Vector2(214f, 514f);

    void Update()
    {
        float width = mapImage.rect.width;
        float height = mapImage.rect.height;

        float worldWidth = worldMax.x - worldMin.x;
        float worldHeight = worldMax.y - worldMin.y;

        float x =
            (player.position.x - worldMin.x) /
            worldWidth * width;

        float y =
            (player.position.z - worldMin.y) /
            worldHeight * height;

        mapImage.anchoredPosition =
            new Vector2(
                width * .5f - x,
                height * .5f - y
            );
    }
}