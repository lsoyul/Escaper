using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class SoundContainer : MonoBehaviour
{
    [Serializable]
    public struct NamedSound
    {
        public string name;
        public AudioSource audioSource;
    }

    public NamedSound[] SoundEffectsList;
    public NamedSound[] BackGroundMusicsList;

    private Dictionary<string, AudioSource> soundEffectsDic;
    private Dictionary<string, AudioSource> backGroundMusicsDic;


    public Dictionary<string, AudioSource> SoundEffectsDic
    {
        get { return soundEffectsDic; }
    }
    public Dictionary<string, AudioSource> BackGroundMusicsDic
    {
        get { return backGroundMusicsDic; }
    }

    private static GameObject container;
    private static SoundContainer instance;

    public static SoundContainer Instance()
    {
        //if (instance == null)
        //{
        //    container = new GameObject();
        //    container.name = "StageLoader";
        //    instance = container.AddComponent(typeof(StageLoader)) as StageLoader;
        //}

        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            soundEffectsDic = new Dictionary<string, AudioSource>();
            backGroundMusicsDic = new Dictionary<string, AudioSource>();

            foreach (NamedSound sound in SoundEffectsList)
            {
                soundEffectsDic.Add(sound.name, sound.audioSource);
            }

            foreach (NamedSound music in BackGroundMusicsList)
            {
                backGroundMusicsDic.Add(music.name, music.audioSource);
            }

            container = this.gameObject;
            container.name = "SoundContainer";
            instance = GetComponent<SoundContainer>();
            DontDestroyOnLoad(this);
        }
    }
}
