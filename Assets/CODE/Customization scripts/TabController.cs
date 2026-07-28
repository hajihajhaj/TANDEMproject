using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    public GameObject playerControlsCanvas;

    public Color selectedColor = Color.white;
    public Color unselectedColor = Color.gray;

    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    {
        if (tabNo < 0 || tabNo >= pages.Length)
            return;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = unselectedColor;
        }

        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = selectedColor;


        // Player tab
        if (tabNo == 0)
        {
            playerControlsCanvas.SetActive(true);
        }
        else
        {
            playerControlsCanvas.SetActive(false);
        }
    }
}