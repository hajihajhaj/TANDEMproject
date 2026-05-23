using UnityEngine;
using UnityEngine.InputSystem;

public class BikeMovementLevelSelection : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotateSpeed = 10f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        // PLAYER 1
        if (Keyboard.current.sKey.isPressed) move += Vector3.forward;
        if (Keyboard.current.wKey.isPressed) move += Vector3.back;
        if (Keyboard.current.dKey.isPressed) move += Vector3.left;
        if (Keyboard.current.aKey.isPressed) move += Vector3.right;

        // PLAYER 2
        if (Keyboard.current.hKey.isPressed) move += Vector3.forward;
        if (Keyboard.current.yKey.isPressed) move += Vector3.back;
        if (Keyboard.current.jKey.isPressed) move += Vector3.left;
        if (Keyboard.current.gKey.isPressed) move += Vector3.right;

        move = move.normalized;

        if (move != Vector3.zero)
        {
            transform.position += move * moveSpeed * Time.deltaTime;

            Quaternion targetRotation =
                Quaternion.LookRotation(move);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotateSpeed * Time.deltaTime
                );
        }
    }
}