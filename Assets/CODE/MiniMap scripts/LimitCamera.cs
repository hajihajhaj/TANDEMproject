using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    public Transform Player;

    private void LateUpdate()
    {
        Vector3 newPosition = Player.position;

        newPosition.y = transform.position.y;

        transform.position = newPosition;
    }
}