using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] AudioClip deathClip;

    [Header("Grave")]
    [SerializeField] AudioClip graveClip;

    static AudioPlayer instance;
    AudioSource audioSource;

    void Awake() 
    {
        ManageSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayDeathClip()
    {
        PlayClip(deathClip);
    }

    public void PlayGraveClip()
    {
        PlayClip(graveClip);
    }

    void PlayClip(AudioClip clip)
    {
        if(clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

