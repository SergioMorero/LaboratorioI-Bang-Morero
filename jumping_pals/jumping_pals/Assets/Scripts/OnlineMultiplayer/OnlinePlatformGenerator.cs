using Mirror.Examples.CouchCoop;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OnlinePlatformGenerator : PlatformGenerator
{
    public OnlinePlayerMovement playerScript;
    public OnlineSpawner spawnManager;
    public FollowPlayer targetCamera;

    public GameObject otherTMap1;
    public GameObject otherTMap2;
    public GameObject otherTMap3;

    public GameObject plat6;
    public GameObject plat7;
    public GameObject plat8;

    public GameObject otherPlat1;
    public GameObject otherPlat2;
    public GameObject otherPlat3;
    public GameObject otherPlat4;
    public GameObject otherPlat5;
    public GameObject otherPlat6;
    public GameObject otherPlat7;
    public GameObject otherPlat8;

    public int heighIncrease = 6;

    [SerializeField] private LayerMask floor;
    private RaycastHit2D hitFloor;
    private RaycastHit2D hitFloorLeft;
    private RaycastHit2D hitFloorRight;
    public bool playerIsGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = FindFirstObjectByType<OnlineSpawner>();
        tileMaps = new GameObject[3];
        normalPlatformTiles = new Sprite[3];
        bigPlatformTiles = new Sprite[3];
        coinPositionDeviation = new int[3];
        horizontalValues = new float[8];
        verticalValues = new float[3];
        platforms = new Transform[9];
        enemies = new Transform[4];

        colorChosen = spawnManager.colorChosen;
        SelectMap();


        plat1.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat2.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat3.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat4.GetComponent<SpriteRenderer>().sprite = bigPlatformTiles[colorChosen];
        plat5.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat6.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat7.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat8.GetComponent<SpriteRenderer>().sprite = bigPlatformTiles[colorChosen];

        otherPlat1.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        otherPlat2.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        otherPlat3.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        otherPlat4.GetComponent<SpriteRenderer>().sprite = bigPlatformTiles[colorChosen];
        otherPlat5.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        otherPlat6.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        otherPlat7.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        otherPlat8.GetComponent<SpriteRenderer>().sprite = bigPlatformTiles[colorChosen];


        platform1.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        platform2.GetComponent<SpriteRenderer>().sprite = bigPlatformTiles[colorChosen];
        movingPlatform1.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];


        horizontalValues[0] = -70;
        horizontalValues[1] = -65;
        horizontalValues[2] = -60;
        horizontalValues[3] = -60;
        horizontalValues[4] = -55;
        horizontalValues[5] = -55;
        horizontalValues[6] = -50;
        horizontalValues[7] = -45;


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

    public void OnEnable()
    {
        player = targetCamera.targetPlayer;
        playerScript = player.GetComponent<OnlinePlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        transform.position = player.transform.position;

        hitFloor = Physics2D.Raycast(transform.position + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
        hitFloorLeft = Physics2D.Raycast(transform.position + Vector3.left * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
        hitFloorRight = Physics2D.Raycast(transform.position + Vector3.right * 0.8f + Vector3.down * 0.75f, Vector2.down, 0.75f, floor);
        playerIsGrounded = (hitFloor.collider != null || hitFloorLeft.collider != null || hitFloorRight.collider != null);


    }
    

    void SelectMap()
    {
        tileMaps[0] = tMap1;
        tileMaps[1] = tMap2;
        tileMaps[2] = tMap3;

        normalPlatformTiles[0] = normalTile1;
        normalPlatformTiles[1] = normalTile2;
        normalPlatformTiles[2] = normalTile3;

        bigPlatformTiles[0] = bigTile1;
        bigPlatformTiles[1] = bigTile2;
        bigPlatformTiles[2] = bigTile3;

        switch (colorChosen)
        {
            case 0:
                tMap1.SetActive(true);
                tMap2.SetActive(false);
                tMap3.SetActive(false);
                otherTMap1.SetActive(true);
                otherTMap2.SetActive(false);
                otherTMap3.SetActive(false);
                break;
            case 1:
                tMap2.SetActive(true);
                tMap1.SetActive(false);
                tMap3.SetActive(false);
                otherTMap1.SetActive(false);
                otherTMap2.SetActive(true);
                otherTMap3.SetActive(false);
                break;
            case 2:
                tMap3.SetActive(true);
                tMap1.SetActive(false);
                tMap2.SetActive(false);
                otherTMap1.SetActive(false);
                otherTMap2.SetActive(false);
                otherTMap3.SetActive(true);
                break;
        }
        return;
    }
}
