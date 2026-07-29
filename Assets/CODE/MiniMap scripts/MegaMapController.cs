using UnityEngine;

public class MegaMapController : MonoBehaviour
{
    public GameObject megaMapPanel;

    void Update()
    {
        bool showMap = Input.GetKey(KeyCode.M);

        megaMapPanel.SetActive(showMap);
    }
}