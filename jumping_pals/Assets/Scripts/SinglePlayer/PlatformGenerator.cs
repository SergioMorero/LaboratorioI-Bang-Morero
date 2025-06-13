using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformGenerator : MonoBehaviour
{
    private MainCamera cameraScript;
    public GameObject player;
    public bool isSinglePlayer = true;
    protected bool playerGrounded;

    [SerializeField] protected GameObject tMap1;
    [SerializeField] protected GameObject tMap2;
    [SerializeField] protected GameObject tMap3;

    [SerializeField] protected GameObject plat1;
    [SerializeField] protected GameObject plat2;
    [SerializeField] protected GameObject plat3;
    [SerializeField] protected GameObject plat4;
    [SerializeField] protected GameObject plat5;

    [SerializeField] protected Sprite normalTile1;
    [SerializeField] protected Sprite normalTile2;
    [SerializeField] protected Sprite normalTile3;

    [SerializeField] protected Sprite bigTile1;
    [SerializeField] protected Sprite bigTile2;
    [SerializeField] protected Sprite bigTile3;

    [SerializeField] protected Transform platform1;
    [SerializeField] protected Transform platform2;
    [SerializeField] protected Transform movingPlatform1;
    [SerializeField] protected Transform icePlatform;
    [SerializeField] protected Transform jumpPlatform;
    [SerializeField] protected Transform slimePlatform;

    [SerializeField] protected Transform patrolEnemy;
    [SerializeField] protected Transform muncherEnemy;

    [SerializeField] protected Transform coin;

    [SerializeField] protected Transform laser;
    
    public float lastPlatformHeigh;
    public float lastXPosition;
    public float distanceOfGeneration;

    [SerializeField] private AudioManager audioManager;
    protected int colorChosen;
    public float lastPlayerGroundedHeigh;
    private Movement singlePlayerScript;
    private LocalPlayer localGamePlayerScript;
    protected GameObject[] tileMaps = new GameObject[3];
    protected Sprite[] normalPlatformTiles = new Sprite[3];
    protected Sprite[] bigPlatformTiles = new Sprite[3];
    protected int[] coinPositionDeviation = new int[3];
    protected float[] horizontalValues = new float[8];
    protected float[] verticalValues = new float[3];
    protected Transform[] platforms = new Transform[12];
    protected Transform[] enemies = new Transform[4];

    // Start is called before the first frame update
    void Start()
    {

        cameraScript = Camera.main.GetComponent<MainCamera>();
        player = cameraScript.highestPlayer;
        if (player.GetComponent<Movement>() != null)
        {
            singlePlayerScript = player.GetComponent<Movement>();
        }
        else
        {
            // Local Game
            isSinglePlayer = false;
        }

        // Elecci�n de color muy ineficaz, se optimizar� m�s adelante
        colorChosen = SelectMap();

        // Already placed platforms
        plat1.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat2.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat3.GetComponent<SpriteRenderer>().sprite = bigPlatformTiles[colorChosen];
        // plat4.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        plat5.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];

        // Platform prefabs
        platform1.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];
        platform2.GetComponent<SpriteRenderer>().sprite = bigPlatformTiles[colorChosen];
        // movingPlatform1.GetComponent<SpriteRenderer>().sprite = normalPlatformTiles[colorChosen];

        coinPositionDeviation[0] = -2;
        coinPositionDeviation[1] = 0;
        coinPositionDeviation[2] = 2;

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
        platforms[9] = icePlatform;
        platforms[10] = slimePlatform;
        platforms[11] = jumpPlatform;

        enemies[0] = patrolEnemy;
        enemies[1] = patrolEnemy;
        enemies[2] = patrolEnemy;
        enemies[3] = muncherEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSinglePlayer)
        {
            player = cameraScript.highestPlayer;
        }


        if (player != null)
        {
            if (isSinglePlayer)
            {
                playerGrounded = singlePlayerScript.isGrounded;
            }
            else
            {
                localGamePlayerScript = player.GetComponent<LocalPlayer>();
                playerGrounded = localGamePlayerScript.isGrounded;
            }

            if (playerGrounded && player.transform.position.y > lastPlayerGroundedHeigh + 3 && Mathf.Abs(lastPlatformHeigh - player.transform.position.y) < distanceOfGeneration)
            {
                GeneratePlatform();
                GeneratePlatform();
            }


            if (playerGrounded)
            {
                lastPlayerGroundedHeigh = player.transform.position.y;
            }
        }
    }

    void GeneratePlatform()
    {
        int chanceOfLaser = Random.Range(0, 9);
        int chanceOfEnemy = Random.Range(0, 5);
        int chanceOfCoin = Random.Range(0, 9);
        GameObject lastLaser = GameObject.FindWithTag("Laser");
        float heighIncrease = verticalValues[Random.Range(0, 3)];
        float xPosition = horizontalValues[Random.Range(0, 8)];
        int coinDeviation = coinPositionDeviation[Random.Range(0, 3)];
        while (Mathf.Abs(xPosition - lastXPosition) > 19)
        {
            xPosition = horizontalValues[Random.Range(0, 8)];
        }
        Transform chosenPlatform = platforms[Random.Range(0, platforms.Length)];
        Transform chosenEnemy = enemies[Random.Range(0, 4)];
        Instantiate(chosenPlatform, new Vector3(xPosition, lastPlatformHeigh + heighIncrease, 0), Quaternion.identity);
        lastXPosition = xPosition;
        lastPlatformHeigh += heighIncrease;

        if (chanceOfCoin == 5)
        {
            Instantiate(coin, new Vector3(xPosition + coinDeviation, lastPlatformHeigh + 3, 0), Quaternion.identity);
        }
        if (chanceOfEnemy == 3 && chosenPlatform != movingPlatform1)
        {
            Instantiate(chosenEnemy, new Vector3(xPosition, lastPlatformHeigh + 1, 0), Quaternion.identity);
        }
        if (chanceOfLaser == 5 && lastLaser == null)
        {
            Instantiate(laser, new Vector3(0, lastPlatformHeigh + 2, 0), Quaternion.identity);
        }
    }

    int SelectMap()
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

        int chosen = Random.Range(0, 3);
        switch (chosen)
        {
            case 0:
                tMap1.SetActive(true);
                tMap2.SetActive(false);
                tMap3.SetActive(false);
                break;
            case 1:
                tMap2.SetActive(true);
                tMap1.SetActive(false);
                tMap3.SetActive(false);
                break;
            case 2:
                tMap3.SetActive(true);
                tMap1.SetActive(false);
                tMap2.SetActive(false);
                break;
        }

        return chosen;
    }
}
