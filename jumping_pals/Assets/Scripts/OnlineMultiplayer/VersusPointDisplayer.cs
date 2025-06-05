using TMPro;
using UnityEngine;

public class VersusPointDisplayer : MonoBehaviour
{
    public OnlineSpawner spawnManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.gameObject.activeSelf)
        {
            this.GetComponent<TextMeshProUGUI>().text = 
                "" + (this.name == "HostPoints" ? spawnManager.hostScore : spawnManager.clientScore);
        }
    }
}
