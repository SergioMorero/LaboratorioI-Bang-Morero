using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    [System.Serializable]
    public class Entry
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class EntryList
    {
        public List<Entry> scores;
    }

    private TextMeshProUGUI leaderboardText;
    private string serverUrl = "https://jumping-pals.onrender.com";

    void Start()
    {
        leaderboardText = GetComponent<TextMeshProUGUI>();
        leaderboardText.text = "";
        StartCoroutine(FetchLeaderboard());
    }

    IEnumerator FetchLeaderboard()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl + "/leaderboard"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Flask devuelve una lista JSON la convertimos a un objeto compatible con Unity
                string json = "{\"scores\":" + request.downloadHandler.text + "}";
                Debug.Log(json);
                EntryList leaderboard = JsonUtility.FromJson<EntryList>(json);
                int rank = 1;
                foreach (var entry in leaderboard.scores)
                {
                    leaderboardText.text += $"{rank}. {entry.name} - {entry.score}\n";
                    rank++;
                }
            }
            else
            {
                leaderboardText.text = "Error al cargar leaderboard";
                Debug.LogError("Leaderboard request failed: " + request.error);
            }
        }
    }
}