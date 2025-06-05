using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDeletion : MonoBehaviour
{

    private Transform deathBarPosition;
    MainCamera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Death Bar").Length == 1)
        {
            deathBarPosition = GameObject.FindWithTag("Death Bar").transform;
        }
        else
        {
            if (transform.position.x < 0)
            {
                deathBarPosition = GameObject.Find("Bottom1").transform;
            }
            else
            {
                deathBarPosition = GameObject.Find("Bottom2").transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (deathBarPosition.position.y > transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }
}
