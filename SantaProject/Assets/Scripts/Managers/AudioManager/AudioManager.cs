using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]public List<SoundList> soundLists = new List<SoundList>();

    private AudioSource speaker;

    // Start is called before the first frame update
    void Awake()
    {
        speaker = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        //foreach (SoundList list in soundLists)
        //{
        //    foreach (Sound sound in list.sounds)
        //    {
        //        sound.source = gameObject.AddComponent<AudioSource>();
        //        sound.source.clip = sound.clip;

        //        sound.source.volume = sound.volume;
        //        sound.source.pitch = sound.pitch;
        //    }
        //}

    }

    public void PlaySound(String soundName)
    {
        for(int index = 0; index < soundLists.Count; index++)
        {
            if (soundLists[index].name == soundName)
            {
                if (soundLists[index].sounds.Count > 0)
                {
                    int rand = Random.Range(0, soundLists[index].sounds.Count);
                    speaker.volume = soundLists[index].sounds[rand].volume;
                    speaker.pitch = soundLists[index].sounds[rand].pitch;
                    speaker.PlayOneShot(soundLists[index].sounds[rand].clip,speaker.volume);
                    //list.sounds[rand].source.PlayOneShot(list.sounds[rand].clip);
                }
                else
                {
                    speaker.PlayOneShot(soundLists[index].sounds[0].clip, speaker.volume);
                    //list.sounds[0].source.PlayOneShot(list.sounds[0].clip);
                }
            }
        }
    }
}
