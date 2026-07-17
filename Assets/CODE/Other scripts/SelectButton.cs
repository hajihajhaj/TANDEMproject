using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectButton : MonoBehaviour
{
    public GameObject buttonToSelect;

    void OnEnable()
    {
        StartCoroutine(SelectNextFrame());
    }

    IEnumerator SelectNextFrame()
    {
        yield return null;

        if (EventSystem.current == null || buttonToSelect == null)
            yield break;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonToSelect);
    }
}