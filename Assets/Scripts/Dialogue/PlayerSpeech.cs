﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Created by Arttu Paldán on 5.11.2020: 
public class PlayerSpeech: MonoBehaviour
{
    GameObject bubbleObject;
    TextMeshProUGUI speechBubble;
    public Image bubbleTalk;
    public Sprite[] bubbleTalkArray;

    public string howToAttack, howToBlock, howToClimb ,cameraScroll;
    public float startMessageWait, waitBetweenMessages;

    void Awake()
    {
        bubbleObject = GameObject.FindGameObjectWithTag("PlayerSpeechBubble");
        speechBubble = bubbleObject.GetComponent<TextMeshProUGUI>();
        bubbleTalk = bubbleObject.transform.parent.gameObject.GetComponent<Image>();
    }
    void Start()
    {
        StartCoroutine(Wait(startMessageWait));
    }

    IEnumerator Wait(float time)
    {
        speechBubble.text = "";
        bubbleTalk.enabled = false;

        yield return new WaitForSeconds(time);

        StartCoroutine(SpawnMessage(speechBubble, howToAttack, howToBlock, howToClimb, cameraScroll, waitBetweenMessages));
    }

    IEnumerator SpawnMessage(TextMeshProUGUI messageObject, string message1, string message2, string message3, string message4, float time)
    {
        messageObject.text = message1;
        bubbleTalk.enabled = true;
        if (bubbleTalkArray.Length != 0)
        {
            bubbleTalk.sprite = bubbleTalkArray[0];
        }

        yield return new WaitForSeconds(time);

        messageObject.text = "";
        bubbleTalk.enabled = false;


        yield return new WaitForSeconds(time);

        messageObject.text = message2;
        bubbleTalk.enabled = true;
        if (bubbleTalkArray.Length != 0)
        {
            bubbleTalk.sprite = bubbleTalkArray[1];
        }

        yield return new WaitForSeconds(time);

        messageObject.text = "";
        bubbleTalk.enabled = false;


        yield return new WaitForSeconds(time);

        messageObject.text = message3;
        bubbleTalk.enabled = true;
        if (bubbleTalkArray.Length != 0)
        {
            bubbleTalk.sprite = bubbleTalkArray[1];
        }

        yield return new WaitForSeconds(time);

        messageObject.text = "";
        bubbleTalk.enabled = false;

        yield return new WaitForSeconds(time);


        messageObject.text = message4;
        bubbleTalk.enabled = true;
        if (bubbleTalkArray.Length != 0)
        {
            bubbleTalk.sprite = bubbleTalkArray[0];
        }

        yield return new WaitForSeconds(time);

        messageObject.text = "";
        bubbleTalk.enabled = false;

    }
}
