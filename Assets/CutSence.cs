﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;




//Prototype made by thuyet to handle cutscenes
public class CutSence : MonoBehaviour
{
    public int sequenceIndex=-1;
   
    Image backGroundImg;
    Transform CanvasUI;
    Button continueButton, previousButton, pauseButton, skipButton;
    TextMeshProUGUI hint;
    TextMeshProUGUI dialogueText;

    public float initWaitTime=0.5f;
    public float betweenLetterTime=0.02f;
    public float betweenSequenceWaitTime = 5f;
    public float deltaTime;
    float startSequenceTime;
    bool isPause=true;


    //class contain background and dialogue
    [System.Serializable]
    public class Sequences
    {
        public Sprite chosenbackGround;
        [TextArea(3, 10)]
        public string sentences;

    }
    public Sequences[] sequencesArray;
    //end of class declaration

   


    void Awake()
    {
        CanvasUI = GameObject.FindGameObjectWithTag("CanvasUI").transform;
        //CanvasUI = this.transform;
        if (CanvasUI == null)
        {
            print("null canvas");
        }
        else
        {
            previousButton = CanvasUI.GetChild(1).GetComponent<Button>();
            pauseButton = CanvasUI.GetChild(2).GetComponent<Button>();
            continueButton = CanvasUI.GetChild(3).GetComponent<Button>();
            skipButton = CanvasUI.GetChild(4).GetComponent<Button>();
            hint = CanvasUI.GetChild(5).GetComponent<TextMeshProUGUI>();
            dialogueText = CanvasUI.GetChild(6).GetComponent<TextMeshProUGUI>();
            backGroundImg = CanvasUI.GetChild(7).GetComponent<Image>();
        }
        SetActiveUIs(false);
        

    }

    void Start()
    {
      
        StartCoroutine(FirstWait(initWaitTime));
        
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.D))
        {
          SetNextSequence();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NextBuiltScene();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            SetLastSequence();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SetPause();
        }   

        if (isPause == false)
        {
            deltaTime = Time.time - startSequenceTime;
            if (deltaTime >= betweenSequenceWaitTime)
            {
                SetNextSequence();
            }
        }
    }


    void SetActiveUIs(bool var)
    {
        continueButton.gameObject.SetActive(var);
        pauseButton.gameObject.SetActive(var);
        previousButton.gameObject.SetActive(var);
        skipButton.gameObject.SetActive(var);
        hint.gameObject.SetActive(var);
        dialogueText.gameObject.SetActive(var);
        backGroundImg.gameObject.SetActive(var);
    }

    

    void SetBackGround(Sprite chosenbackGround)
    {
        if (chosenbackGround != null)
        {
            backGroundImg.sprite = chosenbackGround;
        }
        else
        {
            backGroundImg.sprite = null;
            print("no background");
        }
    }

    public void SetPause()
    {
        isPause = !isPause;
        if (isPause == false)
        {
            startSequenceTime = Time.time;
            print("pause");
        }
       
        
    }

    public void NextBuiltScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        SceneManager.LoadScene(buildIndex + 1);
    }

    IEnumerator Type()
    {
        foreach (char letter in sequencesArray[sequenceIndex].sentences.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(betweenLetterTime);
        }
    }
    public void SetNextSequence()
    {
        if (sequenceIndex< sequencesArray.Length - 1)
        {
            startSequenceTime = Time.time;
            sequenceIndex++;
            dialogueText.text = "";
            StartCoroutine(Type());
            SetBackGround(sequencesArray[sequenceIndex].chosenbackGround);

        }
        else
        {
            dialogueText.text = "";
            NextBuiltScene();

        }
    }

    public void SetLastSequence()
    {
        if (sequenceIndex > 0)
        {
            sequenceIndex--;
            dialogueText.text = "";
            StartCoroutine(Type());
            SetBackGround(sequencesArray[sequenceIndex].chosenbackGround);
        }
    }

  

    IEnumerator FirstWait(float time)
    {
        yield return new WaitForSeconds(time);
        
        SetActiveUIs(true);
        SetNextSequence();
        isPause = false;
    }

}
