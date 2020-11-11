using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Arttu Paldán 11.9.2020: This is just a temporary script for testing purposes. I need to have some kinda of temporary currency in place to test buying part of the component unlocking. 
public class Money : MonoBehaviour
{
    private PlayerCurrency playerCurrency;
    
    private int currency, newCurrency;

    private Text currencyHolderShop, currencyHolderForge;
    

    void Awake()
    {
        playerCurrency = GetComponent<PlayerCurrency>();
        
        currencyHolderShop = GameObject.FindGameObjectWithTag("Money").GetComponentInChildren<Text>();
        currencyHolderForge = GameObject.FindGameObjectWithTag("MoneyUpgrade").GetComponentInChildren<Text>();
    }

    void Start()
    {
        SetStartingCurrency();
    }

    // Public function for the other scripts to get how much player has money.
    public int GetCurrentCurrency(){ return currency;}

    // Function to set the starting money.
    void SetStartingCurrency()
    {
        currency = playerCurrency.gold;
        
        if(currencyHolderShop != null) { currencyHolderShop.text = "Money: " + currency; }
        if(currencyHolderForge != null) { currencyHolderForge.text = "Money: " + currency; }
    }
    
    // Function for changing amount of money, for example when buying components. 
    public void ChangeCurrencyAmount(int change)
    {
        newCurrency = currency - change;

        currency = newCurrency;

        playerCurrency.gold = currency;

        if (currencyHolderShop != null) { currencyHolderShop.text = "Money: " + currency; }
        if (currencyHolderForge != null) { currencyHolderForge.text = "Money: " + currency; }
    }
}
