using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using Unity.VisualScripting;
using TMPro;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class OnlineSpawner : NetworkBehaviour
{
    [Header("---------------------TimeManagement---------------------")]
    [SyncVar(hook = nameof(OnTimeChanged))]
    public float time;
    [SyncVar(hook = nameof(OnPauseStateChanged))]
    public bool isPaused = false;

    public float duration;
    private float remainingTime;
    public bool started = false;

    public TextMeshProUGUI timer1;
    public TextMeshProUGUI timer2;

    [Header("---------------------PointsManagement---------------------")]
    public int hostScore = 0;
    public int clientScore = 0;


    [Header("---------------------GenerationManagement---------------------")]
	public Transform normalPlatform;
	public Transform bigPlatform;
	public Transform movingPlatform;
    public Transform patrolEnemy;
    public Transform muncherEnemy;
    public Transform coin;


	[SyncVar]
	public int colorChosen;
    [SyncVar]
    public int chosenPlatform;
    [SyncVar]
    public int chosenEnemy;
    [SyncVar]
    public int chanceOfEnemy;
    [SyncVar]
    public int chanceOfCoin;
    [SyncVar]
    public int xPosition;
    [SyncVar]
    public int lastXPosition;

    public override void OnStartServer()
    {
        base.OnStartServer();
        colorChosen = Random.Range(0, 3);
        lastXPosition = -65;
    }

    public void Update()
    {
        if (isServer && started)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                time = remainingTime;
            }
            else
            {
                Debug.Log("GameEnded");
                isPaused = true;
                started = false;
            }
        }
    }

    public void addPoint(string player)
    {
        Debug.Log("punto dado");
        if (player == "host") hostScore += 1;
        else clientScore += 1;
    }



    [Server]
    public void startTimer()
    {
        remainingTime = duration;
        started = true;
    }

    public void OnTimeChanged(float oldTime, float newTime)
    {
        int seconds = Mathf.CeilToInt(time);
        timer1.text = seconds + "s.";
        timer2.text = seconds + "s.";
    }

    public void OnPauseStateChanged(bool olvValue, bool newValue)
    {
        if (newValue)
        {
            Time.timeScale = 0;
        }
    }


    [Server]
    public void spawnPlatform(string platformName, Vector3 position)
    {
        Transform platform;
        switch(platformName)
        {
            case "onlineBigPlatform":
                platform = bigPlatform;
                break;
            case "onlineMovingPlatform":
                platform = movingPlatform;
                break;
            default:
                platform = normalPlatform;
                break;
        }
        GameObject instantiatedPlatformLeft = Instantiate(platform, position, Quaternion.identity).gameObject;
        NetworkServer.Spawn(instantiatedPlatformLeft);
        GameObject instantiatedPlatformRight = Instantiate(platform, position + 110 * Vector3.right, Quaternion.identity).gameObject;
        NetworkServer.Spawn(instantiatedPlatformRight);
    }

    [Server]
    public void spawnCoin(Vector3 position)
    {
        GameObject instantiatedCoinLeft = Instantiate(coin, position, Quaternion.identity).gameObject;
        NetworkServer.Spawn(instantiatedCoinLeft);
        GameObject instantiatedCoinRight = Instantiate(coin, position + 110 * Vector3.right, Quaternion.identity).gameObject;
        NetworkServer.Spawn(instantiatedCoinRight);
    }

    [Server]
    public void spawnEnemy(string enemyName, Vector3 position)
    {
        Transform enemy = enemyName == "onlineMuncherEnemy" ? muncherEnemy : patrolEnemy;
        GameObject instantiatedEnemyLeft = Instantiate(enemy, position, Quaternion.identity).gameObject;
        NetworkServer.Spawn(instantiatedEnemyLeft);
        GameObject instantiatedEnemyRight = Instantiate(enemy, position + 110 * Vector3.right, Quaternion.identity).gameObject;
        NetworkServer.Spawn(instantiatedEnemyRight);
    }


    [Server]
    public int choosePlatform()
    {
        chosenPlatform = Random.Range(0, 9);
        return chosenPlatform;
    }


    [Server]
    public int setChanceOfEnemy()
    {
        chanceOfEnemy = Random.Range(0, 5);
        return chanceOfEnemy; 
    }

    [Server]
    public int chooseEnemy()
    {
        chosenEnemy = Random.Range(0, 4);
        return chosenEnemy;
    }

    [Server]
    public int setChanceOfCoin()
    {
        chanceOfCoin = Random.Range(0, 9);
        return chanceOfCoin;
    }

    [Server]
    public int chooseXPosition()
    {
        xPosition = Random.Range(0, 8);
        return xPosition;
    }

    [Server]
    public void setLastXPosition(int position)
    {
        lastXPosition = position;
    }
}
