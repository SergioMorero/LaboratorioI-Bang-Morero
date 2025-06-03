using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class easter : MonoBehaviour {

    /*
    Metal pipe
    Shuba
    A
    Combo
    Hadoken
    Bonk

     */

    // Add from inspector
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private List<AudioClip> sounds;
    private List<AudioClip> unplayedPile;
    private List<AudioClip> playedPile;

    public bool enable = true;

    private List<KeyCode> targetSequence = new List<KeyCode> {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };

    private int i = 0;
    private KeyCode nextKey;

    void Start() {
        unplayedPile = new List<AudioClip>(sounds);
        playedPile = new List<AudioClip>();
        nextKey = targetSequence[i++];
    }

    void Update() {
        if (!enable) {return;}
        if (Input.GetKeyDown(nextKey)) { // continue the sequence
            if (i == 11) {
                playSound();
                i = 0;
                nextKey = targetSequence[i++];
            }
            nextKey = targetSequence[i++];
        } else if (Input.anyKeyDown) { //clear, input was different from sequence
            i = 0;
            nextKey = targetSequence[i++];
        }
    }

    void playSound() {
        int index = Random.Range(0, unplayedPile.Count);
        AudioClip clip = unplayedPile[index];
        playedPile.Add(clip);
        unplayedPile.RemoveAt(index);

        if (unplayedPile.Count == 0) {
            unplayedPile = new List<AudioClip>(sounds);
            playedPile = new List<AudioClip>();
        }
        SFXSource.PlayOneShot(clip);
    }

    /** Activate or deactivate on death */
    void enable(Boolean enable)
    {
        enable = enable;
    }
}
