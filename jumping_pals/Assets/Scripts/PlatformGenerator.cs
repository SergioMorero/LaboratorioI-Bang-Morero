using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    private GameObject player;
    [SerializeField] private Transform platform1;
    [SerializeField] private Transform platform2;
    [SerializeField] private Transform movingPlatform1;

    [SerializeField] private Transform patrolEnemy;
    [SerializeField] private Transform muncherEnemy;

    [SerializeField] private Transform laser;
    private float lastPlatformHeigh = 15;

    [SerializeField] private AudioManager audioManager;
    private float lastPlayerGroundedHeigh;
    private Movement playerMovement;
    private float[] horizontalValues = new float[8];
    private float[] verticalValues = new float[3];
    private Transform[] platforms = new Transform[9];
    private Transform[] enemies = new Transform[4];
    private float lastXPosition = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        horizontalValues[0] = -13;
        horizontalValues[1] = -9;
        horizontalValues[2] = -6;
        horizontalValues[3] = 0;
        horizontalValues[4] = 5;
        horizontalValues[5] = 7;
        horizontalValues[6] = 8.5f;
        horizontalValues[7] = 10;

        verticalValues[0] = 5;
        verticalValues[1] = 6;
        verticalValues[2] = 6;

        platforms[0] = platform1;
        platforms[1] = platform1;
        platforms[2] = platform1;
        platforms[3] = platform1;
        platforms[4] = platform1;
        platforms[5] = platform1;
        platforms[6] = platform2;
        platforms[7] = platform2;
        platforms[8] = movingPlatform1;

        enemies[0] = patrolEnemy;
        enemies[1] = patrolEnemy;
        enemies[2] = patrolEnemy;
        enemies[3] = muncherEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (playerMovement.isGrounded && player.transform.position.y > lastPlayerGroundedHeigh + 3 && Mathf.Abs(lastPlatformHeigh - player.transform.position.y) < 20)
            {
                GeneratePlatform();
                GeneratePlatform();
            }


            if (playerMovement.isGrounded)
            {
                lastPlayerGroundedHeigh = player.transform.position.y;
            }
        }
    }

    void GeneratePlatform()
    {
        int chanceOfLaser = Random.Range(0, 15);
        int chanceOfEnemy = Random.Range(0, 5);
        float heighIncrease = verticalValues[Random.Range(0, 3)];
        float xPosition = horizontalValues[Random.Range(0, 8)];
        while (Mathf.Abs(xPosition - lastXPosition) > 19)
        {
            xPosition = horizontalValues[Random.Range(0, 8)];
        }
        Transform chosenPlatform = platforms[Random.Range(0, 9)];
        Transform chosenEnemy = enemies[Random.Range(0, 4)];
        Instantiate(chosenPlatform, new Vector3(xPosition, lastPlatformHeigh + heighIncrease, 0), Quaternion.identity);
        lastXPosition = xPosition;
        lastPlatformHeigh += heighIncrease;

        if (chanceOfEnemy == 3 && chosenPlatform != movingPlatform1)
        {
            Instantiate(chosenEnemy, new Vector3(xPosition, lastPlatformHeigh + 1, 0), Quaternion.identity);
        }
        if (chanceOfLaser == 5)
        {
            Instantiate(laser, new Vector3(0, lastPlatformHeigh + 2, 0), Quaternion.identity);
        }
    }

}
