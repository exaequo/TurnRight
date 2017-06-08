using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ContinuousSound : MonoBehaviour
{
    public SoundInfo activeSound;
	public AudioSource playingSound;
	public AudioMixerGroup mixer;

    void Start()
    {
        playingSound = GetComponent<AudioSource>();

		Debug.Log (mixer);
		if (mixer != null) 
		{
			playingSound.outputAudioMixerGroup = mixer;
		}
        Play();
    }

    void Update()
    { }

    public void Play()
    {
        playingSound.loop = true;
        playingSound.clip = activeSound.sound;
        playingSound.Play();
    }

    public void Play(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(activeSound.sound, position);
    }

    public void DestroyPlaying()
    {
        DestroyImmediate(this.gameObject);
    }
}
