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

    [Header("Camera")]
    public Transform cameraTransform;

    private Gamepad p2;

    private bool isCharging;
    private float currentThrowForce;

    void Update()
    {
        // Get Player 2 controller
        if (p2 == null && Gamepad.all.Count > 1)
            p2 = Gamepad.all[1];

        if (p2 == null && Keyboard.current == null)
            return;

        HandleThrowInput();
        DrawPredictionLine();
    }

    void HandleThrowInput()
    {
        // Controller
        bool controllerPressed =
            p2 != null && p2.rightTrigger.wasPressedThisFrame;

        bool controllerHeld =
            p2 != null && p2.rightTrigger.isPressed;

        bool controllerReleased =
            p2 != null && p2.rightTrigger.wasReleasedThisFrame;

        // Keyboard
        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame;

        bool keyboardHeld =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.isPressed;

        bool keyboardReleased =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasReleasedThisFrame;

        bool pressed =
            controllerPressed || keyboardPressed;

        bool held =
            controllerHeld || keyboardHeld;

        bool released =
            controllerReleased || keyboardReleased;

        // Start charging
        if (pressed && !isCharging)
        {
            isCharging = true;
            currentThrowForce = minThrowForce;

            Debug.Log("P2 THROW START");
        }

        // Charge while holding
        if (isCharging && held)
        {
            currentThrowForce +=
                chargeSpeed * Time.deltaTime;

            currentThrowForce = Mathf.Clamp(
                currentThrowForce,
                minThrowForce,
                maxThrowForce
            );
        }

        // Throw when released
        if (isCharging && released)
        {
            isCharging = false;

            Debug.Log(
                "P2 THROW RELEASE - FORCE: " +
                currentThrowForce
            );

            ThrowBox();

            if (aimLine != null)
                aimLine.enabled = false;
        }
    }

    void ThrowBox()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("Box Prefab is missing!");
            return;
        }

        if (throwPoint == null)
        {
            Debug.LogError("Throw Point is missing!");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is missing!");
            return;
        }

        GameObject box = Instantiate(
            boxPrefab,
            throwPoint.position,
            Quaternion.identity
        );

        Destroy(box, 15f);

        Rigidbody rb = box.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 throwDirection =
                cameraTransform.forward;

            throwDirection.y = 0f;
            throwDirection.Normalize();

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
            if (aimLine != null)
                aimLine.enabled = false;

            return;
        }

        if (
            aimLine == null ||
            throwPoint == null ||
            cameraTransform == null
        )
            return;

        aimLine.enabled = true;
        aimLine.positionCount = linePoints;

        Vector3 startPosition =
            throwPoint.position;

        Vector3 throwDirection =
            cameraTransform.forward;

        throwDirection.y = 0f;
        throwDirection.Normalize();

        Vector3 startVelocity =
            (throwDirection * currentThrowForce) +
            (Vector3.up * upwardForce);

        for (int i = 0; i < linePoints; i++)
        {
            float time =
                i * timeBetweenPoints;

            Vector3 point =
                startPosition +
                (startVelocity * time) +
                (0.5f * Physics.gravity * time * time);

            aimLine.SetPosition(i, point);
        }
    }
}