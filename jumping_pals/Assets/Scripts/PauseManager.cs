using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    public bool isPaused;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject pausePannel;

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
        pausePannel.SetActive(true);
    }

    public void Resume() {
        isPaused = false;
        Time.timeScale = 1f;
        pausePannel.SetActive(false);
    }

}
