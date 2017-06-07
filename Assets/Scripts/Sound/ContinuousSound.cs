using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSound : MonoBehaviour
{
    public SoundInfo activeSound;
    public AudioSource playingSound;

    void Start()
    {
        playingSound = GetComponent<AudioSource>();
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
