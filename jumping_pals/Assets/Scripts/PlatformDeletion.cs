using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDeletion : MonoBehaviour
{

    private Transform deathBarPosition;

    // Start is called before the first frame update
    void Start()
    {
        deathBarPosition = GameObject.FindWithTag("Death Bar").transform;
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
