using UnityEngine;

public class coinScript : MonoBehaviour
{
    private ScoreManager scoreManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scoreManager.GiveCoin();
            Destroy(gameObject);
        }
    }

}
