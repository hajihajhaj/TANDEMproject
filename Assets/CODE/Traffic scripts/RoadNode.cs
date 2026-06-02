using System.Collections.Generic;
using UnityEngine;

public class RoadNode : MonoBehaviour
{
    public List<RoadNode> connectedNodes =
        new List<RoadNode>();

    public bool isIntersection;
    public bool isStopNode;

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