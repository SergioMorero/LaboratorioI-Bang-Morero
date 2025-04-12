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
    [SerializeField] private bool isLogged;

    [Header("----- Objects -----")]
    [SerializeField] private GameObject DeathMessage;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text ScoreBestText;
    [SerializeField] private TMP_Text NewBestText;
    [SerializeField] private TMP_Text ScoreDisplay;

    private string serverUrl = "http://localhost:5000";

    void Start()
    {
        loadPrefs();
        NewBestText.text = "";
    }

    public void loadPrefs()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            userName = PlayerPrefs.GetString("Username");
            userPassword = PlayerPrefs.GetString("Password");
            userID = PlayerPrefs.GetInt("ID");
            userMaxScore = PlayerPrefs.GetInt("Score");
            userCoins = PlayerPrefs.GetInt("Coins");
            isLogged = true;
        }
        else
        {
            isLogged = false;
        }
    }

    // UI

    public void ShowDeathMessage(int score)
    {
        Destroy(ScoreDisplay);
        DeathMessage.SetActive(true);

        if (isLogged && score > userMaxScore)
        {
            NewBestText.text = "New Best!";
            userMaxScore = score;
            SendScore(score);
        }

        /*
         Displaying text after the query will automatically update the best score to the
         new best score if needed without having to add more conditions
         If not, best score will record the previous best score
         */
        ScoreBestText.text = userMaxScore.ToString();
        ScoreText.text = score.ToString();

    }
    
    // SQL

    private void SendScore(int score)
    {
            StartCoroutine(sendScoreCoroutine(userID, score));
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

    [System.Serializable]
    public class UserScore
    {
        public int id;
        public int score;
    }
}