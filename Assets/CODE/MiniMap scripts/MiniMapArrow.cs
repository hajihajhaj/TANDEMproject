using UnityEngine;

public class MiniMapArrow : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        transform.localEulerAngles = new Vector3(
            0f,
            0f,
            -player.eulerAngles.y + 180f
        );
    }
}