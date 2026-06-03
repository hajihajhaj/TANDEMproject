using UnityEngine;
using System.Collections;

public class AchievementPopupUI : MonoBehaviour
{
    public static AchievementPopupUI Instance;
    public GameObject panel;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(3f);
        panel.SetActive(false);
    }
}