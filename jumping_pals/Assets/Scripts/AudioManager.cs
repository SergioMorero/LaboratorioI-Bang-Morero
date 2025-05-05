using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;

public class AudioManager : MonoBehaviour {

    [Header("---------- Audio Sources ----------")]
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource DeadSoundSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Controllers ----------")]
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;

    [Header("---------- Music Clips ----------")]
    public AudioClip intro;
    public AudioClip loop;
    public bool hasIntro;

    [Header("---------- SFX Clips ----------")]
    public AudioClip button;
    public AudioClip jumping;
    public AudioClip death;
    public AudioClip enemy_death;
    public AudioClip coin1;
    public AudioClip coin2;
    public AudioClip coin3;
    public AudioClip coin4;

    void Start() {
        loadPrefs();
    }
    
    // Music

    public void playMusic() {
        if (hasIntro) { // If there is an intro clip
            MusicSource.clip = intro;
            MusicSource.Play();
            StartCoroutine(WaitForIntroToEnd());
        } else { // Else play loop inmediately
            MusicSource.loop = true;
            MusicSource.clip = loop;
            MusicSource.Play();
        }
    }

    public void stopMusic() {
        MusicSource.Stop();
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

    // SFX

    public void playSound(AudioClip sound) {
        SFXSource.PlayOneShot(sound);
    }

    public void playButton() {
        SFXSource.PlayOneShot(button);
    }

    public void playGameOver() {
        // MusicSource.Stop();
        DeadSoundSource.PlayOneShot(death);
        //MusicSource.clip = death;
        //MusicSource.loop = false;
        //MusicSource.Play();
    }

    public void playJumping() {
        SFXSource.PlayOneShot(jumping);
    }

    public void playEnemy() {
        SFXSource.PlayOneShot(enemy_death);
    }

    public void playGetCoin() {
        AudioClip[] coins = new AudioClip[4]{coin1, coin2, coin3, coin4};
        int coin = Random.Range(0, 4);
        SFXSource.PlayOneShot(coins[coin]);
    }

    // Volume

    public void setVolume(AudioSource source, Slider slider) {
        source.volume = slider.value;
        savePrefs();
    }

    // Save preferences

    public void savePrefs() {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume.value);
    }

    public void loadPrefs() {
        if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SFXVolume")) {
            musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");
        } else {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        PlayerPrefs.SetFloat("SFXVolume", 1);
        }
    }

}