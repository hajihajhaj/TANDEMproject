using UnityEngine;
using System.Collections.Generic;

public class IntersectionController : MonoBehaviour
{
    Queue<TrafficCar> waitingCars =
        new Queue<TrafficCar>();

    TrafficCar currentCar;

    public void RegisterCar(TrafficCar car)
    {
        if (!waitingCars.Contains(car))
        {
            waitingCars.Enqueue(car);
        }
    }

    void Update()
    {
        if (currentCar == null &&
            waitingCars.Count > 0)
        {
            currentCar =
                waitingCars.Dequeue();

            StartCoroutine(
                currentCar.WaitThenGo(3f, this)
            );
        }
    }

    public void ReleaseCar()
    {
        currentCar = null;
    }
}