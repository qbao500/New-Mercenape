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
  
    SpriteRenderer spriteRenderer;

    Transform CanvasUI;
    Button continueButton, previousButton, pauseButton, skipButton;
    TextMeshProUGUI hint;
    TextMeshProUGUI dialogueText;

    private Queue<string> sentences;

    public float initWaitTime;

    


    //class contain background and bubbletalk
    [System.Serializable]
    public class Sequences
    {
        public Sprite chosenbackGround;
        
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
        CanvasUI = GameObject.FindGameObjectWithTag("CanvasUI").transform;
        if (CanvasUI == null)
        {
            print("null");
        }
        else
        {
            previousButton = CanvasUI.GetChild(1).GetComponent<Button>();
            pauseButton = CanvasUI.GetChild(2).GetComponent<Button>();
            continueButton = CanvasUI.GetChild(3).GetComponent<Button>();
            skipButton = CanvasUI.GetChild(4).GetComponent<Button>();
            hint = CanvasUI.GetChild(5).GetComponent<TextMeshProUGUI>();
            dialogueText = CanvasUI.GetChild(6).GetComponent<TextMeshProUGUI>();
        }


        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
        setActiveUIs(false);
        


    }

    void Start()
    {
        sentences = new Queue<string>();
        StartCoroutine(FirstWait(initWaitTime));
        
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            setNextSequence();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NextBuiltScene();
        }
    }


    void setActiveUIs(bool arg)
    {
        continueButton.gameObject.SetActive(arg);
        pauseButton.gameObject.SetActive(arg);
        previousButton.gameObject.SetActive(arg);
        skipButton.gameObject.SetActive(arg);
        hint.gameObject.SetActive(arg);
        dialogueText.gameObject.SetActive(arg);
    }

    void NextBuiltScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        SceneManager.LoadScene(buildIndex + 1);
    }

    public void setNextSequence()
    {
        if (sequenceIndex >= sequencesArray.Length)
        {
            NextBuiltScene();
        }
        else
        {

            setBackGround(sequencesArray[sequenceIndex].chosenbackGround);

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


     void setBackGround(Sprite chosenbackGround)
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

    


     void StartDialogue(Dialogue dialogue)
    {
        //animator.SetBool("IsOpen", true);


        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

     void DisplayNextSentence()
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


    IEnumerator FirstWait(float time)
    {
        yield return new WaitForSeconds(time);

        setActiveUIs(true);
        setNextSequence();
        
    }

}
