using UnityEngine;

public class NPCCrosswalk : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    [Range(0, 1)]
    public float crossingChance = 0.3f;

    public bool IsSafeToCross()
    {
        // later the car system will control this
        return true;
    }

    public bool ShouldCross()
    {
        return Random.value < crossingChance;
    }
}
