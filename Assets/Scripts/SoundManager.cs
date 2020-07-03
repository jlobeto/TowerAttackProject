using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;

    AudioSource _audioSource;

    void Start()
    {
        DontDestroyOnLoad(this);
        _audioSource = GetComponentInChildren<AudioSource>();
        var a = GetAudioClip("menu");
        _audioSource.clip = a;
        _audioSource.Play();
        _audioSource.loop = true;
        
    }

    
    void Update()
    {
        
    }

    public void MuteEverything(bool value)
    {
        _audioSource.mute = value;
    }

    public void PlayMusic(int lvlid)
    {
        if (lvlid <= 15)
            _audioSource.clip = GetAudioClip("ingame_first");
        else if(lvlid <= 30)
            _audioSource.clip = GetAudioClip("ingame_second");
        else
            _audioSource.clip = GetAudioClip("ingame_third");

        _audioSource.Play();
    }

    public void PlayMenuMusic()
    {
        _audioSource.clip = GetAudioClip("menu");
        _audioSource.Play();
    }


    AudioClip GetAudioClip(string name)
    {
        foreach (var item in audioClips)
        {
            if (item.name == name)
                return item;
        }

        return null;
    }
}
