using UnityEngine;
using TMPro;

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

    float p1Multiplier = 1.5f; // stronger
    float p2Multiplier = 0.5f; // weaker

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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.linearDamping = coastDrag;
        rb.angularDamping = 5f;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // ------------------------
        // ?? PLAYER 1 TURN ONLY
        // ------------------------
        float keyboardTurn = 0f;
        if (Input.GetKey(KeyCode.A)) keyboardTurn = -1f;
        if (Input.GetKey(KeyCode.D)) keyboardTurn = 1f;

        float stickTurn = Input.GetAxis("P1_LeftStick_X");

        turnInput = keyboardTurn;

        if (Mathf.Abs(stickTurn) > 0.2f)
        {
            turnInput = stickTurn;
        }

        turnInput = Mathf.Clamp(turnInput, -1f, 1f);

        bool p1Pedaled = false;
        bool p2Pedaled = false;

        // ------------------------
        // ?? PLAYER 1 PEDAL (Q/E)
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

        // ?? PLAYER 1 PEDAL (L1/R1)
        if (p1ExpectL && Input.GetKeyDown(KeyCode.Joystick1Button4)) // L1
        {
            p1ExpectL = false;
            p1Pedaled = true;
        }
        else if (!p1ExpectL && Input.GetKeyDown(KeyCode.Joystick1Button5)) // R1
        {
            p1ExpectL = true;
            p1Pedaled = true;
        }

        // ------------------------
        // ?? PLAYER 2 PEDAL (U/O)
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

        // ?? PLAYER 2 PEDAL (Controller 2)
        if (p2ExpectL && Input.GetKeyDown(KeyCode.Joystick2Button4)) // L1
        {
            p2ExpectL = false;
            p2Pedaled = true;
        }
        else if (!p2ExpectL && Input.GetKeyDown(KeyCode.Joystick2Button5)) // R1
        {
            p2ExpectL = true;
            p2Pedaled = true;
        }

        // ------------------------
        // ?? APPLY FORCE
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

            // BONUS if both pedal same frame
            if (p1Pedaled && p2Pedaled)
            {
                totalForce *= 1.5f;
            }

            rb.AddForce(transform.forward * totalForce, ForceMode.Impulse);
        }

        // ------------------------
        // ?? SPEED UI
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
        // decay timers
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
        // ?? BRAKE (PLAYER 1 ONLY)
        // ------------------------
        bool braking = Input.GetKey(KeyCode.S);

        if (Input.GetKey(KeyCode.Joystick1Button6)) // L2
        {
            braking = true;
        }

        Vector3 flat = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (braking)
        {
            Vector3 slowed = Vector3.Lerp(flat, Vector3.zero, brakeStrength * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(slowed.x, rb.linearVelocity.y, slowed.z);
            rb.linearDamping = brakeDrag;
        }
        else if (!p1Active && !p2Active)
        {
            Vector3 slowed = Vector3.Lerp(flat, Vector3.zero, coastSlowdown * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(slowed.x, rb.linearVelocity.y, slowed.z);
            rb.linearDamping = coastDrag;
        }
        else
        {
            rb.linearDamping = coastDrag;
        }

        // recalc AFTER changes
        flat = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // SPEED CAP
        if (flat.magnitude > currentMaxSpeed)
        {
            Vector3 limited = flat.normalized * currentMaxSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }

        // TURN (PLAYER 1 ONLY)
        rb.MoveRotation(
            rb.rotation * Quaternion.Euler(0, turnInput * turnSpeed * Time.fixedDeltaTime, 0)
        );
    }
}