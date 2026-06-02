using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficCar : MonoBehaviour
{
    public RoadNode startingNode;

    public float speed = 6f;
    public float turnSpeed = 5f;
    public float nodeReachDistance = 1f;

    RoadNode currentNode;
    RoadNode targetNode;
    RoadNode previousNode;

    bool blocked;
    bool waitingAtStop;

    float speedMultiplier = 1f;

    void Start()
    {
        currentNode = startingNode;

        ChooseNextNode();
    }

    void Update()
    {
        if (targetNode == null)
            return;

        Vector3 direction =
            targetNode.transform.position -
            transform.position;

        direction.y = 0;

        if (direction.magnitude < nodeReachDistance)
        {
            currentNode = targetNode;

            if (currentNode.isStopNode)
            {
                StartCoroutine(StopAtNode());
            }

            ChooseNextNode();

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

        if (!blocked &&
            !waitingAtStop)
        {
            transform.position +=
                transform.forward *
                speed *
                speedMultiplier *
                Time.deltaTime;
        }
    }

    void ChooseNextNode()
    {
        if (currentNode.connectedNodes.Count == 0)
            return;

        List<RoadNode> possibleNodes =
            new List<RoadNode>();

        foreach (RoadNode node in currentNode.connectedNodes)
        {
            if (node != previousNode)
            {
                possibleNodes.Add(node);
            }
        }

        if (possibleNodes.Count == 0)
        {
            possibleNodes.AddRange(
                currentNode.connectedNodes
            );
        }

        previousNode = currentNode;

        targetNode =
            possibleNodes[
                Random.Range(
                    0,
                    possibleNodes.Count
                )
            ];
    }

    public IEnumerator WaitThenGo(
        float waitTime,
        IntersectionController intersection)
    {
        waitingAtStop = true;

        yield return new WaitForSeconds(waitTime);

        waitingAtStop = false;

        intersection.ReleaseCar();
    }

    IEnumerator StopAtNode()
    {
        waitingAtStop = true;

        yield return new WaitForSeconds(3f);

        waitingAtStop = false;
    }

    public void SetBlocked(bool value)
    {
        blocked = value;
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }
}