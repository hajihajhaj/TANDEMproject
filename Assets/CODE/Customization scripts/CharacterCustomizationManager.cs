using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    [Header("Female Body")]
    public GameObject femaleBody;

    [Header("Female Clothing")]
    public GameObject[] femaleShirts;
    public GameObject[] femalePants;
    public GameObject[] femaleShoes;
    public GameObject[] femaleHair;

    [Header("Accessories")]
    public GameObject glasses;
    public GameObject beard;

    private bool isDone = false;

    private int currentShirt = 0;
    private int currentPants = 0;
    private int currentShoes = 0;
    private int currentHair = 0;

    void Start()
    {
        Debug.Log("Character Customization Started");

        if (femaleBody != null)
            femaleBody.SetActive(true);

        RefreshCharacter();

        if (glasses != null)
            glasses.SetActive(false);

        if (beard != null)
            beard.SetActive(false);
    }

    //=========================================
    // Helper Functions
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

    //=========================================
    // Shirts
    //=========================================

    public void NextShirt()
    {
        Debug.Log("Next Shirt Clicked");

        isDone = false;

        currentShirt = NextIndex(currentShirt, femaleShirts.Length);

        RefreshCharacter();
    }

    public void PreviousShirt()
    {
        isDone = false;

        currentShirt = PreviousIndex(currentShirt, femaleShirts.Length);

        RefreshCharacter();
    }

    //=========================================
    // Pants
    //=========================================

    public void NextPants()
    {
        isDone = false;

        currentPants = NextIndex(currentPants, femalePants.Length);

        RefreshCharacter();
    }

    public void PreviousPants()
    {
        isDone = false;

        currentPants = PreviousIndex(currentPants, femalePants.Length);

        RefreshCharacter();
    }

    //=========================================
    // Shoes
    //=========================================

    public void NextShoes()
    {
        isDone = false;

        currentShoes = NextIndex(currentShoes, femaleShoes.Length);

        RefreshCharacter();
    }

    public void PreviousShoes()
    {
        isDone = false;

        currentShoes = PreviousIndex(currentShoes, femaleShoes.Length);

        RefreshCharacter();
    }

    //=========================================
    // Hair
    //=========================================

    public void NextHair()
    {
        isDone = false;

        currentHair = NextIndex(currentHair, femaleHair.Length);

        RefreshCharacter();
    }

    public void PreviousHair()
    {
        isDone = false;

        currentHair = PreviousIndex(currentHair, femaleHair.Length);

        RefreshCharacter();
    }

    //=========================================
    // Accessories
    //=========================================

    public void ToggleGlasses()
    {
        isDone = false;

        if (glasses != null)
            glasses.SetActive(!glasses.activeSelf);
    }

    public void ToggleBeard()
    {
        isDone = false;

        if (beard != null)
            beard.SetActive(!beard.activeSelf);
    }

    //=========================================
    // Refresh Character
    //=========================================

    void RefreshCharacter()
    {
        Debug.Log("Refreshing Female Character");

        SetOnlyActive(femaleShirts, currentShirt);
        SetOnlyActive(femalePants, currentPants);
        SetOnlyActive(femaleShoes, currentShoes);
        SetOnlyActive(femaleHair, currentHair);
    }

    //=========================================
    // Done
    //=========================================

    public void SaveCustomization()
    {
        isDone = true;

        Debug.Log(gameObject.name + " customization saved.");
    }

    public bool IsDone()
    {
        return isDone;
    }

    public void ResetDone()
    {
        isDone = false;
    }
}