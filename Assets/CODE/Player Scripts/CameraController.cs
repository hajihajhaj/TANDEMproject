using UnityEngine;
using UnityEngine.InputSystem;

public class BikeCameraOrbit : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    public Vector3 offset = new Vector3(0, 2, -6);

    [Header("Rotation")]
    public float rotationSpeed = 70f;
    public float verticalSpeed = 50f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Camera Collision")]
    public float collisionRadius = 0.25f;
    public float collisionBuffer = 0.15f;
    public float collisionSmooth = 12f;
    public LayerMask collisionLayers = ~0;

    [Header("Aim Target")]
    public Transform aimTarget;
    public float aimDistance = 10f;

    [Header("Player Controller")]
    [Tooltip("0 = Player 1 controller, 1 = Player 2 controller")]
    public int playerControllerIndex = 1;

    private float yaw;
    private float pitch = 20f;
    private float currentDistance;

    void Start()
    {
        if (!followTarget)
        {
            Debug.LogError("Follow target not assigned!");
            enabled = false;
            return;
        }

        // START FROM CURRENT CAMERA ROTATION
        Vector3 startRot = transform.eulerAngles;

        yaw = startRot.y;
        pitch = startRot.x;

        currentDistance = offset.magnitude;
    }

    void Update()
    {
        ReadInput();
        UpdateCamera();
    }

    Gamepad GetPlayerGamepad()
    {
        if (Gamepad.all.Count > playerControllerIndex)
        {
            return Gamepad.all[playerControllerIndex];
        }

        return null;
    }

    void ReadInput()
    {
        float inputX = 0f;
        float inputY = 0f;

        // ------------------------
        // PLAYER CONTROLLER
        // ------------------------

        Gamepad gamepad = GetPlayerGamepad();

        if (gamepad != null)
        {
            inputX = gamepad.rightStick.x.ReadValue();
            inputY = gamepad.rightStick.y.ReadValue();
        }

        // ------------------------
        // DEADZONE
        // ------------------------

        if (Mathf.Abs(inputX) < 0.2f) inputX = 0f;
        if (Mathf.Abs(inputY) < 0.2f) inputY = 0f;

        // ------------------------
        // KEYBOARD TESTING
        // ------------------------

        if (inputX == 0f && inputY == 0f)
        {
            if (Input.GetKey(KeyCode.G)) inputX = -1f;
            if (Input.GetKey(KeyCode.J)) inputX = 1f;
            if (Input.GetKey(KeyCode.H)) inputY = 1f;
            if (Input.GetKey(KeyCode.Y)) inputY = -1f;
        }

        // ------------------------
        // APPLY ROTATION
        // ------------------------

        yaw += inputX * rotationSpeed * Time.deltaTime;

        pitch += inputY * verticalSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void UpdateCamera()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPos = followTarget.position + Vector3.up * 1.5f;

        Vector3 desiredOffset = rotation * offset;

        float desiredDistance = offset.magnitude;

        // ------------------------
        // CAMERA COLLISION
        // ------------------------

        RaycastHit hit;

        if (Physics.SphereCast(
            targetPos,
            collisionRadius,
            desiredOffset.normalized,
            out hit,
            desiredDistance,
            collisionLayers,
            QueryTriggerInteraction.Ignore))
        {
            desiredDistance = Mathf.Max(hit.distance - collisionBuffer, 0.5f);
        }

        currentDistance = Mathf.Lerp(
            currentDistance,
            desiredDistance,
            Time.deltaTime * collisionSmooth
        );

        transform.position =
            targetPos +
            desiredOffset.normalized * currentDistance;

        transform.LookAt(targetPos);

        // ------------------------
        // AIM TARGET
        // ------------------------

        if (aimTarget)
        {
            aimTarget.position =
                targetPos +
                rotation * Vector3.forward * aimDistance;
        }
    }
}