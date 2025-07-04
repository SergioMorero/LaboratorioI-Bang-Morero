using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Rendering;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private Transform coin;

    [Header("---------- Varialbes ----------")]

    [SerializeField] private float speed = 2.75f;
    private RaycastHit2D hitFloor;
    private RaycastHit2D hitFloorLeft;
    private RaycastHit2D hitFloorRight;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private LayerMask playerLayer;
    private GameObject player;
    private Rigidbody2D playerRB;
    private Rigidbody2D localPlayerRB;
    private RaycastHit2D hitHead;
    private RaycastHit2D hitHeadLeft;
    private RaycastHit2D hitHeadRight;

    public bool gotHit;
    private bool isSinglePlayer;
    public bool isMultiplayer;

    public LayerMask floor;

    private Movement playerMovement;
    private ScoreManager scoreManager;
    private LocalScoreManager localScoreManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        GameObject pG = GameObject.Find("PlatformGenerator");
        isMultiplayer = pG == null;
        isSinglePlayer = (pG != null) ? pG.GetComponent<PlatformGenerator>().isSinglePlayer : false;
        playerMovement = (isSinglePlayer) ? player.GetComponent<Movement>() : null;
        scoreManager = (isSinglePlayer) ? player.GetComponent<ScoreManager>() : null;
        localScoreManager = (!isSinglePlayer && pG != null) ? GameObject.FindWithTag("ScoreManager").GetComponent<LocalScoreManager>() : null;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gotHit) {
            //Attacked
            hitHead = Physics2D.Raycast(transform.position, Vector2.up, 1.05f, playerLayer);
            hitHeadLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.6f, Vector2.up, 1.05f, playerLayer);
            hitHeadRight = Physics2D.Raycast(transform.position + Vector3.right * 0.6f, Vector2.up, 1.05f, playerLayer);

            gotHit = (hitHead.collider != null || hitHeadLeft.collider != null || hitHeadRight.collider != null);
            
            if (gotHit) {
                die();
            }

            // Movement
            hitFloor = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, floor);
            hitFloorLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.5f, Vector2.down, 1.5f, floor);
            hitFloorRight = Physics2D.Raycast(transform.position + Vector3.right * 0.5f, Vector2.down, 1.5f, floor);

            if (transform.localScale.x > 0)
            {
                if (hitFloorRight.collider == null)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            else
            {
                if (hitFloorLeft.collider == null)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            transform.Translate(new Vector3(speed * transform.localScale.x * Time.deltaTime, 0, 0));
        }

        if (transform.position.y < -14) {
            Destroy(this.gameObject);
        }

    }

    private void die() {
        GetComponent<BoxCollider2D>().enabled = false;
        if (!isMultiplayer) Instantiate(coin, transform.position, Quaternion.identity);

        if (isSinglePlayer)
        {
            playerMovement.enemiesKilled += 1;
            playerMovement.playEnemy();
            playerRB.linearVelocity -= new Vector2(0, playerRB.linearVelocity.y);
            playerRB.AddForce(new Vector2(-2 * playerRB.linearVelocity.x, 20), ForceMode2D.Impulse);
        }
        else if (localScoreManager != null)
        {
            localScoreManager.enemiesKilled += 1;
            localScoreManager.playEnemy();
            if (hitHeadLeft.collider != null)
            {
                localPlayerRB = hitHeadLeft.collider.gameObject.GetComponent<Rigidbody2D>();
            }
            else if (hitHead.collider != null)
            {
                localPlayerRB = hitHead.collider.gameObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                localPlayerRB = hitHeadRight.collider.gameObject.GetComponent<Rigidbody2D>();
            }
            localPlayerRB.linearVelocity -= new Vector2(0, localPlayerRB.linearVelocity.y);
            localPlayerRB.AddForce(new Vector2(-2 * localPlayerRB.linearVelocity.x, 20), ForceMode2D.Impulse);
        }
        else
        {
            Rigidbody2D multiplayerRB;
            if (hitHeadLeft.collider != null)
            {
                multiplayerRB = hitHeadLeft.collider.gameObject.GetComponent<Rigidbody2D>();
            }
            else if (hitHead.collider != null)
            {
                multiplayerRB = hitHead.collider.gameObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                multiplayerRB = hitHeadRight.collider.gameObject.GetComponent<Rigidbody2D>();
            }
            multiplayerRB.linearVelocity -= new Vector2(0, multiplayerRB.linearVelocity.y);
            multiplayerRB.AddForce(new Vector2(-2 * multiplayerRB.linearVelocity.x, 20), ForceMode2D.Impulse);

        }
        Destroy(gameObject);


        /*
        animator.SetTrigger("GotHit");
        speed = 0;
        rb.AddForce(Vector3.up * 15, ForceMode2D.Impulse);
         */
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.up * 1.05f);
        Gizmos.DrawRay(transform.position + Vector3.left * 0.6f, Vector3.up * 1.05f);
        Gizmos.DrawRay(transform.position + Vector3.right * 0.6f, Vector3.up * 1.05f);
    }
}
