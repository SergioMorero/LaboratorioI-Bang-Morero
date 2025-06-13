using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.CouchCoop;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class OnlinePlayerMovement : NetworkBehaviour
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

    public bool alive = true;

    public float speed = 15;
    public float jumpForce;
    [SerializeField] private LayerMask floor;
    [SerializeField] private LayerMask enemy;
    public LayerMask bottom;

    public GameObject camera;
    public FollowPlayer cameraScript;
    

    // public MainCamera camera;
    private SpriteRenderer sprite;


    [Header("--- Platform Generator ---")]
    public OnlinePlatformGenerator clientPlayer;

    public OnlineSpawner spawnManager;

    public Transform normalPlatform;
    public Transform bigPlatform;
    public Transform movingPlatform;

    public Transform patrolEnemy;
    public Transform muncherEnemy;

    public Transform coin;

    public float lastPlayerGroundedHeigh;
    public int lastPlatformHeigh;
    public int distanceOfGeneration;
    public int heighIncrease = 6;

    private int[] horizontalValues = new int[8];
    private Transform[] platforms = new Transform[9];
    private Transform[] enemies = new Transform[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // camera = transform.position.x < 0 ? GameObject.Find("Camera1") : GameObject.Find("Camera2");
        // cameraScript = camera.GetComponent<FollowPlayer>();
        // cameraScript.targetPlayer = this.gameObject;
        
        rb = GetComponent<Rigidbody2D>();
        isGrounded = false;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        spawnManager = GameObject.Find("OnlineSpawner").GetComponent<OnlineSpawner>();

        horizontalValues[0] = -70;
        horizontalValues[1] = -65;
        horizontalValues[2] = -60;
        horizontalValues[3] = -60;
        horizontalValues[4] = -55;
        horizontalValues[5] = -55;
        horizontalValues[6] = -50;
        horizontalValues[7] = -45;


        platforms[0] = normalPlatform;
        platforms[1] = normalPlatform;
        platforms[2] = normalPlatform;
        platforms[3] = normalPlatform;
        platforms[4] = normalPlatform;
        platforms[5] = normalPlatform;
        platforms[6] = bigPlatform;
        platforms[7] = bigPlatform;
        platforms[8] = movingPlatform;

        enemies[0] = patrolEnemy;
        enemies[1] = patrolEnemy;
        enemies[2] = patrolEnemy;
        enemies[3] = muncherEnemy;
    }


    public void setClientPlayer()
    {
        if (GameObject.Find("PlatformGenerator2") != null)
        {
            clientPlayer = GameObject.Find("PlatformGenerator2").GetComponent<OnlinePlatformGenerator>();
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isLocalPlayer) { return; }

        setClientPlayer();

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
            hitFloor = Physics2D.Raycast(transform.position + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
            hitFloorLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
            hitFloorRight = Physics2D.Raycast(transform.position + Vector3.right * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);

            isGrounded = (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorRight.collider != null);
            //animator.SetBool("IsJumping", !isGrounded);

            MovX = Input.GetAxisRaw("Horizontal");
            FlipSprite(MovX);
            transform.Translate(MovX * speed * Time.deltaTime, 0, 0);
            //animator.SetFloat("Xvelocity", (float)Math.Abs(Math.Round(MovX)));
            //animator.SetFloat("Yvelocity", (float)rb.linearVelocity.y);

            if (isGrounded)
            {
                lastGrounded = coyoteTime;
            }
            else
            {
                lastGrounded -= Time.deltaTime;
            }

            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && lastGrounded > 0f)
            {
                //test();

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                lastGrounded = 0f;

                //animator.SetTrigger("Jump");
            }


        } else { // Player is death
            if (transform.position.y < -14) {
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        //PlatformGenerating
        if (isServer)
        {
            if (clientPlayer != null)
            {
                if (clientPlayer.transform.position.y >= transform.position.y + 1)
                {
                    if (clientPlayer.playerIsGrounded && clientPlayer.transform.position.y > clientPlayer.lastPlayerGroundedHeigh + 3 && Mathf.Abs(lastPlatformHeigh - clientPlayer.transform.position.y) < distanceOfGeneration)
                    {
                        Debug.Log("Client generates");
                        StartCoroutine(GeneratePlatformCoroutine());
                    }
                }
                else if (isGrounded && transform.position.y > lastPlayerGroundedHeigh + 3 && Mathf.Abs(lastPlatformHeigh - transform.position.y) < distanceOfGeneration)
                {
                    Debug.Log("Host generates");
                    StartCoroutine(GeneratePlatformCoroutine());
                }
            }
        }
            
        if (isGrounded)
        {
            lastPlayerGroundedHeigh = transform.position.y;
        }

        if (clientPlayer != null)
        {
            if (clientPlayer.playerIsGrounded)
            {
                clientPlayer.lastPlayerGroundedHeigh = clientPlayer.transform.position.y;
            }
        }
        

        

    }

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

    [Command(requiresAuthority = false)]
    public void spawnPlatform(string platformName, Vector3 position)
    {
        spawnManager.spawnPlatform(platformName, position);
    }

    [Command(requiresAuthority = false)]
    public void spawnCoin(Vector3 position)
    {
        spawnManager.spawnCoin(position);
    }

    [Command(requiresAuthority = false)]
    public void spawnEnemy(string enemyName, Vector3 position)
    {
        spawnManager.spawnEnemy(enemyName, position);
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

    // Death protocol
    private void die()
    {
        alive = false;
        //cameraScript.isAlive = false;

        // pauseButton.SetActive(false);

        // scoreManager.ShowDeathMessage(score, jumps, enemiesKilled);

        // audioManager.stopMusic();
        // audioManager.playGameOver();

        animator.SetTrigger("Death");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        // cameraScript.stop();
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

    IEnumerator GeneratePlatformCoroutine()
    {   
        GeneratePlatform();
        GeneratePlatform();

        yield return null;
    }

    [Command(requiresAuthority = false)]
    public void GeneratePlatform()
    {
        string chosenPlatform = platforms[spawnManager.choosePlatform()].name;
        int xPosition = horizontalValues[spawnManager.chooseXPosition()];
        int chanceOfEnemy = spawnManager.setChanceOfEnemy();
        string chosenEnemy = enemies[spawnManager.chooseEnemy()].name;
        int chanceOfCoin = spawnManager.setChanceOfCoin();
        
        while (Mathf.Abs(xPosition - spawnManager.lastXPosition) > 17)
        {
            xPosition = horizontalValues[spawnManager.chooseXPosition()];
        }
        
        spawnManager.spawnPlatform(chosenPlatform, new Vector3(xPosition, lastPlatformHeigh + heighIncrease, 0));
        lastPlatformHeigh += heighIncrease;
        if (chanceOfCoin == 5)
        {
            spawnManager.spawnCoin(new Vector3(xPosition, lastPlatformHeigh + 3, 0));
        }
        if (chanceOfEnemy == 3 && chosenPlatform != "onlineMovingPlatform")
        {
            spawnManager.spawnEnemy(chosenEnemy, new Vector3(xPosition, lastPlatformHeigh + 1, 0));
        }
        spawnManager.setLastXPosition(xPosition);
    }
}
