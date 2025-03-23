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
        StartCoroutine(WaitForIntroToEnd());
    }

    private System.Collections.IEnumerator WaitForIntroToEnd()
    {
        // Wait until the intro finishes playing
        while (MusicSource.isPlaying)
        {
            yield return null; // Wait until next frame
        }

        // Play the main menu loop immediately
        MusicSource.clip = mainmenu_loop;
        MusicSource.loop = true;
        MusicSource.Play();
    }
}
