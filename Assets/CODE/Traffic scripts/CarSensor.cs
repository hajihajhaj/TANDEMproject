using UnityEngine;

public class CarSensor : MonoBehaviour
{
    public TrafficCar car;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TrafficCar"))
        {
            car.SetBlocked(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TrafficCar"))
        {
            car.SetBlocked(false);
        }
    }
}