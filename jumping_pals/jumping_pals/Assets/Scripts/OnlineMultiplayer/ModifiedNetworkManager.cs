using UnityEngine;
using Mirror;
using System.Collections;
using System.Security;
using UnityEngine.Networking;
public class ModifiedNetworkManager : NetworkManager
{
    public Transform[] spawnPoints;
    private int index = 0;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform spawnPoint = spawnPoints[index];

        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        index++;
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= 2)
        {
            conn.Disconnect();
            Debug.Log("Partida llena");
            return;
        }
        else
        {
            base.OnServerConnect(conn);
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        DeleteRoom();
    }

    public void DeleteRoom()
    {
        StartCoroutine(DeleteRoomCoroutine());
    }

    IEnumerator DeleteRoomCoroutine()
    {
        string roomId = PlayerPrefs.GetString("roomId");
        if (!string.IsNullOrEmpty(roomId))
        {
            string serverURL = "https://jumping-pals.onrender.com/delete-room/" + roomId;
            UnityWebRequest request = UnityWebRequest.Delete(serverURL);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Sala eliminada exitosamente");
            }
            else
            {
                Debug.LogError("No se pudo eliminar la sala: " + request.error);
            }
        }
    }
}
