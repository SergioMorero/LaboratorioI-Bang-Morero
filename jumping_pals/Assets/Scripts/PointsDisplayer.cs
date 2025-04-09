using TMPro;
using UnityEngine;

public class PointsDisplayer : MonoBehaviour
{

    private Movement playerScript;
    private TextMeshProUGUI textMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Movement>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = playerScript.score.ToString();
    }
}
