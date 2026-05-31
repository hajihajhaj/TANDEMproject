using UnityEngine;

public class TrafficCar : MonoBehaviour
{
    public Transform[] waypoints;

    public float speed = 6f;
    public float turnSpeed = 5f;

    int currentWaypoint;

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        Transform target =
            waypoints[currentWaypoint];

        Vector3 direction =
            target.position - transform.position;

        direction.y = 0;

        if (direction.magnitude < 1f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }

            return;
        }

        Quaternion lookRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                turnSpeed * Time.deltaTime
            );

        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;
    }
}