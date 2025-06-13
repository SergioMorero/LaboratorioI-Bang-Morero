using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private GameObject deathBar;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        deathBar = GameObject.FindWithTag("Death Bar");
    }

    // Update is called once per frame
    void Update()
    {
        if (deathBar.transform.position.y > transform.position.y)
        {
            Destroy(this.gameObject);
        }
        if (isActive)
        {
            Invoke("desactivateLaser", 1);
        }
        else
        {
            Invoke("activateLaser", 2);
        }
    }
    
    void activateLaser()
    {
        isActive = true;
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    void desactivateLaser()
    {
        isActive = false;
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }
    
}
