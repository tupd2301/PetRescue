using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public List<SoundData> sounds = new List<SoundData>();

    public List<AudioSourceData> audioSources = new List<AudioSourceData>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        PlaySound("BGM");
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            PlaySound("Test (S)");
        }
    }

    public void PlaySound(string name)
    {
        List<SoundData> datas = sounds.FindAll(x => x.name == name);
        foreach (SoundData sound in datas)
        {
            if (sound != null && sound.clipPaths.Count > 0)
            {
                int index = Random.Range(0, sound.clipPaths.Count);
                AudioSource audioSource = audioSources.FirstOrDefault(x => x.type == sound.audioSourceType).audioSource;
                AudioClip audioClip = Resources.Load<AudioClip>("SFXs/" + sound.clipPaths[index]);
                if (audioClip == null) return;
                audioSource.clip = audioClip;
                audioSource.pitch = sound.speedScale;
                if (sound.audioSourceType == AudioSourceType.BGM)
                {
                    audioSource.Play();
                }
                else
                {
                    audioSource.PlayOneShot(audioSource.clip, sound.volumeScale);
                }
            }
        }
    }
}
[System.Serializable]
public class SoundData
{
    public string name;
    public AudioSourceType audioSourceType;
    public float volumeScale = 1;
    public float speedScale = 1;
    public List<string> clipPaths = new List<string>();
}
[System.Serializable]
public class AudioSourceData
{
    public AudioSourceType type;
    public AudioSource audioSource;
}
public enum AudioSourceType
{
    BGM,
    SFX
}


