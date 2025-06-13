using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class OnlineDeleteGeneration : NetworkBehaviour
{

    [Server]
    public void despawn(Collider2D other)
    {
        NetworkServer.Destroy(other.gameObject);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        despawn(other);
    }
}
