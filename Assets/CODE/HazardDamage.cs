using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        BikeHealth bike = collision.gameObject.GetComponent<BikeHealth>();

        if (bike != null)
        {
            bike.TakeDamage(damageAmount);
        }
    }
}