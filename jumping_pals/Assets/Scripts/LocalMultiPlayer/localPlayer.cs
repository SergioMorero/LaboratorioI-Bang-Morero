using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LocalPlayer : MonoBehaviour
{

    [Header("---------- Variables ----------")]

    private Rigidbody2D rb;
    private float MovX;
    [SerializeField] private float coyoteTime;
    private float lastGrounded;
    public bool isGrounded;
    private RaycastHit2D hitFloor;
    private RaycastHit2D hitFloorLeft;
    private RaycastHit2D hitFloorRight;
    private Animator animator;

    private RaycastHit2D hitEnemyLeft;
    private RaycastHit2D hitEnemyUpLeft;
    private RaycastHit2D hitEnemyDownLeft;
    private RaycastHit2D hitEnemyRight;
    private RaycastHit2D hitEnemyUpRight;
    private RaycastHit2D hitEnemyDownRight;

    public string player;
    public bool alive = true;
    public float speed = 15;
    public float jumpForce;
    public bool arePlayersAlive = true;
    [SerializeField] private LayerMask floor;
    [SerializeField] private LayerMask enemy;
    public LayerMask bottom;

    private KeyCode left;
    private KeyCode right;
    private KeyCode up;

    private Transform transform;

    [Header("---------- Objects ----------")]

    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LocalScoreManager scoreManager;
    [SerializeField] private GameObject nameDisplayer;
    public MainCamera camera;
    private SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager.playMusic();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = gameObject.name;
        transform = GetComponent<Transform>();
        switch (player)
        {
            case "player1":
                left = KeyCode.LeftArrow;
                right = KeyCode.RightArrow;
                up = KeyCode.UpArrow;
                break;
            case "player2":
                left = KeyCode.A;
                right = KeyCode.D;
                up = KeyCode.W;
                break;
        }
        scoreManager = GameObject.Find("ScoreManager").GetComponent<LocalScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (alive)
        {
            StartCoroutine(displayName());

            hitEnemyLeft = Physics2D.Raycast(transform.position, Vector3.left, 1.25f, enemy);
            hitEnemyUpLeft = Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector3.left, 1.25f, enemy);
            hitEnemyDownLeft = Physics2D.Raycast(transform.position + Vector3.down * 0.55f, Vector3.left, 1.25f, enemy);
            hitEnemyRight = Physics2D.Raycast(transform.position, Vector3.right, 1.25f, enemy);
            hitEnemyUpRight = Physics2D.Raycast(transform.position + Vector3.up * 0.75f, Vector3.right, 1.25f, enemy);
            hitEnemyDownRight = Physics2D.Raycast(transform.position + Vector3.down * 0.55f, Vector3.right, 1.25f, enemy);

            if (hitEnemyLeft.collider != null || hitEnemyUpLeft.collider != null || hitEnemyDownLeft.collider != null || hitEnemyRight.collider != null || hitEnemyUpRight.collider != null || hitEnemyDownRight.collider != null)
            {
                die();
            }

            hitFloor = Physics2D.Raycast(transform.position + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
            hitFloorLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
            hitFloorRight = Physics2D.Raycast(transform.position + Vector3.right * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);

            isGrounded = (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorRight.collider != null);
            animator.SetBool("IsJumping", !isGrounded);

            MovX = (Input.GetKey(left) ? -1 : 0) + (Input.GetKey(right) ? 1 : 0);
            FlipSprite(MovX);
            transform.Translate(MovX * speed * Time.deltaTime, 0, 0);
            animator.SetFloat("Xvelocity", (float) Math.Abs(Math.Round(MovX)));
            animator.SetFloat("Yvelocity", rb.linearVelocity.y);

            if (isGrounded)
            {
                lastGrounded = coyoteTime;
            }
            else
            {
                lastGrounded -= Time.deltaTime;
            }

            if (Input.GetKeyDown(up) && lastGrounded > 0f && !pauseManager.isPaused)
            {
                scoreManager.jumps += 1;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                lastGrounded = 0f;
                audioManager.playJumping();
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            if (transform.position.y < -14)
            {
                GetComponent<BoxCollider2D>().enabled = true;
                Destroy(this.gameObject);
            }
        }


    }

    public void FlipSprite(float moveX)
    {
        if (moveX > 0)
        {
            sprite.flipX = true;
        }
        else if (moveX < 0)
        {
            sprite.flipX = false;
        }
    }

    // Death by muncher, spike
    private void OnCollisionEnter2D(Collision2D muncher)
    {
        if (muncher.collider.CompareTag("muncher"))
        {
            die();
        }

    }

    // Death by Death Bar
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death Bar") || other.CompareTag("Laser"))
        {
            die();
        }
    }

    private void die()
    {
        alive = false;
        nameDisplayer.setActive(false);

        int count = 0;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<LocalPlayer>().alive) count++;
        }
        if (count == 0) arePlayersAlive = false;

        Debug.Log(arePlayersAlive);

        //All players died
        if (!arePlayersAlive)
        {
            camera.isAlive = false;
            pauseButton.SetActive(false);

            scoreManager.ShowDeathMessage();

            audioManager.stopMusic();
            
            camera.stop();
        }
        audioManager.playGameOver();
        animator.SetTrigger("Death");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator displayName()
    {
        nameDisplayer.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, 0);
        yield return null;
    }

    public void playEnemy()
    {
        audioManager.playEnemy();
    }

    public void revive(Vector3 position) {
        alive = true;
        nameDisplayer.setActive(true);
        GetComponent<BoxCollider2D>().enabled = true;
        /*
        audioManager.playRevive();     -> Not implemented yet.
        animator.setTrigger("Revive"); -> Restart animation.
         */
        transform.position = position;
    }
}
