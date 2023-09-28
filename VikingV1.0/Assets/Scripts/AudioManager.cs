using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume= sound.volume;
            sound.source.pitch= sound.pitch;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds,sound => sound.name==name);
        s.source.Play();
    }
    public void StopPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip);
    }
}
