using UnityEngine;

public class WheelSpin : MonoBehaviour
{
    public Rigidbody rb;

    public Transform frontWheel;
    public Transform backWheel;

    public float wheelRadius = 0.35f;

    // ADDED (tuning control)
    public float spinMultiplier = 0.25f;

    void Update()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float speed = Vector3.Dot(flatVel, transform.forward);
        speed = Mathf.Abs(speed);

        float rotationAmount =
            (speed / (2f * Mathf.PI * wheelRadius))
            * 360f
            * Time.deltaTime
            * spinMultiplier;

        frontWheel.Rotate(Vector3.forward, rotationAmount);
        backWheel.Rotate(Vector3.forward, rotationAmount);
    }
}