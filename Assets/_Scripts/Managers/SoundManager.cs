using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource soundFXAudioSource;
    [SerializeField] private AudioClip textEnableSound;
    [SerializeField] private AudioClip endingMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StopAllSounds()
    {
        soundFXAudioSource.Stop();
    }

    public void PlayTextEnableSound()
    {
        PlaySound(textEnableSound);
    }

    public void PlaySound(AudioClip audioClip)
    {
        soundFXAudioSource.PlayOneShot(audioClip);
    }
}
