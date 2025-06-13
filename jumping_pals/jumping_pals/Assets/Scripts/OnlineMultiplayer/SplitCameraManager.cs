using System.Collections;
using Mirror.Examples.Common.Controllers.Player;
using Mirror;
using System.Linq;
using UnityEngine;

public class SplitCameraManager : MonoBehaviour
{

    public Camera camera1;
    public Camera camera2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        while (GameObject.FindGameObjectsWithTag("Player").Length < 2)
        {
            yield return null;
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");


        // The Host always goes to the left, the Cliente always to the right
        GameObject player1 = players[0].transform.position.x < 0 ? players[0] : players[1];
        GameObject player2 = players[0].transform.position.x < 0 ? players[1] : players[0];

        camera1.GetComponent<FollowPlayer>().targetPlayer = player1;
        camera2.GetComponent<FollowPlayer>().targetPlayer = player2;

        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(true);

        

        GameObject.Find("MainCamera").gameObject.SetActive(false);
    
    }
}
