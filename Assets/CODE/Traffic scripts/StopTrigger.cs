using UnityEngine;
using System.Collections;

public class StopTrigger : MonoBehaviour
{
    public float stopTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("STOP HIT");
        TrafficCar car =
            other.GetComponent<TrafficCar>();

        if (car != null)
        {
            StartCoroutine(StopCar(car));
        }
    }

    IEnumerator StopCar(TrafficCar car)
    {
        car.SetBlocked(true);

        yield return new WaitForSeconds(stopTime);

        car.SetBlocked(false);
        car.SetSpeedMultiplier(1f);
    }
}