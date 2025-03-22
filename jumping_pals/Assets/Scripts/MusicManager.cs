using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio sources ----------")]
    [SerializeField] AudioSource MusicSource;

    [Header("---------- Audio Clips ----------")]
    public AudioClip intro;
    public AudioClip mainmenu_loop;

    void Start() {
        MusicSource.clip = intro;
        MusicSource.Play();
        // StartCoroutine(PlayMainMenuLoopAfterIntro());
    }

    void Update() {
        if (MusicSource.isPlaying == false) {
            MusicSource.clip = mainmenu_loop;
            MusicSource.Play();
        }
    }

    private System.Collections.IEnumerator PlayMainMenuLoopAfterIntro() {
        // Wait until the intro clip finishes
        yield return new WaitForSeconds(intro.length);

        // Switch to the looping track
        MusicSource.clip = mainmenu_loop;
        MusicSource.loop = true; // Ensure the loop is enabled
        MusicSource.Play();
    }
}
