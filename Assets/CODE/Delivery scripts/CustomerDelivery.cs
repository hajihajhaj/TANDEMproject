using UnityEngine;
using UnityEngine.UI;

public enum DeliveryDifficulty
{
    Easy = 2,
    Medium = 3,
    Hard = 4
}

[System.Serializable]
public class CustomerDelivery
{
    public string customerName;

    [Header("Customer Visuals")]
    public Sprite customerImage;

    [Header("Persistent UI")]
    public GameObject[] persistentImages;

    [Header("Delivery")]
    public DeliveryHouse targetHouse;

    [Header("Difficulty")]
    public DeliveryDifficulty difficulty =
        DeliveryDifficulty.Easy;

    [Header("Timer")]
    public float maxTime = 60f;

    [HideInInspector] public float currentTime;
    [HideInInspector] public bool delivered;
    [HideInInspector] public bool failed;

    [Header("UI")]
    public Slider timerBar;

    [Header("Messages")]
    [TextArea] public string patientMessage;
    [TextArea] public string hurryMessage;
    [TextArea] public string angryMessage;
    [TextArea] public string successMessage;

    [HideInInspector] public bool hurryShown;
}