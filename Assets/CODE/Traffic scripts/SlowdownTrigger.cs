using UnityEngine;

public class SlowdownTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TrafficCar car =
            other.GetComponent<TrafficCar>();

        if (car != null)
        {
            car.SetSpeedMultiplier(0.4f);
        }
    }
}