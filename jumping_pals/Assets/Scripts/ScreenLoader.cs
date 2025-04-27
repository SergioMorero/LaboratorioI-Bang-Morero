using UnityEngine;

public class ScreenLoader : MonoBehaviour
{

    public GameObject loadingScreen;
    public AudioManager audioManager;

    public void begin() {
        loadingScreen.SetActive(false);
        audioManager.playMusic();
    }

}
