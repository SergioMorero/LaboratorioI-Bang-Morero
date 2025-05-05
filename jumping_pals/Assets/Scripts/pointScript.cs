using Unity.VisualScripting;
using UnityEngine;

public class pointScript : MonoBehaviour
{
    private PlatformGenerator platformGenerator;
    private LocalScoreManager scoreManager;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformGenerator = GameObject.Find("PlatformGenerator").GetComponent<PlatformGenerator>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent <LocalScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (platformGenerator.isSinglePlayer)
            {
                Movement playerScript = other.GetComponent<Movement>();
                playerScript.score += 1;
                Debug.Log(playerScript.score);
            }
            else
            {
                scoreManager.score += 1;
            }
            Destroy(this.gameObject);
        }
    }
}
