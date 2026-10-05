using UnityEngine;
using TMPro;

public class RoomCustomization : MonoBehaviour
{
    [Header("Room UI Options")]
    public GameObject[] roomOptions;

    [Header("Actual Room Models")]
    public GameObject[] roomModels;

    [Header("Select Button Text")]
    public TMP_Text selectButtonText;

    private int currentRoom = 0;
    private int selectedRoom = 0;

    void Start()
    {
        // Always start with Room 1
        currentRoom = 0;
        selectedRoom = 0;

        // Turn every room OFF
        for (int i = 0; i < roomModels.Length; i++)
        {
            if (roomModels[i] != null)
            {
                roomModels[i].SetActive(false);
            }
        }

        // Turn Room 1 ON
        if (roomModels.Length > 0 && roomModels[0] != null)
        {
            roomModels[0].SetActive(true);
        }

        ShowRoom();
        UpdateSelectButton();
    }

    public void NextRoom()
    {
        currentRoom++;

        if (currentRoom >= roomOptions.Length)
            currentRoom = 0;

        ShowRoom();
        UpdateSelectButton();
    }

    public void PreviousRoom()
    {
        currentRoom--;

        if (currentRoom < 0)
            currentRoom = roomOptions.Length - 1;

        ShowRoom();
        UpdateSelectButton();
    }

    void ShowRoom()
    {
        for (int i = 0; i < roomOptions.Length; i++)
        {
            if (roomOptions[i] != null)
            {
                roomOptions[i].SetActive(i == currentRoom);
            }
        }
    }

    public void SelectRoom()
    {
        selectedRoom = currentRoom;

        // Turn all rooms OFF
        for (int i = 0; i < roomModels.Length; i++)
        {
            if (roomModels[i] != null)
            {
                roomModels[i].SetActive(false);
            }
        }

        // Turn selected room ON
        if (selectedRoom >= 0 && selectedRoom < roomModels.Length)
        {
            if (roomModels[selectedRoom] != null)
            {
                roomModels[selectedRoom].SetActive(true);
            }
        }

        UpdateSelectButton();

        Debug.Log("Selected Room: " + selectedRoom);
    }

    void UpdateSelectButton()
    {
        if (selectButtonText != null)
        {
            if (currentRoom == selectedRoom)
                selectButtonText.text = "Selected";
            else
                selectButtonText.text = "Select";
        }
    }
}