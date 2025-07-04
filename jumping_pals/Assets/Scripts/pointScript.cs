using Unity.VisualScripting;
using UnityEngine;

public class pointScript : MonoBehaviour
{
    private PlatformGenerator platformGenerator = null;
    private LocalScoreManager scoreManager = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Multiplayer won�t add points(?)
        platformGenerator = FindAnyObjectByType<PlatformGenerator>();
        GameObject scoreManagerObject = GameObject.Find("ScoreManager");
        if (scoreManagerObject != null) scoreManager = scoreManagerObject.GetComponent<LocalScoreManager>();
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
                    if (playerScript != null)
                    {
                        playerScript.score += 1;
                        Debug.Log(playerScript.score);
                    }

                }
                else
                {
                    scoreManager.score += 1;
                }
                Destroy(this.gameObject);
            
        }

    }
}
