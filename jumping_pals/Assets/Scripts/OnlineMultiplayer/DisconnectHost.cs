using Mirror;
using UnityEngine;

public class DisconnectHost : MonoBehaviour
{

    public void Disconnect()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            // Si es host (server + client)
            Debug.Log("Cerrando Host...");
            NetworkManager.singleton.StopHost();
        }
    }
}
