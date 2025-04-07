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

    void Start()
    {
        userID = 0;
        userName = null;
        userPassword = null;
        isLogged = false;
        hideAllErrors();
    }

    public void LogUserIn(int id, string name, string password)
    {
        userID = id;
        userName = name;
        userPassword = password;
        hideAllErrors();
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
        hideAllErrors();
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
        hideAllErrors();
        edit.SetActive(false);
    }

    public void hideAllErrors()
    {
        loginError.SetActive(false);
        createError.SetActive(false);
        deleteError.SetActive(false);
        editError.SetActive(false);
    }

    public void ShowLoginError()
    {
        loginError.SetActive(true);
    }

    public void ShowCreateError()
    {
        createError.SetActive(true);
    }

    public void ShowDeleteError()
    {
        deleteError.SetActive(true);
    }

    public void ShowEditError()
    {
        editError.SetActive(true);
    }

    public string GetName()
    {
        return userName;
    }

    public string GetPassword()
    {
        return userPassword;
    }

}
