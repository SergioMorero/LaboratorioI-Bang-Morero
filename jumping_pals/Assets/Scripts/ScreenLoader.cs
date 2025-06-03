using UnityEngine;

public class ScreenLoader : MonoBehaviour
{

    bool started = false;

    public GameObject loadingScreen;
    public AudioManager audioManager;
    // This is a panel that appears on top of everything, blocking all possible input during fetches
    public GameObject loadingPanel;

    public void begin() {
        loadingScreen.SetActive(false);
        if (!started) { // First time
            audioManager.playMusic();
            started = true;
        }
    }

    public void load() {
        loadingPanle.setActive(true);
    }

    public void stop() {
        loadingPanel.SetActive(false);
    }
}
