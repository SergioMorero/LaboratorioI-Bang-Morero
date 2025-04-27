using UnityEngine;

public class ScreenLoader : MonoBehaviour
{

    bool started = false;

    public GameObject loadingScreen;
    public AudioManager audioManager;

    public void begin() {
        loadingScreen.SetActive(false);
        if (!started) { // First time
            audioManager.playMusic();
            started = true;
        }
    }

}
