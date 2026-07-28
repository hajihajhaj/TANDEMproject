using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    [Header("Bodies")]
    public GameObject maleBody;
    public GameObject femaleBody;

    [Header("Male Clothing")]
    public GameObject[] maleShirts;
    public GameObject[] malePants;
    public GameObject[] maleShoes;
    public GameObject[] maleHair;

    [Header("Female Clothing")]
    public GameObject[] femaleShirts;
    public GameObject[] femalePants;
    public GameObject[] femaleShoes;
    public GameObject[] femaleHair;

    [Header("Accessories")]
    public GameObject glasses;
    public GameObject beard;

    private bool isFemale = false;
    private bool isDone = false;

    private int currentShirt = 0;
    private int currentPants = 0;
    private int currentShoes = 0;
    private int currentHair = 0;

    void Start()
    {
        Debug.Log("Character Customization Started");

        ApplyGender();
        RefreshCharacter();

        if (glasses != null)
            glasses.SetActive(false);

        if (beard != null)
            beard.SetActive(false);
    }

    //=========================================
    // Gender
    //=========================================

    public void ToggleGender()
    {
        isDone = false;

        isFemale = !isFemale;

        ApplyGender();
        RefreshCharacter();
    }

    void ApplyGender()
    {
        if (maleBody != null)
            maleBody.SetActive(!isFemale);

        if (femaleBody != null)
            femaleBody.SetActive(isFemale);
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

        if (isFemale)
            currentShirt = NextIndex(currentShirt, femaleShirts.Length);
        else
            currentShirt = NextIndex(currentShirt, maleShirts.Length);

        RefreshCharacter();
    }

    public void PreviousShirt()
    {
        isDone = false;

        if (isFemale)
            currentShirt = PreviousIndex(currentShirt, femaleShirts.Length);
        else
            currentShirt = PreviousIndex(currentShirt, maleShirts.Length);

        RefreshCharacter();
    }

    //=========================================
    // Pants
    //=========================================

    public void NextPants()
    {
        isDone = false;

        if (isFemale)
            currentPants = NextIndex(currentPants, femalePants.Length);
        else
            currentPants = NextIndex(currentPants, malePants.Length);

        RefreshCharacter();
    }

    public void PreviousPants()
    {
        isDone = false;

        if (isFemale)
            currentPants = PreviousIndex(currentPants, femalePants.Length);
        else
            currentPants = PreviousIndex(currentPants, malePants.Length);

        RefreshCharacter();
    }

    //=========================================
    // Shoes
    //=========================================

    public void NextShoes()
    {
        isDone = false;

        if (isFemale)
            currentShoes = NextIndex(currentShoes, femaleShoes.Length);
        else
            currentShoes = NextIndex(currentShoes, maleShoes.Length);

        RefreshCharacter();
    }

    public void PreviousShoes()
    {
        isDone = false;

        if (isFemale)
            currentShoes = PreviousIndex(currentShoes, femaleShoes.Length);
        else
            currentShoes = PreviousIndex(currentShoes, maleShoes.Length);

        RefreshCharacter();
    }

    //=========================================
    // Hair
    //=========================================

    public void NextHair()
    {
        isDone = false;

        if (isFemale)
            currentHair = NextIndex(currentHair, femaleHair.Length);
        else
            currentHair = NextIndex(currentHair, maleHair.Length);

        RefreshCharacter();
    }

    public void PreviousHair()
    {
        isDone = false;

        if (isFemale)
            currentHair = PreviousIndex(currentHair, femaleHair.Length);
        else
            currentHair = PreviousIndex(currentHair, maleHair.Length);

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
        Debug.Log("isFemale: " + isFemale);

        if (isFemale)
        {
            Debug.Log("Female shirts: " + femaleShirts.Length);

            for (int i = 0; i < femaleShirts.Length; i++)
            {
                Debug.Log("Female Shirt " + i + " = " + femaleShirts[i].name);
            }

            SetOnlyActive(femaleShirts, currentShirt);
            SetOnlyActive(femalePants, currentPants);
            SetOnlyActive(femaleShoes, currentShoes);
            SetOnlyActive(femaleHair, currentHair);
        }
        else
        {
            Debug.Log("Male shirts: " + maleShirts.Length);

            for (int i = 0; i < maleShirts.Length; i++)
            {
                Debug.Log("Male Shirt " + i + " = " + maleShirts[i].name);
            }

            SetOnlyActive(maleShirts, currentShirt);
            SetOnlyActive(malePants, currentPants);
            SetOnlyActive(maleShoes, currentShoes);
            SetOnlyActive(maleHair, currentHair);
        }
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