using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    private GameObject mc;
    private GameObject deathBar;
    private float prevHeigh;
    public bool isAlive = true; // Player is alive

    // Start is called before the first frame update
    void Start()
    {
        mc = GameObject.FindWithTag("Player");
        deathBar = GameObject.FindWithTag("Death Bar");
    }

    // Update is called once per frame
    void Update()
    {
        if (mc != null && isAlive)
        {
            prevHeigh = transform.position.y;
            if (transform.position.y < mc.transform.position.y + 1 && mc.transform.position.y > prevHeigh)
            {
                transform.position = new Vector3(transform.position.x, mc.transform.position.y, transform.position.z);
                deathBar.transform.position = new Vector3(transform.position.x, mc.transform.position.y - 17, transform.position.z);
            }
        }
    }

    public void stop() {
        isAlive = false; // Player has perished
    }

}
