using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Mirror;
using UnityEngine.UI;

public class RoomDisplayerManager : MonoBehaviour
{
    public string serverUrl = "https://jumping-pals.onrender.com";
    public Transform contentContainer; // El objeto con VerticalLayoutGroup
    public GameObject roomItemPrefab;  // Prefab que tiene RoomItemUI

    void Start()
    {
        StartCoroutine(GetRooms());
    }

    IEnumerator GetRooms()
    {
        string route = "/get-all-rooms";
        UnityWebRequest request = UnityWebRequest.Get(serverUrl + route);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // El JSON que llega es un array. Para que JsonUtility funcione, lo envolvemos:
            string rawJson = "{\"rooms\":" + request.downloadHandler.text + "}";

            RoomListWrapper wrapper = JsonUtility.FromJson<RoomListWrapper>(rawJson);

            // Limpiar antes de cargar nuevos
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (RoomData room in wrapper.rooms)
            {
                GameObject roomGO = Instantiate(roomItemPrefab, contentContainer);
                RoomItemUI roomUI = roomGO.GetComponent<RoomItemUI>();
                roomUI.Setup(room.roomId, room.host, JoinRoom);
            }
        }
        else
        {
            Debug.LogError("Error al obtener salas: " + request.error);
        }
    }

    // Método que se llama cuando se hace clic en una sala
    void JoinRoom(string roomId)
    {
        Debug.Log("Intentando unirse a la sala con ID: " + roomId);
        // Usamos el mismo script de joining que ya tienes
        GameObject.FindFirstObjectByType<PrivateRoomJoining>().JoinRoomWithCode(roomId);
    }

    [System.Serializable]
    public class RoomData
    {
        public string roomId;
        public string ip;
        public string port;
        public string host;
    }

    [System.Serializable]
    public class RoomListWrapper
    {
        public List<RoomData> rooms;
    }
}
