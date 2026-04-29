using UnityEngine;

public class BikeBell : MonoBehaviour
{
    public float bellRadius = 5f;
    public LayerMask npcLayer;
    public AudioSource bellSound;

    void Update()
    {
        if (Input.GetButtonDown("P1_R2"))
        {
            RingBell();
        }
    }

    void RingBell()
    {
        if (bellSound != null)
            bellSound.Play();

    
    }
}