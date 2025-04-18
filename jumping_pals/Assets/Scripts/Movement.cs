using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{

    public int score;

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

    public float speed;
    public float jumpForce;
    [SerializeField] private LayerMask floor;
    [SerializeField] private LayerMask enemy;

    [Header("---------- Objects ----------")]

    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private SpriteRenderer Self;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        animator = GetComponent<Animator>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
        hitFloor = Physics2D.Raycast(transform.position + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
        hitFloorLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
        hitFloorRight = Physics2D.Raycast(transform.position + Vector3.right * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);

        isGrounded = (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorRight.collider != null);

        MovX = Input.GetAxisRaw("Horizontal");
        FlipSprite(MovX);
        transform.Translate(MovX * speed * Time.deltaTime, 0, 0);

        if (isGrounded)
        {
            lastGrounded = coyoteTime;
        }
        else
        {
            lastGrounded -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && lastGrounded > 0f && !pauseManager.isPaused)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            lastGrounded = 0f;
            audioManager.playJumping();
        }
    }

    void FlipSprite(float moveX)
    {
        if (moveX > 0)
        {
            Self.flipX = true; // mirando a la derecha
        }
        else if (moveX < 0)
        {
            Self.flipX = false;  // mirando a la izquierda
        }
    }

    private void OnCollisionEnter2D(Collision2D muncher)
    {
        if (muncher.collider.CompareTag("muncher"))
        {
            die();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Death Bar") || other.CompareTag("Laser"))
        {
            die();
        }
    }

    private void die() {
        pauseButton.SetActive(false);

        scoreManager.ShowDeathMessage(score);
            
        audioManager.stopMusic();
        audioManager.playGameOver();
        Destroy(this.gameObject);
    }


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

    public void playEnemy() {
        audioManager.playEnemy();
    }

}
