using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class soundManager
{
    //Written by Ossi Uusitalo, guide to it by Code Monkey, https://www.youtube.com/watch?v=QL29aTa7J5Q&t=323s
    public enum Sound
    {
        //26.11 update by Ossi: Now all sounds will have either SFX, Audio or Music before the sound name to sort them in their audio mixer groups.

        //Sound effects
        SFX_playerMove,
        SFX_playerJump,
        SFX_playerAttack,
        SFX_playerDie,
        SFX_enemyAttack,
        SFX_enemyDie,
        SFX_heal,
        SFX_runnerSpawn,
        SFX_mowerSpawn,

        //Audio/Voice lines
        Audio_playerHurt,
        Audio_playerDie,
        Audio_enemyPain,

        //Music
        Music_Menu,
        Music_Level,

        // UI sounds
        UI_buttonPress,
        UI_buttonBack,
        UI_Quit,
    }

    //This is for sound we want to play at different intervals such as footsteps
    private static Dictionary<Sound, float> soundtimerDictionary;

    public static Sound chosenSound;
    private static AudioClip audioClip; // for saving clip to play

    private static AssetManager assetManager;

    public static void Initialize(AssetManager aManager)
    {
        //This method is called by the Asset Manager script upon Awake()
        soundtimerDictionary = new Dictionary<Sound, float>();
        soundtimerDictionary[Sound.SFX_playerMove] = 0f;
        assetManager = aManager;
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        //AssetManager aManager = GameObject.Find("GameManager").GetComponent<AssetManager>();
        if (assetManager != null) //This is a failsafe
        {
            if (CanPlaySound(sound))
            {
                // Edit by Bao 10.11.20: use object pooling to play sound instead of Instansiate/Destroy
                audioClip = GetAudioClip(sound);
                chosenSound = sound;
                ObjectPooler.Instance.SpawnFromPool("Sound", position, Quaternion.identity);

                // Most of the below code are moved to SoundObject script, which is attached to a prefab that already has AudioSource component
                // Which will handle sound without continuously Instantiate, AddComponent and Destroy
                // I just comment those code if you want to delete or revert something

                //THis creates an empty game object that plays the audioclip it's been given.
                //GameObject soundGameObject = new GameObject("Sound");
                //soundGameObject.transform.position = position;
                //AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
                //audioSource.clip = GetAudioClip(sound);

                // These are to set the sound as a 3D sound.
                /*audioSource.maxDistance = 100f;
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.dopplerLevel = 0f;  
                audioSource.Play();*/
                //Once the lenght of the audioclip is done, the gameobject destroys itself from the scene.
                //Object.Destroy(soundGameObject, audioSource.clip.length);


                //Debug.Log(sound + " played");
            }
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        //This checks if the requested sound is in the Asset Manager array
        switch(sound)
        {
            case Sound.SFX_playerMove:
                {
                    if (soundtimerDictionary.ContainsKey(sound))
                    {
                        float lastTimeplayed = soundtimerDictionary[sound];
                        float playerMoveTimerMax = 0.3f;
                        if (lastTimeplayed + playerMoveTimerMax < Time.time)
                        {
                            soundtimerDictionary[sound] = Time.time;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    } else
                    {
                        return false;
                    }
                }
            default: { return true; }

        }
    }

    public static AudioClip GetAudioClip(Sound sound)
    {
        //If CanPLay SOund bool is true, it gets the named audioclip and adds it to the newly made "Sound" gameObject.
        //AssetManager aManager = GameObject.Find("GameManager").GetComponent<AssetManager>();
        if (assetManager != null)
        {
            foreach(AssetManager.SoundAudioClip soundAudioclip in assetManager.soundAudioclipArray)
            {
                if(soundAudioclip.sound == sound)
                {
                    return soundAudioclip.audioclip;
                }
            }
            Debug.LogError("Sound not found");
            return null;
        } else
        {
            return null;
        }
                
    }

    public static void PlayObject(this SoundObject soundObject)
    {
        soundObject.audioSource.clip = audioClip;
        soundObject.audioSource.Play();
    }
}
