using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string FX_GO_NAME = "audioSource_fx_";
    public static SoundManager instance;

    public AudioClip[] musicAudioClips;
    public AudioClip[] soundFXAudioClips;
    /// <summary>
    /// number of audiosources to instantiate for sound fxs
    /// </summary>
    public int soundFXSourcesCount = 5;

    AudioSource _musicSource;
    List<AudioSource> _audioSourceList;
    int _fxSourcesCount;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
    }

    void Start()
    {
        _musicSource = GetComponentInChildren<AudioSource>();
        var a = GetMusicAudioClip("menu");
        _musicSource.clip = a;
        _musicSource.Play();
        _musicSource.loop = true;

        _audioSourceList = new List<AudioSource>();
        _fxSourcesCount = soundFXSourcesCount;

        for (int i = 0; i < soundFXSourcesCount; i++)
        {
            var go = new GameObject(FX_GO_NAME + i);
            go.transform.SetParent(transform);
            var audio = go.AddComponent<AudioSource>();
            _audioSourceList.Add(audio);
        }
            

    }

    
    void Update()
    {
        
    }

    public void AdjustMusicVol(float value)
    {
        var val = value;
        /*if (value < 0.05f)
            val = 0;*/

        _musicSource.volume = val;
        
    }

    public void AdjustSoundFXVol(float value)
    {
        var val = value;
        /*if (value < 0.05f)
            val = 0;*/

        foreach (var item in _audioSourceList)
        {
            item.volume = val;
        }
    }

    public void PlaySound(SoundFxNames name)
    {
        if (name == SoundFxNames.none)
            Debug.Log("none sound fx name");

        var source = GetSoundFXSource();
        var clip = GetSoundAudioClip(name.ToString());
        source.clip = clip;
        source.Play();
    }

    public void MuteEverything(bool value)
    {
        _musicSource.mute = value;
    }

    public void PlayLevelMusic(int lvlid)
    {
        if (lvlid <= 15)
            _musicSource.clip = GetMusicAudioClip("ingame_first");
        else if(lvlid <= 30)
            _musicSource.clip = GetMusicAudioClip("ingame_second");
        else
            _musicSource.clip = GetMusicAudioClip("ingame_third");

        _musicSource.Play();
    }

    public void PlayMenuMusic()
    {
        _musicSource.clip = GetMusicAudioClip("menu");
        _musicSource.Play();
    }


    AudioClip GetSoundAudioClip(string name)
    {
        foreach (var clip in soundFXAudioClips)
        {
            if (clip.name == name)
            {
                foreach (var source in _audioSourceList)
                {
                    if(source.clip != null && source.clip.name == clip.name)
                        return source.isPlaying ? null : clip;
                }
                
                return clip;
            }
        }

        Debug.Log("no sound founded - param name= " + name);
        return null;
    }

    AudioClip GetMusicAudioClip(string name)
    {
        foreach (var item in musicAudioClips)
        {
            if (item.name == name)
                return item;
        }

        return null;
    }

    AudioSource GetSoundFXSource()
    {
        foreach (var item in _audioSourceList)
        {
            if (item.clip == null || !item.isPlaying)
                return item;
        }

        var go = new GameObject(FX_GO_NAME + _fxSourcesCount);
        _fxSourcesCount++;
        go.transform.SetParent(transform);
        var audio = go.AddComponent<AudioSource>();
        audio.volume = _audioSourceList[0].volume;
        _audioSourceList.Add(audio);
        

        return audio;
    }
}
