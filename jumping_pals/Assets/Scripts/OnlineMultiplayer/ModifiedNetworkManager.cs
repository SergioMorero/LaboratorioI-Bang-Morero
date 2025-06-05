using UnityEngine;
using Mirror;
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
}
