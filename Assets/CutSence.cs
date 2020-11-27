using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;




//Prototype made by thuyet to handle cutscenes
public class CutSence : MonoBehaviour
{
    public int backgroundIndex;
    public int counter = 0;
   
 

    public Sprite[] backgroundArray;
    SpriteRenderer spriteRenderer;

    public Sprite[] bubbleTalkArray;
    public Image bubbleTalkImg;

    //public Animator animator;
    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences;

    [System.Serializable]
    public class Sequences
    {
        public int backGroundIndex;
        public int[] bubbleTalkIndex;
        [TextArea(3, 10)]
        public string[] sentences;
    }
    public Sequences[] sequencesArray;
    

    void Awake()
    {
	
        spriteRenderer = this.GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        sentences = new Queue<string>();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            backgroundIndex++;

            spriteRenderer.sprite = backgroundArray[backgroundIndex];


            if (counter == 0)
            {
               StartDialogue(sequencesArray[counter]);
                counter++;
            }
            else
            {
               DisplayNextSentence();
                counter++;
            }
            if (counter %2 == 1)
            {
                setBubbleTalk(3);
            }else if(counter % 2 == 0)
            {
               setBubbleTalk(4);
            }
           
        }
        if(backgroundIndex >= backgroundArray.Length || Input.GetKeyDown(KeyCode.Escape))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int buildIndex = currentScene.buildIndex;
            SceneManager.LoadScene(buildIndex+1);
        }
    }


    public void setBubbleTalk(int bubbleTalkIndex)
    {
        if (bubbleTalkIndex < bubbleTalkArray.Length)
        {
            bubbleTalkImg.sprite = bubbleTalkArray[bubbleTalkIndex];
        }
        else
        {
            print("out of array");
        }

    }

    public void StartDialogue(Sequences dialogue)
    {
        //animator.SetBool("IsOpen", true);


        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        //animator.SetBool("IsOpen", false);
        dialogueText.SetText("");
    }

}
