
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [Header("Tabs")]
    public Image[] tabImages;
    public GameObject[] pages;

    [Header("Player Controls")]
    public GameObject playerControlsCanvas;

    [Header("Tab Colors")]
    public Color selectedColor = Color.white;
    public Color unselectedColor = Color.gray;

    public int CurrentTab { get; private set; } = 0;

    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    {
        if (pages == null || pages.Length == 0)
            return;

        if (tabNo < 0 || tabNo >= pages.Length)
            return;

        CurrentTab = tabNo;

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(false);

            if (tabImages != null &&
                i < tabImages.Length &&
                tabImages[i] != null)
            {
                tabImages[i].color =
                    i == tabNo ? selectedColor : unselectedColor;
            }
        }

        pages[tabNo].SetActive(true);

        if (playerControlsCanvas != null)
            playerControlsCanvas.SetActive(tabNo == 0);
    }

    public void NextTab()
    {
        if (pages == null || pages.Length == 0)
            return;

        ActivateTab((CurrentTab + 1) % pages.Length);
    }

    public void PreviousTab()
    {
        if (pages == null || pages.Length == 0)
            return;

        ActivateTab(
            (CurrentTab - 1 + pages.Length) % pages.Length
        );
    }
}
