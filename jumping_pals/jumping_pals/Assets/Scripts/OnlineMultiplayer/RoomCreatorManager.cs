using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RoomCreatorManager : MonoBehaviour
{


    private string serverUrl = "https://jumping-pals.onrender.com";


    void Start()
    {
        StartCoroutine(TestConnection());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    IEnumerator TestConnection()
    {
        string route = "/server";
        UnityWebRequest request = UnityWebRequest.Get(serverUrl + route);

        yield return request.SendWebRequest();
        Debug.Log("Connection result: " + request.result);
    }

    public void OnCreateRoomButton()
    {
        StartCoroutine(RegisterRoom());
    }

    public string GetLocalIPAddress()
    {
        string localIP = "";
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }

        return localIP;
    }

    IEnumerator RegisterRoom()
    {
        string route = "/create-room";
        string hostIP = GetLocalIPAddress();
        WWWForm form = new WWWForm();
        //form.AddField("ip", NetworkManager.singleton.networkAddress);
        form.AddField("ip", hostIP);
        form.AddField("port", 7777);
        form.AddField("host", PlayerPrefs.GetString("Username"));

        UnityWebRequest request = UnityWebRequest.Post(serverUrl + route, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<CreateRoomResponse>(request.downloadHandler.text);
            if (response.status == "success")
            {
                Debug.Log("Sala creada con ID: " + response.room_id);
                PlayerPrefs.SetString("roomId", response.room_id);
                NetworkManager.singleton.StartHost();
                NetworkManager.singleton.ServerChangeScene("multiplayer scene");
            }
            else
            {
                Debug.LogError("Error al registrar sala");
            }
        }
        else
        {
            Debug.LogError("Error de red: " + request.error);
            Debug.Log("Código HTTP: " + request.responseCode);
            Debug.Log(request.result);
        }
    }


    [System.Serializable]
    public class CreateRoomResponse
    {
        public string status;
        public string room_id;
    }
}
