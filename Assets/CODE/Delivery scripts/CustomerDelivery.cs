using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CustomerDelivery
{
    public string customerName;

    [Header("Customer Visuals")]
    public Sprite customerImage;

    [Header("Persistent UI")]
    public Image persistentImageUI;

    [Header("Delivery")]
    public DeliveryHouse targetHouse;

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

    // prevents repeated audio/message spam
    [HideInInspector] public bool hurryShown;
}

