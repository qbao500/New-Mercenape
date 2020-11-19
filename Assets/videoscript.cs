using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


//Prototype made by thuyet to handle start video such as logo or trailer

public class videoscript : MonoBehaviour
{
    VideoPlayer video;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;


    }


    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
          Scene currentScene = SceneManager.GetActiveScene();
            int buildIndex = currentScene.buildIndex;
            SceneManager.LoadScene(buildIndex+1);
    }
}
