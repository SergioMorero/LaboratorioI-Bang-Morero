using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    public bool isPaused;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject pausePannel;
    [SerializeField] private GameObject player1Name = null;
    [SerializeField] private GameObject player2Name = null;

    void Start() {
        isPaused = false;
        if (pauseButton != null) {
            pauseButton.onClick.AddListener(Pause);
        }
        if (resumeButton != null) {
            resumeButton.onClick.AddListener(Resume);
        }
    }

    public void Pause() {
        isPaused = true;
        Time.timeScale = 0f;
        if (player1Name != null) player1Name.SetActive(false);
        if (player2Name != null) player2Name.SetActive(false);
        pausePannel.SetActive(true);
    }

    public void Resume() {
        isPaused = false;
        Time.timeScale = 1f;
        if (player1Name != null) player1Name.SetActive(true);
        if (player2Name != null) player2Name.SetActive(true);
        pausePannel.SetActive(false);
    }

}
