using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{

    private RaycastHit2D hitFloor;
    private RaycastHit2D hitFloorLeft;
    private RaycastHit2D hitFloorRight;
    private Rigidbody2D rb;

    public LayerMask floor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        transform.Translate(new Vector3(0.03f * transform.localScale.x, 0, 0));
    }
}
