using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BubbleTalkManager : MonoBehaviour
{
    public Image bubbleTalkImg;
    public Sprite[] spriteArray;

 

    // Update is called once per frame
    public void setBubbleTalk(int bubbleTalkIndex)
    {
        if (bubbleTalkIndex < spriteArray.Length)
        {
            bubbleTalkImg.sprite = spriteArray[bubbleTalkIndex];
        }
        else
        {
            print("out of array");
        }
         
    }
}
