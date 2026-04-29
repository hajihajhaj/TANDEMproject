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

    bool p1ExpectQ = true;
    bool p2ExpectU = true;

    float p1Multiplier = 1.5f;
    float p2Multiplier = 0.5f;

    public TextMeshProUGUI speedText;

    // track who pedaled recently
    float p1Timer = 0f;
    float p2Timer = 0f;
    float activeWindow = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.linearDamping = 0.3f;
        rb.angularDamping = 5f;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // TURN
        turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;

        bool p1Pedaled = false;
        bool p2Pedaled = false;

        // PLAYER 1
        if (p1ExpectQ && Input.GetKeyDown(KeyCode.Q))
        {
            p1ExpectQ = false;
            p1Pedaled = true;
        }
        else if (!p1ExpectQ && Input.GetKeyDown(KeyCode.E))
        {
            p1ExpectQ = true;
            p1Pedaled = true;
        }

        // PLAYER 2
        if (p2ExpectU && Input.GetKeyDown(KeyCode.U))
        {
            p2ExpectU = false;
            p2Pedaled = true;
        }
        else if (!p2ExpectU && Input.GetKeyDown(KeyCode.O))
        {
            p2ExpectU = true;
            p2Pedaled = true;
        }

        // APPLY FORCE
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

            // BONUS if both hit close together
            if (p1Pedaled && p2Pedaled)
            {
                totalForce *= 1.5f;
            }

            rb.AddForce(transform.forward * totalForce, ForceMode.Impulse);
        }

        // SPEED UI
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = flatVel.magnitude;

        if (speedText != null)
        {
            speedText.text = "Speed: " + speed.ToString("F1");
        }
    }

    void FixedUpdate()
    {
        // decay activity timers
        if (p1Timer > 0) p1Timer -= Time.fixedDeltaTime;
        if (p2Timer > 0) p2Timer -= Time.fixedDeltaTime;

        Vector3 flat = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // DETERMINE CURRENT MAX SPEED
        float currentMaxSpeed;

        bool p1Active = p1Timer > 0;
        bool p2Active = p2Timer > 0;

        if (p1Active && p2Active)
        {
            currentMaxSpeed = bothMaxSpeed;
        }
        else if (p1Active)
        {
            currentMaxSpeed = p1MaxSpeed;
        }
        else if (p2Active)
        {
            currentMaxSpeed = p2MaxSpeed;
        }
        else
        {
            currentMaxSpeed = p2MaxSpeed; // fallback (slow coast)
        }

        // APPLY SPEED CAP
        if (flat.magnitude > currentMaxSpeed)
        {
            Vector3 limited = flat.normalized * currentMaxSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }

        // TURN
        rb.MoveRotation(
            rb.rotation * Quaternion.Euler(0, turnInput * turnSpeed * Time.fixedDeltaTime, 0)
        );
    }
}