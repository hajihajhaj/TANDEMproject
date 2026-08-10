using UnityEngine;
using UnityEngine.InputSystem;

public class TwoPlayerMovement : MonoBehaviour
{
    [Header("Player Transforms")]
    public Transform player1;
    public Transform player2;

    [Header("Animators")]
    public Animator player1Animator;
    public Animator player2Animator;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public bool canMove = true;

    void Update()
    {
        if (!canMove)
        {
            if (player1Animator != null)
                player1Animator.SetFloat("Speed", 0f);

            if (player2Animator != null)
                player2Animator.SetFloat("Speed", 0f);

            return;
        }

        MovePlayer1();
        MovePlayer2();
    }

    void MovePlayer1()
    {
        if (player1 == null) return;

        Vector3 move = Vector3.zero;

        // Keyboard WASD
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
                move += Vector3.forward;

            if (Keyboard.current.dKey.isPressed)
                move += Vector3.back;

            if (Keyboard.current.sKey.isPressed)
                move += Vector3.left;

            if (Keyboard.current.wKey.isPressed)
                move += Vector3.right;
        }

        // Controller 1 left stick
        if (Gamepad.all.Count > 0)
        {
            Vector2 stick = Gamepad.all[0].leftStick.ReadValue();

            move += new Vector3(
                stick.y,
                0f,
                -stick.x
            );
        }

        // Move Player 1
        if (move.magnitude > 1f)
            move.Normalize();

        player1.position += move * moveSpeed * Time.deltaTime;

        // Turn Player 1 toward movement
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            player1.rotation = Quaternion.Slerp(
                player1.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Tell Animator how fast Player 1 is moving
        if (player1Animator != null)
        {
            player1Animator.SetFloat("Speed", move.magnitude);
        }
    }

    void MovePlayer2()
    {
        if (player2 == null) return;

        Vector3 move = Vector3.zero;

        // Keyboard YGHJ
        if (Keyboard.current != null)
        {
            if (Keyboard.current.gKey.isPressed)
                move += Vector3.forward;

            if (Keyboard.current.jKey.isPressed)
                move += Vector3.back;

            if (Keyboard.current.hKey.isPressed)
                move += Vector3.left;

            if (Keyboard.current.yKey.isPressed)
                move += Vector3.right;
        }

        // Controller 2 left stick
        if (Gamepad.all.Count > 1)
        {
            Vector2 stick = Gamepad.all[1].leftStick.ReadValue();

            move += new Vector3(
                stick.y,
                0f,
                -stick.x
            );
        }

        // Move Player 2
        if (move.magnitude > 1f)
            move.Normalize();

        player2.position += move * moveSpeed * Time.deltaTime;

        // Turn Player 2 toward movement
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            player2.rotation = Quaternion.Slerp(
                player2.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Tell Animator how fast Player 2 is moving
        if (player2Animator != null)
        {
            player2Animator.SetFloat("Speed", move.magnitude);
        }
    }
}