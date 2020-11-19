using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Prototype made by thuyet to handle cutscenes
public class CutSence : MonoBehaviour
{
    public int imageCount;
    float startWaitTime=1f ;
    float delta;
    // Start is called before the first frame update
   
    void Start()
    {
        imageCount = 0;
        delta = startWaitTime - Time.deltaTime;
        if (delta <= 0)
        {
            gameObject.transform.GetChild(imageCount).gameObject.SetActive(true);
            print("hello");
        }

    }
    
    

 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
                imageCount++;

                if (imageCount < gameObject.transform.childCount)
                {
                gameObject.transform.GetChild(imageCount).gameObject.SetActive(true);
                 }
                if (imageCount > 0)
                {
                    gameObject.transform.GetChild(imageCount - 1).gameObject.SetActive(false);
                }
           
        }
        if(imageCount >= gameObject.transform.childCount || Input.GetKeyDown(KeyCode.Escape))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int buildIndex = currentScene.buildIndex;
            SceneManager.LoadScene(buildIndex+1);
        }
    } 

}
