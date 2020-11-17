using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;
    }

    private void OnEnable()
    {
        Play();
        if (audioSource.clip != null)
        {
            Invoke("Off", audioSource.clip.length);
        }
    }

    private void Play()
    {
        this.PlayObject();       
    }

    private void Off()
    {
        gameObject.SetActive(false);
    }
}
