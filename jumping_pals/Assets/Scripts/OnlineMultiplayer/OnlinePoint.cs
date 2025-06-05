using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class OnlinePoint : NetworkBehaviour
{
    public OnlineSpawner spawnManager;

    void Start()
    {
        spawnManager = GameObject.Find("OnlineSpawner").GetComponent<OnlineSpawner>();
    }

    public void addPoint(string player)
    {
        spawnManager.addPoint(player);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlatformGenerator"))
        {
            Debug.Log("Entering...");
            string player = other.transform.position.x < 0 ? "host" : "client";
            addPoint(player);
        }
    }
}
