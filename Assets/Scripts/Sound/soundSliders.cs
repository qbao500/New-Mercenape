using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class soundSliders : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider Master, SFX, Audio, Music;


    private void Start()
    {
        Master.value = PlayerPrefs.GetFloat("masterVolume", 0);
        SFX.value = PlayerPrefs.GetFloat("sfxVolume", 0);
        Audio.value = PlayerPrefs.GetFloat("audioVolume", 0);
        Music.value = PlayerPrefs.GetFloat("musicVolume", 0);
    }

    private void Update()
    {
        //For testing the mixers
        if(Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("U pressed");
            soundManager.PlaySound(soundManager.Sound.Audio_enemyPain, transform.position);

        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I pressed.");
            soundManager.PlaySound(soundManager.Sound.Music_Level, transform.position);
        }
    }

    public void setMasterVolume(float volume)
    {
        mixer.SetFloat("masterVolume", volume);
    }

    public void setSfxVolume(float volume)
    {
        mixer.SetFloat("sfxVolume", volume);
    }

    public void setAudioVolume(float volume)
    {
        mixer.SetFloat("audioVolume", volume);
    }

    public void setMusicVolume(float volume)
    {
        mixer.SetFloat("musicVolume", volume);
    }

    private void OnDisable()
    {
        float masterVolume = 0;
        float sfxVolume = 0;
        float audioVolume = 0;
        float musicvolume = 0;

        mixer.GetFloat("masterVolume", out masterVolume);
        mixer.GetFloat("sfxVolume", out sfxVolume);
        mixer.GetFloat("audioVolume", out audioVolume);
        mixer.GetFloat("musicVolume", out musicvolume);

        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("sfxVolume",  sfxVolume);
        PlayerPrefs.SetFloat("audioVolume", audioVolume);
        PlayerPrefs.SetFloat("musicVolume", musicvolume);
        PlayerPrefs.Save();
    }
}
