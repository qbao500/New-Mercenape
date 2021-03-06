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
   
    TextMeshProUGUI autoText, dialogueText;
    Button continueButton, previousButton;
    Menu menu;

    public float initWaitTime=0.5f;
    public float betweenLetterTime=0.02f;
    public float betweenSequenceWaitTime = 2f;
    public float deltaTime;
    public float characterAutoplayTime = 0.035f;
    float startSequenceTime;
    bool isAuto=false;
    

   
    [System.Serializable]
    public class Sequences
    {
        public Sprite chosenbackGround;
        
        [TextArea(3, 10)]
        public string sentences;

    }
    public Sequences[] sequencesArray;

   
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
            backGroundImg = CanvasUI.GetChild(0).GetComponent<Image>();
            dialogueText = CanvasUI.GetChild(2).GetComponent<TextMeshProUGUI>();
            autoText = CanvasUI.GetChild(8).GetComponent<TextMeshProUGUI>();
            
            previousButton = CanvasUI.GetChild(3).GetComponent<Button>();
            continueButton = CanvasUI.GetChild(5).GetComponent<Button>();
            menu = this.GetComponent<Menu>();
        }
       
        int[] excludedChild = { 1 };
        SetActiveUIs(false, excludedChild);
        
        autoText.gameObject.SetActive(false);

    }

    void Start()
    {
      
        StartCoroutine(FirstWait(initWaitTime));
        
    }



    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SetAuto();
        }

        if (menu != null)
        {
           if(menu.isPaused)
            {
                CanvasUI.gameObject.SetActive(false);
            }
            else
            {
                CanvasUI.gameObject.SetActive(true);
            }
        }

        if (isAuto == true)
        {
            deltaTime = Time.time - startSequenceTime;
            if (deltaTime >= betweenSequenceWaitTime)
            {
                SetNextSequence();
            }

        }

        if (sequenceIndex == 0)
        {
            if (dialogueText.text == sequencesArray[sequenceIndex].sentences)
            {
                continueButton.interactable = true;
                previousButton.interactable = false;
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    SetNextSequence();
                }

            }
        }
        if (sequenceIndex > 0)
        {
            if (dialogueText.text == sequencesArray[sequenceIndex].sentences)
            {
                continueButton.interactable = true;
                previousButton.interactable = true;
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    SetNextSequence();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    SetLastSequence();
                }
            }
        }


    }


    void SetActiveUIs(bool var, int[] excludedChild)
    {
        
        for (int i=0; i< CanvasUI.childCount; i++)
        { if (excludedChild.Length !=0)
            {for (int j=0;j< excludedChild.Length; j++)
                {
                    if (i != excludedChild[j]) { CanvasUI.GetChild(i).gameObject.SetActive(var); }
                }
               
            }

        }

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

    public void SetAuto()
    {
        isAuto = !isAuto;
        if (isAuto)
        {
            startSequenceTime = Time.time;
            autoText.gameObject.SetActive(true);
        }
        else
        {
            autoText.gameObject.SetActive(false);
        }
       
        
    }

    public void NextBuiltScene()
    {
        LevelLoader.instace.LoadLevel(2);
    }

    IEnumerator Type()
    {
        foreach (char letter in sequencesArray[sequenceIndex].sentences.ToCharArray())
        {
            dialogueText.text += letter;
            betweenSequenceWaitTime = betweenSequenceWaitTime + characterAutoplayTime;
            yield return new WaitForSeconds(betweenLetterTime);
        }
    }
    public void SetNextSequence()
    {
        if (sequenceIndex< sequencesArray.Length - 1)
        {
            betweenSequenceWaitTime = 2f;
            continueButton.interactable = false;
            previousButton.interactable = false; 
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
            continueButton.interactable = false;
            previousButton.interactable = false;
            sequenceIndex--;
            dialogueText.text = "";
            StartCoroutine(Type());
            SetBackGround(sequencesArray[sequenceIndex].chosenbackGround);
        }
    }

  

    IEnumerator FirstWait(float time)
    {
        int[] excludedChild = { 8 };
        yield return new WaitForSeconds(time);
        
        SetActiveUIs(true, excludedChild);
        SetNextSequence();
        isAuto = false;
        
    }

}
