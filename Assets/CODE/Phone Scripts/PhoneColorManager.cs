using UnityEngine;

public class PhoneColorManager : MonoBehaviour
{
    [Header("Phone Objects")]
    public GameObject defaultPhone;
    public GameObject lavenderPhone;
    public GameObject blackPhone;
    public GameObject mintPhone;

    void Start()
    {
        LoadPhoneColor();
    }

    public void SetDefault()
    {
        ShowPhone(defaultPhone);

        PlayerPrefs.SetInt("PhoneColor", -1);
        PlayerPrefs.Save();
    }

    public void SetLavender()
    {
        ShowPhone(lavenderPhone);

        PlayerPrefs.SetInt("PhoneColor", 0);
        PlayerPrefs.Save();
    }

    public void SetBlack()
    {
        ShowPhone(blackPhone);

        PlayerPrefs.SetInt("PhoneColor", 1);
        PlayerPrefs.Save();
    }

    public void SetMint()
    {
        ShowPhone(mintPhone);

        PlayerPrefs.SetInt("PhoneColor", 2);
        PlayerPrefs.Save();
    }

    void ShowPhone(GameObject phone)
    {
        defaultPhone.SetActive(false);
        lavenderPhone.SetActive(false);
        blackPhone.SetActive(false);
        mintPhone.SetActive(false);

        phone.SetActive(true);
    }

    void LoadPhoneColor()
    {
        int savedColor = PlayerPrefs.GetInt("PhoneColor", -1);

        switch (savedColor)
        {
            case -1:
                ShowPhone(defaultPhone);
                break;

            case 0:
                ShowPhone(lavenderPhone);
                break;

            case 1:
                ShowPhone(blackPhone);
                break;

            case 2:
                ShowPhone(mintPhone);
                break;

            default:
                ShowPhone(defaultPhone);
                break;
        }
    }
}