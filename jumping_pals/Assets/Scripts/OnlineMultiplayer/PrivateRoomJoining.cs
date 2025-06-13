using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PrivateRoomJoining : MonoBehaviour
{

    public TMP_InputField inputRoomCode;
    private string serverURL = "https://jumping-pals.onrender.com";

    public void JoinRoomWithCode(string roomId)
    {
        if (string.IsNullOrEmpty(roomId))
        {
            string roomCode = inputRoomCode.text;
            if (!string.IsNullOrEmpty(roomCode))
            {
                StartCoroutine(CheckRoomAndJoin(roomCode));
            }
            else
            {
                Debug.Log("Ingrese un código válido");
            }
        }
        else
        {
            StartCoroutine(CheckRoomAndJoin(roomId));
        }
        
    }


    IEnumerator CheckRoomAndJoin(string roomCode)
    {
        string route = $"https://jumping-pals.onrender.com/get-room/{roomCode}";
        UnityWebRequest www = UnityWebRequest.Get(route);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            RoomResponse response = JsonUtility.FromJson<RoomResponse>(www.downloadHandler.text);
            Debug.Log("Respuesta del servidor" + www.downloadHandler.text);

            if (response.status == "success")
            {
                Debug.Log($"Conectando a sala {roomCode}...");

                // Configurar NetworkManager con IP y puerto de la sala
                NetworkManager.singleton.networkAddress = response.ip;
                NetworkManager.singleton.GetComponent<TelepathyTransport>().port = ushort.Parse(response.port);

                // Intentar conectar
                NetworkManager.singleton.StartClient();
            }
            else
            {
                Debug.Log("La sala no existe.");
            }
        }
        else
        {
            Debug.Log("Response: " + www.result);
            Debug.Log("Error: " + www.responseCode);
        }
    }

    [System.Serializable]
    private class RoomResponse
    {
        public string status;
        public string ip;
        public string port;
    }

}
