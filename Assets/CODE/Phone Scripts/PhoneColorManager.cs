using UnityEngine;
using UnityEngine.UI;

public class PhoneColorManager : MonoBehaviour
{
    [Header("Phone Images")]
    public Image homePhone;
    public Image radioPhone;
    public Image galleryPhone;
    public Image settingsPhone;

    [Header("Phone Sprites")]
    public Sprite defaultPhone;
    public Sprite lavenderPhone;
    public Sprite blackPhone;
    public Sprite mintPhone;

    void Start()
    {
        LoadPhoneColor();
    }

    public void SetDefault()
    {
        ChangePhoneColor(defaultPhone);
        PlayerPrefs.SetInt("PhoneColor", -1);
        PlayerPrefs.Save();
    }

    public void SetLavender()
    {
        ChangePhoneColor(lavenderPhone);
        PlayerPrefs.SetInt("PhoneColor", 0);
        PlayerPrefs.Save();
    }

    public void SetBlack()
    {
        ChangePhoneColor(blackPhone);
        PlayerPrefs.SetInt("PhoneColor", 1);
        PlayerPrefs.Save();
    }

    public void SetMint()
    {
        ChangePhoneColor(mintPhone);
        PlayerPrefs.SetInt("PhoneColor", 2);
        PlayerPrefs.Save();
    }

    private void ChangePhoneColor(Sprite phoneSprite)
    {
        homePhone.sprite = phoneSprite;
        radioPhone.sprite = phoneSprite;
        galleryPhone.sprite = phoneSprite;
        settingsPhone.sprite = phoneSprite;
    }

    private void LoadPhoneColor()
    {
        int savedColor = PlayerPrefs.GetInt("PhoneColor", -1);

        switch (savedColor)
        {
            case -1:
                ChangePhoneColor(defaultPhone);
                break;

            case 0:
                ChangePhoneColor(lavenderPhone);
                break;

            case 1:
                ChangePhoneColor(blackPhone);
                break;

            case 2:
                ChangePhoneColor(mintPhone);
                break;

            default:
                ChangePhoneColor(defaultPhone);
                break;
        }
    }
}