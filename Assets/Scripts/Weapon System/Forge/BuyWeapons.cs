using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Created by Arttu Paldán 11.9.2020: This script will handle buying or unlocking component pieces.
public class BuyWeapons : MonoBehaviour
{
    private SetUpScreens setUp;
    private Money money;
    private WeaponStates weaponStates;

    private List<AbstractWeapon> weapons;

    private int weaponID;

    private bool cantBuy;

    private float counterStart, originalStart;
    public float counterEnd;

    void Awake()
    {
        SetUpImportantComponents();
    }


    void Update()
    {
        if (cantBuy)
        {
            CantBuyCounter();
        }
    }

    void SetUpImportantComponents()
    {
        setUp = GetComponent<SetUpScreens>();
        money = GetComponent<Money>();
        weaponStates = GetComponent<WeaponStates>();

        weaponID = -1;
        originalStart = counterStart;
    }

    // Handles the buying action.
    public void BuyWeapon()
    {
        int weaponCost = weapons[weaponID].GetCost();
        int currency = money.GetCurrentCurrency();

        if (currency >= weaponCost)
        {
            money.ChangeCurrencyAmount(weaponCost);
            weaponStates.WhatWeaponWasBought(weaponID);
            SaveManager.SaveWeapons(weaponStates);
        }
        else
        {
            cantBuy = true;
        }
    }

    // Checks which button was pressed and returns id for the Buyweapon script.
    public void WeaponButton()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        if (buttonName == "WeaponA")
        {
            weaponID = weapons[1].GetID();
        }
        else if (buttonName == "WeaponB")
        {
            weaponID = weapons[2].GetID();
        }
        else if (buttonName == "WeaponC")
        {
            weaponID = weapons[3].GetID();
        }
    }

    // Function that announces, that player can't buy component and keeps this message going for couple of frames. 
    void CantBuyCounter()
    {
        //weaponDescription.text = "Don't have enough money for this component";

        counterStart += Time.deltaTime;
        if (counterStart >= counterEnd)
        {
            cantBuy = false;

            // weaponDescription.text = weapons[weaponID].GetDescription();
            counterStart = originalStart;
        }
    }

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
}
