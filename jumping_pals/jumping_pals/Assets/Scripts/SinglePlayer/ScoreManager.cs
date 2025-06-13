using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ScoreManager : MonoBehaviour
{

    [Header("----- Control -----")]
    [SerializeField] private int userID;
    [SerializeField] private string userName;
    [SerializeField] private string userPassword;
    [SerializeField] private int userMaxScore;
    [SerializeField] private int userCoins;

    [Header("----- Objects -----")]
    [SerializeField] private GameObject DeathMessage;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private GameObject bestScore;
    [SerializeField] private TMP_Text ScoreBestText;
    [SerializeField] private TMP_Text NewBestAnnouncer;
    [SerializeField] private TMP_Text ScoreDisplay;
    public AudioManager audioManager;

    private string serverUrl = "https://jumping-pals.onrender.com";

    void Start()
    {
        loadPrefs();
        NewBestAnnouncer.text = "";
    }

    public void loadPrefs()
    {
        userName = PlayerPrefs.GetString("Username");
        userPassword = PlayerPrefs.GetString("Password");
        userID = PlayerPrefs.GetInt("ID");
        userMaxScore = PlayerPrefs.GetInt("Score");
        userCoins = PlayerPrefs.GetInt("Coins");
    }

    // UI

    public void ShowDeathMessage(int score, int jumps, int enemiesKilled)
    {
        Destroy(ScoreDisplay);
        DeathMessage.SetActive(true);
        Debug.Log(userMaxScore);
        if (score > userMaxScore)
        {
            NewBestAnnouncer.text = "New Best!";
            userMaxScore = score;
            SendScore(score);
        /*
         Displaying text after the query will automatically update the best score to the
         new best score if needed without having to add more conditions
         If not, best score will record the previous best score
         */
        }
        SendStats(jumps, enemiesKilled);
        CheckAchievements();

        ScoreBestText.text = userMaxScore.ToString();
        ScoreText.text = score.ToString();

    }
    
    // SQL

    private void SendScore(int score)
    {
       StartCoroutine(sendScoreCoroutine(userID, score));
    }

    public void GiveCoin()
    {
        audioManager.playGetCoin();
        StartCoroutine(sendCoin(userID));
    }

    private void SendStats(int jumps, int enemiesKilled)
    {
        StartCoroutine(sendStats(userID, jumps, enemiesKilled));
    }

    private void CheckAchievements()
    {
        StartCoroutine(checkAchievements(userID));
    }

    IEnumerator sendScoreCoroutine(int id, int score)
    {
        string route = "/set-score";

        UserScore data = new UserScore { id = userID, score = score };
        string json = JsonUtility.ToJson(data);
        Debug.Log("JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "PUT");
        Debug.Log("Created request");
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Set request header");

        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(raw);
        request.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("JSON: " + json);

        yield return request.SendWebRequest();

        Debug.Log("Request sent");

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("Puntaje actualizado");
            PlayerPrefs.SetInt("Score", score);
        } else {
            Debug.Log("Error en la actialización: " + request.error);
        }
    }

    IEnumerator sendCoin(int id) // Give 1 coin
    {
        string route = "/give-coin";

        UserID data = new UserID { id = userID };
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "PUT");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(raw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            PlayerPrefs.SetInt("Coins", userCoins + 1);
        } else {
            Debug.Log("Error: " + request.error);
        }
    }

    IEnumerator sendStats(int id, int jumps, int enemiesKilled)
    {
        string route = "/set-stats";
        UserStats data = new UserStats { id = userID, jumps = jumps, enemiesKilled = enemiesKilled };
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "PUT");
        request.SetRequestHeader("Content-Type", "application/json");
        
        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler= new UploadHandlerRaw(raw);
        request.downloadHandler= new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Stats loaded succesfully - Jumps: " + jumps + ", EnemiesKilled: " + enemiesKilled);
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }

    }

    IEnumerator checkAchievements(int id)
    {
        string route = "/achievements";
        UserID data = new UserID { id = userID };
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(serverUrl + route, "PUT");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] raw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(raw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Achievements checked succesfully");
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }

    [System.Serializable]
    public class UserScore
    {
        public int id;
        public int score;
    }

    [System.Serializable]
    public class UserID
    {
        public int id;
    }

    [System.Serializable]
    public class UserCoins
    {
        public int id;
        public int coins;
    }

    [System.Serializable]
    public class UserStats
    {
        public int id;
        public int jumps;
        public int enemiesKilled;
    }
}