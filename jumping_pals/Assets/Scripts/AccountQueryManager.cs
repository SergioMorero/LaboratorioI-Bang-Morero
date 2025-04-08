using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Text;

public class AccountQueryManager : MonoBehaviour {

    private string serverUrl = "http://localhost:5000";

    [SerializeField] private AccountManager accountManager;

    [Header("----- Text inputs -----")]

    [Header("Log In")]
    [SerializeField] private TMP_InputField loginName;
    [SerializeField] private TMP_InputField loginPassword;

    [Header("Create Account")]
    [SerializeField] private TMP_InputField createUsername;
    [SerializeField] private TMP_InputField createPassword;

    [Header("Modify Account")]
    [SerializeField] private TMP_InputField modifyNewUsername;
    [SerializeField] private TMP_InputField modifyNewPassword;

    void Start()
    {
        StartCoroutine(checkConnection());
    }

    // Interface management

    public void cleanAllInput()
    {
        loginName.text = null;
        loginPassword.text = null;
        createUsername.text = null;
        createPassword.text = null;
        modifyNewPassword.text = null;
        modifyNewPassword.text = null;
    }

    // Query Methods

    public void Login()
    {
        string name = loginName.text;
        string password = loginPassword.text;
        StartCoroutine(get(name, password));
    }

    public void CreateUser()
    {
        string name = createUsername.text;
        string password = createPassword.text;
        StartCoroutine(post(name, password));
    }

    public void UpdateUser()
    {
        string name = accountManager.GetName();
        string password = accountManager.GetPassword();
        string newName = modifyNewUsername.text;
        string newPassword = modifyNewPassword.text;
        StartCoroutine(put(name, password, newName, newPassword));
    }

    public void DeleteUser() {
        string name = accountManager.GetName();
        string password = accountManager.GetPassword();
        StartCoroutine(delete(name, password));
    }

    // Query Coroutines

    IEnumerator checkConnection()
    {
        string route = "/server";
        UnityWebRequest request = UnityWebRequest.Get(serverUrl + route);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("¡Conexión con el servidor exitosa!");
        }
        else
        {
            Debug.Log("Error en la conexión: " + request.error);
        }
    }

    IEnumerator get(string name, string password)
    {

        string route = "/login";

        UserData data = new UserData { name = name, password = password };
        string json = JsonUtility.ToJson(data);
        Debug.Log("JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "POST");
        Debug.Log("Created request");
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Set request header");

        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(raw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log("Request sent");

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("Usuario Válido: " + request.downloadHandler.text);
            UserResponse response = JsonUtility.FromJson<UserResponse>(request.downloadHandler.text);
            Debug.Log("ID: " + response.id + ", name: " + response.name + ", password: " + response.password);
            accountManager.LogUserIn(response.id, response.name, response.password);
        } else {
            Debug.Log("Error en la autenticación: " + request.error);
            accountManager.ShowError("login");
        }
    }

    IEnumerator post(string name, string password)
    {
        string route = "/user";

        UserData data = new UserData { name = name, password = password };
        string json = JsonUtility.ToJson(data);
        Debug.Log("JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "POST");
        Debug.Log("Created request");
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Set request header");

        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(raw);
        request.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("JSON: " + json);

        yield return request.SendWebRequest();

        Debug.Log("Request sent");

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("Usuario Creado: " + request.downloadHandler.text);

            // Realizar una query para obtener la id recién agregada y realizar Log In
            StartCoroutine(get(name, password));

        } else {
            Debug.Log("Error en la creación: " + request.error);
            accountManager.ShowError("register");
        }
    }

    IEnumerator put(string oldName, string oldPassword, string newName, string newPassword)
    {

        string route = "/user";

        UserUpdate data = new UserUpdate { name = oldName, password = oldPassword, newName = newName, newPassword = newPassword };
        string json = JsonUtility.ToJson(data);
        Debug.Log("JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "PUT");
        Debug.Log("Created request");
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Set request header");

        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(raw);
        request.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("JSON: " + json);

        yield return request.SendWebRequest();

        Debug.Log("Request sent");

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("Usuario Modificado: " + request.downloadHandler.text);
            accountManager.UpdateUser(newName, newPassword);
        } else {
            Debug.Log("Error en la Modificación: " + request.error);
            accountManager.ShowError("edit");
        }
    }

    IEnumerator delete(string name, string password)
    {

        string route = "/user";

        UserData data = new UserData { name = name, password = password };
        string json = JsonUtility.ToJson(data);
        Debug.Log("JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "DELETE");
        Debug.Log("Created request");
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Set request header");

        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(raw);
        request.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("JSON: " + json);

        yield return request.SendWebRequest();

        Debug.Log("Request sent");

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("Usuario eliminado");
            accountManager.LogUserOut();
        } else {
            Debug.Log("Error en la eliminación: " + request.error);
            accountManager.ShowError("delete");
        }
    }

    // JSON Classes

    [System.Serializable]
    public class UserData {
        public string name;
        public string password;
    }

    [System.Serializable]
    public class UserUpdate {
        public string name;
        public string password;
        public string newName;
        public string newPassword;
    }

    [System.Serializable]
    public class UserResponse
    {
        public int id;
        public string name;
        public string password;
    }

}