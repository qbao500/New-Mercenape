using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Created by Arttu Paldán on 5.11.2020: 
public class PlayerSpeech: MonoBehaviour
{
    GameObject bubbleObject;
    TextMeshProUGUI speechBubble;

    public string howToAttack, howToBlock, cameraScroll;
    public float startMessageWait, waitBetweenMessages;

    void Awake()
    {
        bubbleObject = GameObject.FindGameObjectWithTag("PlayerSpeechBubble");
        speechBubble = bubbleObject.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        StartCoroutine(Wait(startMessageWait));
    }

    IEnumerator Wait(float time)
    {
        speechBubble.text = "";

        yield return new WaitForSeconds(time);

        StartCoroutine(SpawnMessage(speechBubble, howToAttack, howToBlock, cameraScroll, waitBetweenMessages));
    }

    IEnumerator SpawnMessage(TextMeshProUGUI messageObject, string message1, string message2, string message3, float time)
    {
        messageObject.text = message1;

        yield return new WaitForSeconds(time);

        messageObject.text = "";

        yield return new WaitForSeconds(time);

        messageObject.text = message2;

        yield return new WaitForSeconds(time);

        messageObject.text = "";

        yield return new WaitForSeconds(time);

        messageObject.text = message3;

        yield return new WaitForSeconds(time);

        messageObject.text = "";
    }
}
