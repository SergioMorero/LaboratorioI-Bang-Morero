using UnityEngine;

public class coinScript : MonoBehaviour
{
    private ScoreManager scoreManager;
    private bool collected;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private float initialPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collected) {
            if (transform.position.y < initialPosition) {
                Destroy(gameObject);
            }
        }
    }

    private void jump() {
        initialPosition = transform.position.y;
        rb.gravityScale = 4f;
        rb.AddForce(Vector3.up * 15f, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            scoreManager.GiveCoin();
            // GetComponent<BoxCollider2D>().enabled = false;
            collected = true;
            jump();
            //Destroy(gameObject);
        }
    }

}
