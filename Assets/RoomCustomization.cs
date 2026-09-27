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
        // Load saved room
        selectedRoom = PlayerPrefs.GetInt("SelectedRoom", 0);
        currentRoom = selectedRoom;

        // Show the correct UI preview
        ShowRoom();

        // Show the saved actual room
        ApplySelectedRoom();

        // Update Select button
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
        // Change the room picture/preview in the UI
        for (int i = 0; i < roomOptions.Length; i++)
        {
            roomOptions[i].SetActive(i == currentRoom);
        }
    }

    public void SelectRoom()
    {
        // Save the room the player is currently looking at
        selectedRoom = currentRoom;

        PlayerPrefs.SetInt("SelectedRoom", selectedRoom);
        PlayerPrefs.Save();

        // Change the actual room
        ApplySelectedRoom();

        // Update button
        UpdateSelectButton();

        Debug.Log("Room saved! Selected room: " + selectedRoom);
    }

    void ApplySelectedRoom()
    {
        for (int i = 0; i < roomModels.Length; i++)
        {
            if (roomModels[i] == null)
                continue;

            // Get every Renderer inside this room
            Renderer[] renderers = roomModels[i].GetComponentsInChildren<Renderer>(true);

            // Show selected room, hide the others
            bool shouldShow = (i == selectedRoom);

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = shouldShow;
            }
        }
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