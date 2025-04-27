using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class Achievements : MonoBehaviour
{

    [System.Serializable]
    public class Achievement
    {
        public int id;
        public string name;
        public string desc;
    }

    [System.Serializable]
    public class Wrapper
    {
        public Achievement[] array;
    }

    private int userID;
    private TextMeshProUGUI achievementsText;
    private string serverUrl = "http://localhost:5000";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        userID = PlayerPrefs.GetInt("ID");
        achievementsText = GetComponent<TextMeshProUGUI>();
        achievementsText.text = "";
        StartCoroutine(ShowAchievements());
    }

    IEnumerator ShowAchievements()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl + "/achievements/" + userID))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                // Lo 'wrappeo' para que Unity pueda leerlo bien
                string newJson = "{\"array\": " + json + "}";
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(newJson);
                Achievement[] achievements = wrapper.array;

                if (achievements.Length > 0)
                {
                    foreach (Achievement achv in achievements)
                    {
                        achievementsText.text += $"{achv.name}<size=8>-{achv.desc}</size>\n";
                    }
                }
                else
                {
                    achievementsText.text = "";
                }
                

            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }

        }
    }
    
}
