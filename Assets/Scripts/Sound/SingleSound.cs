using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSound : MonoBehaviour
{
    public SoundInfo activeSound;
    public AudioSource playingSound;
    public bool isStarted = false;


    void Start()
    {
        playingSound = GetComponent<AudioSource>();
        if (isStarted)
        {
            Play();
        }
    }

    void Update()
    {
        if (!playingSound.isPlaying && isStarted)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public void Play()
    {
        PrepareSound();
        playingSound.Play();
        isStarted = true;
    }

    private void PrepareSound()
    {
        playingSound = GetComponent<AudioSource>();
        playingSound.clip = activeSound.sound;
    }
}
