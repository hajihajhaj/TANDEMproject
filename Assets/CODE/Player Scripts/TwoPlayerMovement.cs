using UnityEngine;
using UnityEngine.InputSystem;

public class TwoPlayerMovement : MonoBehaviour
{
    [Header("Player Transforms")]
    public Transform player1;
    public Transform player2;

    [Header("Settings")]
    public float moveSpeed = 5f;

    void Update()
    {
        MovePlayer1();
        MovePlayer2();
    }

    void MovePlayer1()
    {
        if (player1 == null) return;

        Vector3 move = Vector3.zero;

        // WASD (correct mapping)
        if (Keyboard.current.aKey.isPressed) move += Vector3.forward;
        if (Keyboard.current.dKey.isPressed) move += Vector3.back;
        if (Keyboard.current.sKey.isPressed) move += Vector3.left;
        if (Keyboard.current.wKey.isPressed) move += Vector3.right;

        player1.position += move.normalized * moveSpeed * Time.deltaTime;
    }

    void MovePlayer2()
    {
        if (player2 == null) return;

        Vector3 move = Vector3.zero;

        // YGHJ controls (correct mapping)
        if (Keyboard.current.gKey.isPressed) move += Vector3.forward;
        if (Keyboard.current.jKey.isPressed) move += Vector3.back;
        if (Keyboard.current.hKey.isPressed) move += Vector3.left;
        if (Keyboard.current.yKey.isPressed) move += Vector3.right;

        player2.position += move.normalized * moveSpeed * Time.deltaTime;
    }
}