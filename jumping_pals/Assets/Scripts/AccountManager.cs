using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccountManager : MonoBehaviour
{

    /*
     This class will manage user Log-in and Log-out, recieving information from
     AccountQueryManager. This class will not send queries nor interact with the server

     This class will also manage most (but not all) interaction between account panels
     */

    // Variables

    // Privates
    
    private int userID;
    private string userName;
    private string userPassword;
    private int userScore;
    private int userCoins;

    // Publics

    public bool isLogged;

    // Serializeds

    [Header("----- Objects -----")]

    [SerializeField] private AccountQueryManager queryManager;

    [Header("----- Panels -----")]

    [SerializeField] private GameObject init;
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject create;
    [SerializeField] private GameObject account;
    [SerializeField] private GameObject edit;
    [SerializeField] private GameObject deleteConfirm;
    [SerializeField] private GameObject logoutConfirm;

    [Header("----- Text -----")]
    [SerializeField] private TMP_Text accountName;
    [SerializeField] private TMP_Text MaxScore;

    [Header("----- Buttons -----")]

    // From init panel
    [SerializeField] private Button toLogin;
    [SerializeField] private Button toCreate;
    // Login, register and edit panels
    [SerializeField] private Button loginButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button editButton;
    // From account panel
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button toEdit;
    [SerializeField] private Button deleteButton;
    // From delete panel
    [SerializeField] private Button deleteYes;
    [SerializeField] private Button deleteNo;
    // From logout pannel
    [SerializeField] private Button logoutYes;
    [SerializeField] private Button logoutNo;

    [Header("----- Error Texts -----")]

    [SerializeField] private GameObject loginError;
    [SerializeField] private GameObject createError;
    [SerializeField] private GameObject deleteError;
    [SerializeField] private GameObject editError;

    [Header("----- Start Button -----")]
    [SerializeField] private GameObject ButtonsMenu;
    [SerializeField] private GameObject NotLoggedInPanel;
    [SerializeField] private GameObject PlayMenu;

    // Initialize

    void Start()
    {
        loadPreferences();
        HideAllErrors();
        queryManager.cleanAllInput();
    }

    // Main Methods

    public void LogUserIn(int id, string name, string password, int score, int coins)
    {
        userID = id;
        userName = name;
        userPassword = password;
        userScore = score;
        userCoins = coins;

        accountName.text = name;
        MaxScore.text = score.ToString();

        HideAllErrors();
        login.SetActive(false);
        create.SetActive(false);
        account.SetActive(true);
        isLogged = true;
        queryManager.cleanAllInput();
        savePreferences();
    }

    public void LogUserOut()
    {
        erasePreferences();
        HideAllErrors();
        logoutConfirm.SetActive(false);
        deleteConfirm.SetActive(false);
        account.SetActive(false);
        init.SetActive(true);
    }

    public void UpdateUser(string newName, string newPassword)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            userName = newName;
        }
        if (!string.IsNullOrEmpty(newPassword))
        {
            userPassword = newPassword;
        }   
        HideAllErrors();
        edit.SetActive(false);
        queryManager.cleanAllInput();
        savePreferences();
    }

    // PlayerPrefs

    private void loadPreferences() {
        if (PlayerPrefs.HasKey("Username")) {
            isLogged = true;
            userName = PlayerPrefs.GetString("Username");
            userPassword = PlayerPrefs.GetString("Password");
            userID = PlayerPrefs.GetInt("ID");
            userScore = PlayerPrefs.GetInt("Score");
            userCoins = PlayerPrefs.GetInt("Coins");

            accountName.text = userName;
            MaxScore.text = userScore.ToString();

        } else {
            isLogged = false;
            userID = 0;
            userName = null;
            userPassword = null;
            userScore = 0;
            userCoins = 0;
        }
    }

    private void savePreferences() {
        PlayerPrefs.SetString("Username", userName);
        PlayerPrefs.SetString("Password", userPassword);
        PlayerPrefs.SetInt("ID", userID);
        PlayerPrefs.SetInt("Score", userScore);
        PlayerPrefs.SetInt("Coins", userCoins);
    }

    private void erasePreferences() {
        userID = 0;
        userName = null;
        userPassword = null;
        userScore = 0;
        userCoins = 0;
        isLogged = false;

        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.DeleteKey("ID");
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.DeleteKey("Coins");
        PlayerPrefs.DeleteKey("Login");
    }

    // Error Management

    public void HideAllErrors()
    {
        loginError.SetActive(false);
        createError.SetActive(false);
        deleteError.SetActive(false);
        editError.SetActive(false);
    }

    public void ShowError(string error)
    {
        HideAllErrors();
        switch (error)
        {
            case "login":
                loginError.SetActive(true);
                break;
            case "reister":
                createError.SetActive(true);
                break;
            case "delete":
                deleteError.SetActive(true);
                break;
            case "edit":
                editError.SetActive(true);
                break;
            default:
                Debug.Log("Invalid string received when trying to show an error: " + error);
                break;
        }
    }

    // Interface

    public void OpenAccountSettings()
    {
        
        if (isLogged) // Go to account panel
        {
            account.SetActive(true);
        }
        else // Go to Login/Register panel
        {
            init.SetActive(true);
        }
    }

    public void GoPlay()
    {
        if (isLogged)
        {
            ButtonsMenu.SetActive(false);
            PlayMenu.SetActive(true);
        }
        else
        {
            NotLoggedInPanel.SetActive(true);
        }
    }

    // Getters

    public string GetName()
    {
        return userName;
    }

    public string GetPassword()
    {
        return userPassword;
    }

}
