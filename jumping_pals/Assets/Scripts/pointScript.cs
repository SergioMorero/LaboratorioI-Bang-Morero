using Unity.VisualScripting;
using UnityEngine;

public class pointScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Movement playerScript = other.GetComponent<Movement>();
            playerScript.score += 1;
            Debug.Log(playerScript.score);
            Destroy(this.gameObject);
        }
    }
}
