using UnityEngine;
using UnityEngine.InputSystem;

public class BikeMovementLevelSelection : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotateSpeed = 10f;
    public float smoothTime = 0.15f;

    private Vector3 currentMove;
    private Vector3 moveVelocity;

    void Update()
    {
        Vector3 targetMove = Vector3.zero;

        // PLAYER 1
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) targetMove += Vector3.forward;
            if (Keyboard.current.dKey.isPressed) targetMove += Vector3.back;
            if (Keyboard.current.sKey.isPressed) targetMove += Vector3.left;
            if (Keyboard.current.wKey.isPressed) targetMove += Vector3.right;

            // PLAYER 2
            if (Keyboard.current.gKey.isPressed) targetMove += Vector3.forward;
            if (Keyboard.current.jKey.isPressed) targetMove += Vector3.back;
            if (Keyboard.current.hKey.isPressed) targetMove += Vector3.left;
            if (Keyboard.current.yKey.isPressed) targetMove += Vector3.right;
        }

        // Controllers (supports both)
        foreach (Gamepad pad in Gamepad.all)
        {
            Vector2 stick = pad.leftStick.ReadValue();

            targetMove += new Vector3(
                stick.y,
                0,
                -stick.x
            );
        }

        targetMove = targetMove.normalized * moveSpeed;

        // Smoothly move toward target direction
        currentMove = Vector3.SmoothDamp(
            currentMove,
            targetMove,
            ref moveVelocity,
            smoothTime
        );

        transform.position += currentMove * Time.deltaTime;

        // Smoothly rotate toward movement direction
        if (currentMove.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentMove);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
    }
}