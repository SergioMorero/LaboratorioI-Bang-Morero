using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{

    [Header("----- Panels -----")]
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private GameObject UnLoggedPannel;
    [SerializeField] private TMP_Text CoinAmount;

    [Header("----- Control -----")]
    [SerializeField] private int userID;
    [SerializeField] private string userName;
    [SerializeField] private string userPassword;
    [SerializeField] private int userMaxScore;
    [SerializeField] private int userCoins;
    [SerializeField] private bool isLogged;
    
    public void OpenShop()
    {
        if (PlayerPrefs.GetInt("Login") == 1) // PlayerPrefs does not contain GetBool() or SetBool
        {
            isLogged = true;
        }
        else
        {
            isLogged = false;
        }

        if (isLogged)
        {
            UpdateCoinAmount();
            ShopPanel.SetActive(true);
        }
        else
        {
            UnLoggedPannel.SetActive(true);
        }
    }

    private void UpdateCoinAmount()
    {
        userCoins = PlayerPrefs.GetInt("Coins");
        CoinAmount.text = userCoins.ToString();
    }

}
