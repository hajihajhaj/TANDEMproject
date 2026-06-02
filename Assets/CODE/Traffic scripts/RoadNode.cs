using System.Collections.Generic;
using UnityEngine;

public class RoadNode : MonoBehaviour
{
    public List<RoadNode> connectedNodes =
        new List<RoadNode>();

    public bool isIntersection;

    public bool isStopNode;
    public float stopTime = 3f;

    public bool isSlowdownNode;
    public float slowdownMultiplier = 0.7f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (RoadNode node in connectedNodes)
        {
            if (node != null)
            {
                Gizmos.DrawLine(
                    transform.position,
                    node.transform.position
                );
            }
        }
    }
}