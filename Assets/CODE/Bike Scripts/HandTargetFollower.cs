using UnityEngine;

public class HandTargetFollower : MonoBehaviour
{
    public Transform source;

    void LateUpdate()
    {
        transform.position = source.position;
        transform.rotation = source.rotation;
    }
}