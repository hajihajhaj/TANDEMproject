using TMPro;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    [Header("Female Body")]
    public GameObject femaleBody;

    [Header("CUSTOMIZATION PREVIEW - Female Clothing")]
    public GameObject[] femaleShirts;
    public GameObject[] femalePants;
    public GameObject[] femaleShoes;
    public GameObject[] femaleHair;

    [Header("CUSTOMIZATION PREVIEW - Accessories")]
    public GameObject glasses;
    public GameObject beard;

    [Header("ACTUAL PLAYER - Female Clothing")]
    public GameObject[] playerFemaleShirts;
    public GameObject[] playerFemalePants;
    public GameObject[] playerFemaleShoes;
    public GameObject[] playerFemaleHair;

    [Header("ACTUAL PLAYER - Accessories")]
    public GameObject playerGlasses;
    public GameObject playerBeard;

    [Header("Done Button")]
    public TMP_Text doneButtonText;

    private bool isDone = false;

    private int currentShirt = 0;
    private int currentPants = 0;
    private int currentShoes = 0;
    private int currentHair = 0;

    private bool glassesSelected = false;
    private bool beardSelected = false;


    //=========================================
    // START
    //=========================================

    void Start()
    {
        Debug.Log("Character Customization Started");

        if (femaleBody != null)
            femaleBody.SetActive(true);

        // Set up customization preview
        RefreshCharacter();

        if (glasses != null)
            glasses.SetActive(false);

        if (beard != null)
            beard.SetActive(false);

        // Hide actual player outfit options
        HideAll(playerFemaleShirts);
        HideAll(playerFemalePants);
        HideAll(playerFemaleShoes);
        HideAll(playerFemaleHair);

        if (playerGlasses != null)
            playerGlasses.SetActive(false);

        if (playerBeard != null)
            playerBeard.SetActive(false);

        if (doneButtonText != null)
            doneButtonText.text = "DONE";
    }


    //=========================================
    // HELPER FUNCTIONS
    //=========================================

    void SetOnlyActive(GameObject[] objects, int index)
    {
        if (objects == null || objects.Length == 0)
            return;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
                objects[i].SetActive(i == index);
        }
    }


    void HideAll(GameObject[] objects)
    {
        if (objects == null)
            return;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
                objects[i].SetActive(false);
        }
    }


    int NextIndex(int current, int length)
    {
        if (length == 0)
            return 0;

        current++;

        if (current >= length)
            current = 0;

        return current;
    }


    int PreviousIndex(int current, int length)
    {
        if (length == 0)
            return 0;

        current--;

        if (current < 0)
            current = length - 1;

        return current;
    }


    void MarkAsUnsaved()
    {
        isDone = false;

        if (doneButtonText != null)
            doneButtonText.text = "DONE";
    }


    //=========================================
    // SHIRTS
    //=========================================

    public void NextShirt()
    {
        Debug.Log("Next Shirt Clicked");

        MarkAsUnsaved();

        currentShirt = NextIndex(
            currentShirt,
            femaleShirts.Length
        );

        RefreshCharacter();
    }


    public void PreviousShirt()
    {
        MarkAsUnsaved();

        currentShirt = PreviousIndex(
            currentShirt,
            femaleShirts.Length
        );

        RefreshCharacter();
    }


    //=========================================
    // PANTS
    //=========================================

    public void NextPants()
    {
        MarkAsUnsaved();

        currentPants = NextIndex(
            currentPants,
            femalePants.Length
        );

        RefreshCharacter();
    }


    public void PreviousPants()
    {
        MarkAsUnsaved();

        currentPants = PreviousIndex(
            currentPants,
            femalePants.Length
        );

        RefreshCharacter();
    }


    //=========================================
    // SHOES
    //=========================================

    public void NextShoes()
    {
        MarkAsUnsaved();

        currentShoes = NextIndex(
            currentShoes,
            femaleShoes.Length
        );

        RefreshCharacter();
    }


    public void PreviousShoes()
    {
        MarkAsUnsaved();

        currentShoes = PreviousIndex(
            currentShoes,
            femaleShoes.Length
        );

        RefreshCharacter();
    }


    //=========================================
    // HAIR
    //=========================================

    public void NextHair()
    {
        MarkAsUnsaved();

        currentHair = NextIndex(
            currentHair,
            femaleHair.Length
        );

        RefreshCharacter();
    }


    public void PreviousHair()
    {
        MarkAsUnsaved();

        currentHair = PreviousIndex(
            currentHair,
            femaleHair.Length
        );

        RefreshCharacter();
    }


    //=========================================
    // ACCESSORIES
    //=========================================

    public void ToggleGlasses()
    {
        MarkAsUnsaved();

        glassesSelected = !glassesSelected;

        if (glasses != null)
            glasses.SetActive(glassesSelected);
    }


    public void ToggleBeard()
    {
        MarkAsUnsaved();

        beardSelected = !beardSelected;

        if (beard != null)
            beard.SetActive(beardSelected);
    }


    //=========================================
    // REFRESH CUSTOMIZATION PREVIEW
    //=========================================

    void RefreshCharacter()
    {
        Debug.Log("Refreshing Female Character");

        SetOnlyActive(
            femaleShirts,
            currentShirt
        );

        SetOnlyActive(
            femalePants,
            currentPants
        );

        SetOnlyActive(
            femaleShoes,
            currentShoes
        );

        SetOnlyActive(
            femaleHair,
            currentHair
        );
    }


    //=========================================
    // APPLY TO ACTUAL PLAYER
    //=========================================

    void ApplyCustomizationToPlayer()
    {
        Debug.Log("Applying customization to actual player: " + gameObject.name);

        // Shirt
        SetOnlyActive(
            playerFemaleShirts,
            currentShirt
        );

        // Pants
        SetOnlyActive(
            playerFemalePants,
            currentPants
        );

        // Shoes
        SetOnlyActive(
            playerFemaleShoes,
            currentShoes
        );

        // Hair
        SetOnlyActive(
            playerFemaleHair,
            currentHair
        );

        // Glasses
        if (playerGlasses != null)
            playerGlasses.SetActive(glassesSelected);

        // Beard
        if (playerBeard != null)
            playerBeard.SetActive(beardSelected);

        Debug.Log("Player outfit applied!");
    }


    //=========================================
    // DONE / SAVE
    //=========================================

    public void SaveCustomization()
    {
        isDone = true;

        // Apply the selected outfit to the actual character
        ApplyCustomizationToPlayer();

        // Change DONE -> SAVED
        if (doneButtonText != null)
            doneButtonText.text = "SAVED";

        Debug.Log(gameObject.name + " customization saved.");
    }


    //=========================================
    // STATUS
    //=========================================

    public bool IsDone()
    {
        return isDone;
    }


    public void ResetDone()
    {
        isDone = false;

        if (doneButtonText != null)
            doneButtonText.text = "DONE";
    }
}