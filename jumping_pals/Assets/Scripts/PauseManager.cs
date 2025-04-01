using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    public bool isPaused;

    public Button pauseButton;
    public Button resumeButton;

    void Start() {
        isPaused = false;
        if (pauseButton != null) {
            pauseButton.onClick.AddListener(Pause);
        }
        if (resumeButton != null) {
            resumeButton.onClick.AddListener(Resume);
        }
    }

    void Pause() {
        isPaused = true;
        Time.timeScale = 0f;
    }

    void Resume() {
        isPaused = false;
        Time.timeScale = 1f;
    }

}
