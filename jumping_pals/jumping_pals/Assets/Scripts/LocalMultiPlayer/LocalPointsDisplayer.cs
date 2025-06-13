using TMPro;
using UnityEngine;

public class LocalPointsDisplayer : MonoBehaviour
{
    private LocalScoreManager scoreScript;
    private TextMeshProUGUI textMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreScript = GameObject.FindWithTag("ScoreManager").GetComponent<LocalScoreManager>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = scoreScript.score.ToString();
    }
}
