using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BikeMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float basePedalForce = 40f;
    public float turnSpeed = 120f;

    // SPEED CAPS
    public float p1MaxSpeed = 12f;
    public float p2MaxSpeed = 8f;
    public float bothMaxSpeed = 18f;

    float turnInput;

    // PEDAL STATES
    bool p1ExpectL = true;
    bool p2ExpectL = true;

    float p1Multiplier = 1.5f;
    float p2Multiplier = 0.5f;

    public TextMeshProUGUI speedText;

    float p1Timer = 0f;
    float p2Timer = 0f;
    float activeWindow = 0.5f;

    // BRAKE
    public float brakeStrength = 6f;
    public float brakeDrag = 4f;

    // COASTING
    public float coastDrag = 0.1f;
    public float coastSlowdown = 0.5f;

    Gamepad p1;
    Gamepad p2;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.linearDamping = coastDrag;
        rb.angularDamping = 5f;

        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // PLAYER CONTROLLERS
        p1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        p2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;

        // ------------------------
        // PLAYER 1 TURN
        // ------------------------

        float keyboardTurn = 0f;

        if (Input.GetKey(KeyCode.A)) keyboardTurn = -1f;
        if (Input.GetKey(KeyCode.D)) keyboardTurn = 1f;

        float stickTurn = 0f;

        if (p1 != null)
        {
            stickTurn = p1.leftStick.x.ReadValue();
        }

        turnInput = keyboardTurn;

        if (Mathf.Abs(stickTurn) > 0.2f)
        {
            turnInput = stickTurn;
        }

        turnInput = Mathf.Clamp(turnInput, -1f, 1f);

        bool p1Pedaled = false;
        bool p2Pedaled = false;

        // ------------------------
        // PLAYER 1 PEDAL
        // ------------------------

        if (p1ExpectL && Input.GetKeyDown(KeyCode.Q))
        {
            p1ExpectL = false;
            p1Pedaled = true;
        }
        else if (!p1ExpectL && Input.GetKeyDown(KeyCode.E))
        {
            p1ExpectL = true;
            p1Pedaled = true;
        }

        if (p1 != null)
        {
            if (p1ExpectL && p1.leftShoulder.wasPressedThisFrame)
            {
                p1ExpectL = false;
                p1Pedaled = true;
            }
            else if (!p1ExpectL && p1.rightShoulder.wasPressedThisFrame)
            {
                p1ExpectL = true;
                p1Pedaled = true;
            }
        }

        // ------------------------
        // PLAYER 2 PEDAL
        // ------------------------

        if (p2ExpectL && Input.GetKeyDown(KeyCode.U))
        {
            p2ExpectL = false;
            p2Pedaled = true;
        }
        else if (!p2ExpectL && Input.GetKeyDown(KeyCode.O))
        {
            p2ExpectL = true;
            p2Pedaled = true;
        }

        if (p2 != null)
        {
            if (p2ExpectL && p2.leftShoulder.wasPressedThisFrame)
            {
                p2ExpectL = false;
                p2Pedaled = true;
            }
            else if (!p2ExpectL && p2.rightShoulder.wasPressedThisFrame)
            {
                p2ExpectL = true;
                p2Pedaled = true;
            }
        }

        // ------------------------
        // APPLY FORCE
        // ------------------------

        if (p1Pedaled || p2Pedaled)
        {
            float totalForce = 0f;

            if (p1Pedaled)
            {
                totalForce += basePedalForce * p1Multiplier;
                p1Timer = activeWindow;
            }

            if (p2Pedaled)
            {
                totalForce += basePedalForce * p2Multiplier;
                p2Timer = activeWindow;
            }

            // BONUS IF BOTH PEDAL SAME FRAME
            if (p1Pedaled && p2Pedaled)
            {
                totalForce *= 1.5f;
            }

            rb.AddForce(transform.forward * totalForce, ForceMode.Impulse);
        }

        // ------------------------
        // SPEED UI
        // ------------------------

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = flatVel.magnitude;

        if (speedText != null)
        {
            speedText.text = "Speed: " + speed.ToString("F1");
        }
    }

    void FixedUpdate()
    {
        // DECAY TIMERS
        if (p1Timer > 0) p1Timer -= Time.fixedDeltaTime;
        if (p2Timer > 0) p2Timer -= Time.fixedDeltaTime;

        bool p1Active = p1Timer > 0;
        bool p2Active = p2Timer > 0;

        float currentMaxSpeed;

        if (p1Active && p2Active)
            currentMaxSpeed = bothMaxSpeed;
        else if (p1Active)
            currentMaxSpeed = p1MaxSpeed;
        else if (p2Active)
            currentMaxSpeed = p2MaxSpeed;
        else
            currentMaxSpeed = bothMaxSpeed;

        // ------------------------
        // BRAKE
        // ------------------------

        bool braking = Input.GetKey(KeyCode.S);

        if (p1 != null && p1.leftTrigger.ReadValue() > 0.5f)
        {
            braking = true;
        }

        Vector3 flat = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (braking)
        {
            Vector3 slowed = Vector3.Lerp(
                flat,
                Vector3.zero,
                brakeStrength * Time.fixedDeltaTime
            );

            rb.linearVelocity = new Vector3(
                slowed.x,
                rb.linearVelocity.y,
                slowed.z
            );

            rb.linearDamping = brakeDrag;
        }
        else if (!p1Active && !p2Active)
        {
            Vector3 slowed = Vector3.Lerp(
                flat,
                Vector3.zero,
                coastSlowdown * Time.fixedDeltaTime
            );

            rb.linearVelocity = new Vector3(
                slowed.x,
                rb.linearVelocity.y,
                slowed.z
            );

            rb.linearDamping = coastDrag;
        }
        else
        {
            rb.linearDamping = coastDrag;
        }

        // RECALCULATE AFTER CHANGES
        flat = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // SPEED CAP
        if (flat.magnitude > currentMaxSpeed)
        {
            Vector3 limited = flat.normalized * currentMaxSpeed;

            rb.linearVelocity = new Vector3(
                limited.x,
                rb.linearVelocity.y,
                limited.z
            );
        }

        // TURN
        rb.MoveRotation(
            rb.rotation * Quaternion.Euler(
                0,
                turnInput * turnSpeed * Time.fixedDeltaTime,
                0
            )
        );
    }
}