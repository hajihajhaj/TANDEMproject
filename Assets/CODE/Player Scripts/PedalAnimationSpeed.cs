using UnityEngine;

public class PedalAnimationSpeed : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;

    public float speedMultiplier = 0.5f;

    public float minAnimationSpeed = 0.5f;
    public float maxAnimationSpeed = 3f;

    void Update()
    {
        if (animator == null || rb == null)
            return;

        float speed = rb.linearVelocity.magnitude;

        float animSpeed =
            speed * speedMultiplier;

        animSpeed = Mathf.Clamp(
            animSpeed,
            minAnimationSpeed,
            maxAnimationSpeed
        );

        animator.speed = animSpeed;
    }
}