using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SingleSound : MonoBehaviour
{
    public SoundInfo activeSound;
    public AudioSource playingSound;
	public AudioMixerGroup mixer;
    public bool isStarted = false;


    void Start()
    {
        playingSound = GetComponent<AudioSource>();

		if (mixer != null) 
		{
			playingSound.outputAudioMixerGroup = mixer;

			//playingSound.outputAudioMixerGroup = mixer;
		}

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
