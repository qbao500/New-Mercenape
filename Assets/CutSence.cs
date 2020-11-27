using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;




//Prototype made by thuyet to handle cutscenes
public class CutSence : MonoBehaviour
{
    public int sequenceIndex=0;
    
    //public Animator animator;
    public TextMeshProUGUI dialogueText;
    SpriteRenderer spriteRenderer;
    public Image bubbleTalkImg;

    private Queue<string> sentences;

    //class contain background and bubbletalk
    [System.Serializable]
    public class Sequences
    {
        public Sprite chosenbackGround;
        public Sprite chosenBubbleTalk;
        
    }
    public Sequences[] sequencesArray;
    //end of class declaration

    //class contain dialogues
    [System.Serializable]
    public class Dialogue
    {
        [TextArea(3, 10)]
        public string[] sentences;
    }
    public Dialogue dialogue;
    //end of class declaration


    void Awake()
    {
	
        spriteRenderer = this.GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        sentences = new Queue<string>();
        StartCoroutine(Wait(0.5f));
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            if (sequenceIndex >= sequencesArray.Length)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                int buildIndex = currentScene.buildIndex;
                SceneManager.LoadScene(buildIndex + 1);
            }
            else
            {

                setBackGround(sequencesArray[sequenceIndex].chosenbackGround);
                setBubbleTalk(sequencesArray[sequenceIndex].chosenBubbleTalk);

                if (sequenceIndex == 0)
                {
                    StartDialogue(dialogue);
                }
                else
                {
                    DisplayNextSentence();
                }
                sequenceIndex++;
            }
           

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int buildIndex = currentScene.buildIndex;
            SceneManager.LoadScene(buildIndex + 1);
        }
    }


    public void setBubbleTalk(Sprite chosenBubbleTalk)
    {
        if (chosenBubbleTalk != null)
        {
            bubbleTalkImg.sprite = chosenBubbleTalk;
        }
        else
        {
            bubbleTalkImg.sprite = null;

               print("no bubble talk");
        }

    }

    public void setBackGround(Sprite chosenbackGround)
    {
        if (chosenbackGround != null)
        {
            spriteRenderer.sprite = chosenbackGround;
        }
        else
        {
            spriteRenderer.sprite = null;
            print("no background");
        }
    }


    public void StartDialogue(Dialogue dialogue)
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


    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        bubbleTalkImg.enabled=true;

        setBackGround(sequencesArray[sequenceIndex].chosenbackGround);
        setBubbleTalk(sequencesArray[sequenceIndex].chosenBubbleTalk);

        if (sequenceIndex == 0)
        {
            StartDialogue(dialogue);
        }
        else
        {
            DisplayNextSentence();
        }   
        sequenceIndex++;

    }

}
