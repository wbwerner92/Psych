using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerClass
{
    public static AudioManager instance;

    public  AudioSource audioSource;

    private Dictionary<string, AudioClip> m_audioDict;
    public AudioClip hit_sound;
    public AudioClip miss_sound;
    public AudioClip psy_laser_sound;
    public AudioClip power_up_sound;
    public AudioClip psy_wave_sound;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadAudioResources();
        Initialize();
    }

    private void LoadAudioResources()
    {
        m_audioDict = new Dictionary<string, AudioClip>();
        m_audioDict.Add("hit_sound", hit_sound);
        m_audioDict.Add("miss_sound", miss_sound);
        m_audioDict.Add("psy_laser_sound", psy_laser_sound);
        m_audioDict.Add("psy_wave_sound", psy_wave_sound);
        m_audioDict.Add("power_up_sound", power_up_sound);
    }

    public AudioClip GetAudioClip(string clipName)
    {
        if (m_audioDict != null && m_audioDict.ContainsKey(clipName))
        {
            return m_audioDict[clipName];
        }
        else
        {
            Debug.LogError("Cannot find Audio Clip: " + clipName);
            return null;
        }
    }

    public void PlayAudioClip(string clipName)
    {
        Debug.Log("Play Audio Clip: " + clipName);
        AudioClip clip = GetAudioClip(clipName);
        if (clip == null)
        {
            return;
        }
        audioSource.clip = clip;
        audioSource.Play();
    }
}
