using System;
using Unity.Netcode;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource m_MusicSource, m_EffectSource;
    [SerializeField] private AudioClip Music;

    private void OnEnable()
    {
        Instance = this;
        
        PlayAudio(Music, true);
    }

    public void PlayAudio(AudioClip clip, bool isMusic)
    {
        if (isMusic)
        {
            m_MusicSource.PlayOneShot(clip);
        }
        else
        {
            m_EffectSource.PlayOneShot(clip);
        }
    }
}
