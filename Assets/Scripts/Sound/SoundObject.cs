using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundObject : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    
    

    
    private void Awake()
    {

        audioSource = GetComponent<AudioSource>();
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 1f;
        //AudioMixer mixer = Resources.Load("AudioMixer") as AudioMixer;
        //    Debug.Log("Mixer found");
        //if (soundManager.chosenSound.ToString().Contains("SFX"))
        //{
        //    Debug.Log("SFX found");
        //    audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        //}
        //else if (soundManager.chosenSound.ToString().Contains("Audio"))
        //{
        //    Debug.Log("Audio found");
        //    audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Audio")[0];
        //}
        //else if (soundManager.chosenSound.ToString().Contains("Music"))
        //{
        //    audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
        //    Debug.Log("Music found");
        //}
        
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
        //Here we set the sound effect on the correct mixer group.
        audioSource = GetComponent<AudioSource>();
        AudioMixer mixer = Resources.Load("AudioMixer") as AudioMixer;
        if (mixer != null)
        {
            if (soundManager.chosenSound.ToString().Contains("SFX"))
            {
                //Debug.Log("SFX found");
                audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
                audioSource.spatialBlend = 1f;
            }
            else if (soundManager.chosenSound.ToString().Contains("Audio"))
            {
                //Debug.Log("Audio found");
                audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Audio")[0];
                audioSource.spatialBlend = 1f;
            }
            else if (soundManager.chosenSound.ToString().Contains("Music"))
            {
                audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
                audioSource.spatialBlend = 0f;
               // Debug.Log("Music found");
            }
            else if (soundManager.chosenSound.ToString().Contains("UI_"))
            {
                audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("UI")[0];
                audioSource.spatialBlend = 0f;
                Debug.Log("UI sound.");
            }
            this.PlayObject();
        }
    }

    private void Off()
    {
        gameObject.SetActive(false);
    }
}
