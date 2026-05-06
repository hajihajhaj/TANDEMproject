using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public GameObject promptUI;
    public string triggerType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(true);

            PlayerInteraction interaction =
                other.GetComponent<PlayerInteraction>();

            if (interaction != null)
            {
                interaction.currentTrigger = triggerType;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(false);

            PlayerInteraction interaction =
                other.GetComponent<PlayerInteraction>();

            if (interaction != null)
            {
                interaction.currentTrigger = "";
            }
        }
    }
}