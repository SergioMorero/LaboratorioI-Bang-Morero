using UnityEngine;

public class SFXManager : MonoBehaviour {

    [Header("---------- Audio sources ----------")]
    [SerializeField] AudioSource MusicSource;

    [Header("---------- Audio Clips ----------")]
    public AudioClip intro;
    public AudioClip loop;

    void Start() {
        if (intro != null) { // If there is an intro clip
            MusicSource.clip = intro;
            MusicSource.Play();
            StartCoroutine(WaitForIntroToEnd());
        } else { // Else play loop inmediately
            MusicSource.clip = loop;
            MusicSource.Play();
        }
    }

    private System.Collections.IEnumerator WaitForIntroToEnd() {
        // Wait until the intro finishes playing
        while (MusicSource.isPlaying) {
            yield return null; // Wait until next frame
        }

        // Play the loop immediately
        MusicSource.clip = loop;
        MusicSource.loop = true;
        MusicSource.Play();
    }
}