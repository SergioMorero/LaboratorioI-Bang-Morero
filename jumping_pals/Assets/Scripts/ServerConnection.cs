using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnection : MonoBehaviour
{
    private string serverUrl = "https://jumping-pals.onrender.com";

    void Start()
    {
        StartCoroutine(GetUsuarios());
    }

    IEnumerator GetUsuarios()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Respuesta del servidor: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error en la conexión: " + request.error);
        }
    }
}