using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundInfo
{
    public string key;
    public AudioClip sound;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public GameObject singleSoundPrefab;
    public GameObject continuousSoundPrefab;

    public List<SoundInfo> sounds;

    public List<GameObject> activeContinuousSound;
    public List<GameObject> activeSingleSound;
    
    private void Awake()
    {
        activeContinuousSound = new List<GameObject>();
        activeSingleSound = new List<GameObject>();

        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        PlayContinuousSound("kalimba");
    }

    void Update()
    {
        ClearMissingObjectsInList();
    }
    
    public void PlaySingleSound(string key)
    {
        GameObject sound = AddSingleSoundToActiveList(key);
        sound.GetComponent<SingleSound>().Play();
    }
    
    public void PlayContinuousSound(string key)
    {
        GameObject newContinuousSound = Instantiate(continuousSoundPrefab, this.transform) as GameObject;
        SoundInfo info = FindInfoAboutSound(key);

        if (info.key != "")
        {
            ContinuousSound continuousSound = newContinuousSound.GetComponent<ContinuousSound>();
            continuousSound.activeSound.key = info.key;
            continuousSound.activeSound.sound = info.sound;

            activeContinuousSound.Add(newContinuousSound);
        }
    }

    public void DestroyContinuousSound(string key)
    {
        int index = 0;
        foreach (GameObject info in activeContinuousSound)
        {
            if (info.GetComponent<ContinuousSound>())
            {
                if (info.GetComponent<ContinuousSound>().activeSound.key == key)
                {
                    DestroyImmediate(info);
                    activeContinuousSound.RemoveAt(index);
                    break;
                }
            }
            index++;
        }
    }

    private GameObject AddSingleSoundToActiveList(string key)
    {
        GameObject newSingleSound = Instantiate(singleSoundPrefab, this.transform) as GameObject;
        SoundInfo info = FindInfoAboutSound(key);

        if (info.key != "")
        {
            SingleSound singleSound = newSingleSound.GetComponent<SingleSound>();
            singleSound.activeSound.key = info.key;
            singleSound.activeSound.sound = info.sound;

            activeSingleSound.Add(newSingleSound);
        }

        return activeSingleSound[activeSingleSound.Count - 1];
    }

    private SoundInfo FindInfoAboutSound(string key)
    {
        foreach (SoundInfo info in sounds)
        {
            if (info.key == key)
            {
                return info;
            }
        }

        return new SoundInfo();
    }

    private void ClearMissingObjectsInList()
    {
        int index = 0;
        foreach (GameObject info in activeSingleSound)
        {
            if (!info)
            {
                activeSingleSound.RemoveAt(index);
                break;
            }
            index++;
        }
    }
}
