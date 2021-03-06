using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    void Awake () 
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    
    // Start is called before the first frame update
    //void Start()
    //{
    //    
    //}

    public void Play (string name, float delay)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.PlayDelayed(delay);
    }
}
