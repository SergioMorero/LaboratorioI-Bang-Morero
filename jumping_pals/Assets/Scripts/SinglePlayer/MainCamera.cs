using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    public GameObject highestPlayer;
    private GameObject deathBar;
    private GameObject[] players = new GameObject[2];
    private float prevHeigh;
    public bool isAlive = true; // Player is alive
    private bool isSinglePlayer;

    // Start is called before the first frame update
    void Start()
    {
        deathBar = GameObject.FindWithTag("Death Bar");
        isSinglePlayer = GameObject.Find("PlatformGenerator").GetComponent<PlatformGenerator>().isSinglePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        switch (players.Length)
        {
            case 0:
                highestPlayer = null;
                break;
            case 1:
                highestPlayer = players[0];
                break;
            case 2:
                highestPlayer = (players[0].transform.position.y >= players[1].transform.position.y) ? players[0] : players[1];
                break;
        }
        bool isHighestPlayerAlive = false;

        if (highestPlayer == null) isHighestPlayerAlive = false;
        else if (highestPlayer.GetComponent<LocalPlayer>() != null) isHighestPlayerAlive = highestPlayer.GetComponent<LocalPlayer>().alive;
        else if (highestPlayer.GetComponent<Movement>() != null) isHighestPlayerAlive = highestPlayer.GetComponent<Movement>().alive;


        if (highestPlayer != null && isAlive && isHighestPlayerAlive)
        {
            prevHeigh = transform.position.y;
            if (transform.position.y < highestPlayer.transform.position.y + 1 && highestPlayer.transform.position.y > prevHeigh)
            {
                transform.position = new Vector3(transform.position.x, highestPlayer.transform.position.y, transform.position.z);
                deathBar.transform.position = new Vector3(transform.position.x, highestPlayer.transform.position.y - 17, transform.position.z);
            }    
        }
    }

    public void stop() {
        isAlive = false; // Player has perished
    }

}
