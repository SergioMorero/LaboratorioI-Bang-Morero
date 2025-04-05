using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Text;

public class AccountQueryManager : MonoBehaviour {

    private string serverUrl = "http://localhost:5000";

    [Header("----- Text inputs -----")]

    [Header("Create Account")]
    [SerializeField] private TMP_InputField createUsername;
    [SerializeField] private TMP_InputField createPassword;

    [Header("Modify Account")]
    [SerializeField] private TMP_InputField modifyOldUsername;
    [SerializeField] private TMP_InputField cmodifyOldPassword;
    [SerializeField] private TMP_InputField modifyNewUsername;
    [SerializeField] private TMP_InputField modifyNewPassword;

    [Header("Delete Account")]
    [SerializeField] private TMP_InputField deleteUsername;
    [SerializeField] private TMP_InputField deletePassword;

    void Start()
    {
        StartCoroutine(checkConnection());
    }

    public void createUser()
    {
        string name = createUsername.text;
        string password = createPassword.text;
        StartCoroutine(post(name, password));
    }

    public void deleteUser() {
        string name = deleteUsername.text;
        string password = deletePassword.text;
        StartCoroutine(delete(name, password));
    }

    IEnumerator checkConnection()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("¡Conexión con el servidor exitosa!: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error en la conexión: " + request.error);
        }
    }

    IEnumerator post(string name, string password)
    {

        string route = "/create-user";

        UserData data = new UserData { name = name, password = password };
        string json = JsonUtility.ToJson(data);
        Debug.Log("JSON: " + json);


        // string data = "{\"name\": " + name + ", \"password\": " + password + "}";
        
        /* 
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = name;
        data["password"] = password;
        */

        //Debug.Log("Got data: " + data);

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
        } else {
            Debug.Log("Error en la creación: " + request.error);
        }
    }

    IEnumerator delete(string name, string password)
    {

        string route = "/delete-user";

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=name&field2=password"));
        formData.Add(new MultipartFormFileSection(name, password));

        UnityWebRequest request = UnityWebRequest.Post(serverUrl + route, formData);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("Usuario Creado: " + request.downloadHandler.text);
        } else {
            Debug.Log("Error en la creación: " + request.error);
        }
    }

    [System.Serializable]
    public class UserData {
        public string name;
        public string password;
}

}