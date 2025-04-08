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

    // Publics

    public bool isLogged;

    // Serializeds

    [Header("----- Objects -----")]

    [SerializeField] private AccountQueryManager queryManager;
    [SerializeField] private Button accountButton;

    [Header("----- Panels -----")]

    [SerializeField] private GameObject init;
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject create;
    [SerializeField] private GameObject account;
    [SerializeField] private GameObject edit;
    [SerializeField] private GameObject deleteConfirm;
    [SerializeField] private GameObject logoutConfirm;

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

    // Initialize

    void Start()
    {
        userID = 0;
        userName = null;
        userPassword = null;
        isLogged = false;
        HideAllErrors();
    }

    // Main Methods

    public void LogUserIn(int id, string name, string password)
    {
        userID = id;
        userName = name;
        userPassword = password;
        HideAllErrors();
        login.SetActive(false);
        create.SetActive(false);
        account.SetActive(true);
        isLogged = true;
    }

    public void LogUserOut()
    {
        userID = 0;
        userName = null;
        userPassword = null;
        HideAllErrors();
        logoutConfirm.SetActive(false);
        deleteConfirm.SetActive(false);
        account.SetActive(false);
        init.SetActive(true);
        isLogged = false;
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
