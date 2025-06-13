using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System;

public class Movement : MonoBehaviour
{
    public int jumps;
    public int enemiesKilled;
    public int score;
    public bool onIce;
    public bool onSlime;
    public bool onJump;

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

    public bool alive = true;

    public float defaultSpeed = 15;
    public float speed;
    public float jumpForce;
    [SerializeField] private LayerMask floor;
    [SerializeField] private LayerMask enemy;
    public LayerMask bottom;

    [Header("---------- Objects ----------")]

    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private ScoreManager scoreManager;
    public MainCamera camera;
    private SpriteRenderer sprite;

    // States -> Animation management

    // Start is called before the first frame update
    void Start()
    {
        audioManager.playMusic();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        jumps = 0;
        enemiesKilled = 0;
        score = 0;

        onIce = false;
        onSlime = false;
        onJump = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (alive) {

        //Attacked

        // Patroll
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

        //Movement

        if (!isGrounded) {
            onIce = false;
            onSlime = false;
            onJump = false;
        }

        string platformTag = hitFloor.collider != null ? hitFloor.collider.tag : "None";
        switch (platformTag) {
            case "ice_platform":
                onIce = true;
                break;
            case "slime_platform":
                onSlime = true;
                break;
            case "jump_platform":
                onJump = true;
                break;
        }

        if (onIce) {
            speed = defaultSpeed / 2;
        } else {
            speed = defaultSpeed;
        }

        hitFloor = Physics2D.Raycast(transform.position + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
        hitFloorLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
        hitFloorRight = Physics2D.Raycast(transform.position + Vector3.right * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);

        isGrounded = (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorRight.collider != null);
        animator.SetBool("IsJumping", !isGrounded);

        if (onSlime) {
            MovX = MovX * 0.5f;
        } else {
            MovX = Input.GetAxisRaw("Horizontal");
        }

        FlipSprite(MovX);
        transform.Translate(MovX * speed * Time.deltaTime, 0, 0);
        animator.SetFloat("Xvelocity", (float) Math.Abs(Math.Round(MovX)));
        animator.SetFloat("Yvelocity", (float) rb.linearVelocity.y);

        if (isGrounded)
        {
            lastGrounded = coyoteTime;
        }
        else
        {
            lastGrounded -= Time.deltaTime;
        }

        if (onJump && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }


        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && lastGrounded > 0f && !pauseManager.isPaused)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            jumps += 1;
            lastGrounded = 0f;
            audioManager.playJumping();
            animator.SetTrigger("Jump");
        }

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"));

        } else { // Player is death
            if (transform.position.y < -14) {
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    // Interface, Sprite
    public void FlipSprite(float moveX)
    {
        if (moveX > 0)
        {
            sprite.flipX = true; // mirando a la derecha
        }
        else if (moveX < 0)
        {
            sprite.flipX = false;  // mirando a la izquierda
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Death by muncher, spike
        if (other.collider.CompareTag("muncher")) {
            die();
        }

    }

    // Death by Death Bar
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Death Bar") || other.CompareTag("Laser"))
        {
            die();
        }
    }

    // Death protocol
    private void die() {
        alive = false;
        camera.isAlive = false;
        pauseButton.SetActive(false);

        scoreManager.ShowDeathMessage(score, jumps, enemiesKilled);
            
        audioManager.stopMusic();
        audioManager.playGameOver();

        animator.SetTrigger("Death");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        camera.stop();
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Draw the Raycasts that detect the floo and 'patrol enemy' interactions
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.down * 0.75f, Vector3.down * 0.75f);
        Gizmos.DrawRay(transform.position + Vector3.left * 0.8f + Vector3.down * 0.75f, Vector3.down * 0.75f);
        Gizmos.DrawRay(transform.position + Vector3.right * 0.8f + Vector3.down * 0.75f, Vector3.down * 0.75f);

        Gizmos.DrawRay(transform.position, Vector3.left * 1.25f);
        Gizmos.DrawRay(transform.position + Vector3.up * 0.75f, Vector3.left * 1.25f);
        Gizmos.DrawRay(transform.position + Vector3.down * 0.55f, Vector3.left * 1.25f);
        Gizmos.DrawRay(transform.position, Vector3.right * 1.25f);
        Gizmos.DrawRay(transform.position + Vector3.up * 0.75f, Vector3.right * 1.25f);
        Gizmos.DrawRay(transform.position + Vector3.down * 0.55f, Vector3.right * 1.25f);
    }

    // Audio management
    public void playEnemy() {
        audioManager.playEnemy();
    }

}
