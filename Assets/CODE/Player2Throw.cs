using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Throw : MonoBehaviour
{
    [Header("References")]
    public GameObject boxPrefab;
    public Transform throwPoint;
    public LineRenderer aimLine;

    [Header("Throw Settings")]
    public float minThrowForce = 5f;
    public float maxThrowForce = 25f;
    public float chargeSpeed = 15f;
    public float upwardForce = 3f;

    [Header("Aim")]
    public int linePoints = 30;
    public float timeBetweenPoints = 0.1f;

    private Gamepad p2;

    private bool isCharging;
    private float currentThrowForce;

    void Update()
    {
        // Player 2 = second connected gamepad
        if (p2 == null && Gamepad.all.Count > 1)
            p2 = Gamepad.all[1];

        if (p2 == null && Keyboard.current == null)
            return;

        HandleThrowInput();
        DrawPredictionLine();
    }

    void HandleThrowInput()
    {
        bool controllerPressed = p2 != null && p2.rightTrigger.wasPressedThisFrame;
        bool controllerHeld = p2 != null && p2.rightTrigger.isPressed;
        bool controllerReleased = p2 != null && p2.rightTrigger.wasReleasedThisFrame;

        bool keyboardPressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool keyboardHeld = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
        bool keyboardReleased = Keyboard.current != null && Keyboard.current.spaceKey.wasReleasedThisFrame;

        bool triggerPressed = controllerPressed || keyboardPressed;
        bool triggerHeld = controllerHeld || keyboardHeld;
        bool triggerReleased = controllerReleased || keyboardReleased;

        // Start charging
        if (triggerPressed)
        {
            isCharging = true;
            currentThrowForce = minThrowForce;
        }

        // Charge throw
        if (isCharging && triggerHeld)
        {
            currentThrowForce += chargeSpeed * Time.deltaTime;

            currentThrowForce = Mathf.Clamp(
                currentThrowForce,
                minThrowForce,
                maxThrowForce
            );
        }

        // Throw
        if (isCharging && triggerReleased)
        {
            ThrowBox();

            isCharging = false;
            aimLine.enabled = false;
        }
    }

    void ThrowBox()
    {
        GameObject box = Instantiate(boxPrefab, throwPoint.position, Quaternion.identity);
        Destroy(box, 15f);
        Rigidbody rb = box.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 throwDirection = transform.forward;

            Vector3 force =
                (throwDirection * currentThrowForce) +
                (Vector3.up * upwardForce);

            rb.linearVelocity = force;
        }
    }

    void DrawPredictionLine()
    {
        if (!isCharging)
        {
            aimLine.enabled = false;
            return;
        }

        aimLine.enabled = true;
        aimLine.positionCount = linePoints;

        Vector3 startPosition = throwPoint.position;

        Vector3 startVelocity =
            (transform.forward * currentThrowForce) +
            (Vector3.up * upwardForce);

        for (int i = 0; i < linePoints; i++)
        {
            float time = i * timeBetweenPoints;

            Vector3 point = startPosition +
                            (startVelocity * time) +
                            (0.5f * Physics.gravity * time * time);

            aimLine.SetPosition(i, point);
        }
    }
}