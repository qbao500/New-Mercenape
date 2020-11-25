using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Prototype made by thuyet to handle cutscenes
public class CutSence : MonoBehaviour
{
    public int backgroundIndex;
    
    public Dialogue dialogue;
   
    DialogueManager dm;
    BubbleTalkManager bm;
    public int counter = 0;



    void Awake()
    {
		dm = this.GetComponent<DialogueManager>();
        bm = this.GetComponent<BubbleTalkManager>();

	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            backgroundIndex++;

                if (backgroundIndex < gameObject.transform.childCount)
                {
                gameObject.transform.GetChild(backgroundIndex).gameObject.SetActive(true);
                 }
                if (backgroundIndex > 0)
                {
                    gameObject.transform.GetChild(backgroundIndex - 1).gameObject.SetActive(false);
                }


            if (counter == 0)
            {
                dm.StartDialogue(dialogue);
                counter++;
            }
            else
            {
                dm.DisplayNextSentence();
                counter++;
            }
            if (counter %2 == 1)
            {
                bm.setBubbleTalk(3);
            }else if(counter % 2 == 0)
            {
                bm.setBubbleTalk(4);
            }
           
        }
        if(backgroundIndex >= gameObject.transform.childCount || Input.GetKeyDown(KeyCode.Escape))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int buildIndex = currentScene.buildIndex;
            SceneManager.LoadScene(buildIndex+1);
        }
    } 

}
