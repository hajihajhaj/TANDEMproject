using UnityEngine;

public class StopTrigger : MonoBehaviour
{
    public IntersectionController intersection;

    private void OnTriggerEnter(Collider other)
    {
        TrafficCar car =
            other.GetComponent<TrafficCar>();

        if (car != null)
        {
            intersection.RegisterCar(car);
        }
    }
}