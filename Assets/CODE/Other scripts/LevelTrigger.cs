using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public Camera levelSelectCamera;
    public Camera roomCamera;

    public Transform bikeSpawnPoint;
    public GameObject bike;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        roomCamera.gameObject.SetActive(false);
        levelSelectCamera.gameObject.SetActive(true);

        if (bike != null && bikeSpawnPoint != null)
        {
            bike.transform.position = bikeSpawnPoint.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        levelSelectCamera.gameObject.SetActive(false);
        roomCamera.gameObject.SetActive(true);
    }
}