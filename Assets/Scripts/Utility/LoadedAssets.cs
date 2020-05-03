using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadedAssets : MonoBehaviour
{
    public VideoClip[] videoClips;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }

    public VideoClip GetVideoByName(string name)
    {
        if (name == null)
            return null;

        name = name.ToLower()+ "_video";
        foreach (var item in videoClips)
        {
            if (item.name == name)
                return item;
        }

        return null;
    }
}
