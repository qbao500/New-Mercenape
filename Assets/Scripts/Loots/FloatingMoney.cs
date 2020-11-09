﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingMoney: MonoBehaviour
{
    PlayerCurrency playerCurrency;
    TextMeshPro textMesh;

    private int moneyAmount, sortingOrder;

    //public GameObject destination;
    public float speed, destroyTime;


    void Awake()
    {
        playerCurrency = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerCurrency>();
        //destination = GameObject.FindGameObjectWithTag("ResourceDestination");

        moneyAmount = Random.Range(10, 100);

        textMesh = textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        sortingOrder = 15;
        textMesh.sortingOrder = sortingOrder;
        textMesh.SetText(moneyAmount.ToString());     
    }

    private void OnEnable()
    {
        AddMoney();
        Invoke("Off", destroyTime);
    }

    void Update()
    {
         transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void AddMoney()
    {
        playerCurrency.AddGold(moneyAmount);
    }

    void Off()
    {
        gameObject.SetActive(false);
    }
}
