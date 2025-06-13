using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Rendering;
using UnityEngine;

public class EnemyMuncher : MonoBehaviour
{

    [Header("---------- Varialbes ----------")]

    [SerializeField] private float speed;
    private RaycastHit2D hitFloor;
    private RaycastHit2D hitFloorLeft;
    private RaycastHit2D hitFloorRight;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask playerLayer;
    private GameObject player;
    private Rigidbody2D playerRB;
    private RaycastHit2D hitHead;
    private RaycastHit2D hitHeadLeft;
    private RaycastHit2D hitHeadRight;

    public bool gotHit;

    public LayerMask floor;

    private Movement audioManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        audioManager = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Attacked
        hitHead = Physics2D.Raycast(transform.position, Vector2.up, 1.05f, playerLayer);
        hitHeadLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.6f, Vector2.up, 1.05f, playerLayer);
        hitHeadRight = Physics2D.Raycast(transform.position + Vector3.right * 0.6f, Vector2.up, 1.05f, playerLayer);

        gotHit = (hitHead.collider != null || hitHeadLeft.collider != null || hitHeadRight.collider != null);
        if (gotHit)
        {
            playerRB.linearVelocity -= new Vector2(0, playerRB.linearVelocity.y);
            playerRB.AddForce(new Vector2(-2 * playerRB.linearVelocity.x, 20), ForceMode2D.Impulse);
            Destroy(this.gameObject);
            audioManager.playEnemy();
        }
         */

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.up * 1.05f);
        Gizmos.DrawRay(transform.position + Vector3.left * 0.6f, Vector3.up * 1.05f);
        Gizmos.DrawRay(transform.position + Vector3.right * 0.6f, Vector3.up * 1.05f);
    }
}
