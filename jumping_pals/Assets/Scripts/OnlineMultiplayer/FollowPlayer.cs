using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject targetPlayer;
    public GameObject deathBar;
    public GameObject platformGenerator;
    private float prevHigh;
    public bool isAlive = true;
    private int distance = 33;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathBar = this.name == "Camera1" ? GameObject.Find("Bottom1") : GameObject.Find("Bottom2");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && targetPlayer != null)
        {
            prevHigh = transform.position.y;
            if (transform.position.y < targetPlayer.transform.position.y + 5 && targetPlayer.transform.position.y > prevHigh)
            {
                transform.position = new Vector3(transform.position.x, targetPlayer.transform.position.y, transform.position.z);
                deathBar.transform.position = new Vector3(transform.position.x, targetPlayer.transform.position.y - distance, transform.position.z);
                
            }
        }
    }

    public void stop()
    {
        isAlive = false;
    }

    public void OnEnable()
    {
        platformGenerator.SetActive(true);
        platformGenerator.GetComponent<OnlinePlatformGenerator>().player = targetPlayer;
        targetPlayer.GetComponent<OnlinePlayerMovement>().setClientPlayer();
        if (this.name == "Camera2") GameObject.Find("OnlineSpawner").GetComponent<OnlineSpawner>().startTimer();
    }
}
