using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Created by Arttu Paldán 11.9.2020: This script will handle buying or unlocking component pieces.
public class BuyWeapons : MonoBehaviour
{
    private SetUpShop setUpShop;
    private Money money;
    private WeaponStates weaponStates;
    private PlayerCurrency playerCurrency;

    private List<AbstractWeapon> weapons;
    private List<bool> boughtWeapons;

    [SerializeField] private int weaponID;
    [SerializeField] private List<int> forbiddenID;

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
        setUpShop = GetComponent<SetUpShop>();
        money = GetComponent<Money>();
        weaponStates = GetComponent<WeaponStates>();
        playerCurrency = GetComponent<PlayerCurrency>();

        boughtWeapons = weaponStates.GetBoughtWeapons();
        InitializeForbiddenID();
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
            SaveManager.SaveCurrency(playerCurrency);
            
            AddForbiddenID();
            weaponID = weaponID + 1;
            CheckID();
            setUpShop.SetScreen(weaponID);
        }
        else
        {
            cantBuy = true;
        }
    }

    public void ScrollCatalogue()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        CheckID();

        if (buttonName == "ScrollArrowRight")
        {
            weaponID = weaponID + 1;

            if (weaponID == forbiddenID[weaponID]) { weaponID = weaponID - 1; }
        }
        else if (buttonName == "ScrollArrowLeft" && weaponID >= 0)
        {
            weaponID = weaponID - 1;

            if (weaponID == forbiddenID[weaponID]) { weaponID = weaponID + 1; }
        }

        setUpShop.SetScreen(weaponID);
    }

    void InitializeForbiddenID()
    {
        forbiddenID = new List<int>(new int[boughtWeapons.Count]);

        for(int i = 0; i < forbiddenID.Count; i++) { forbiddenID[i] = -1; }

        if (boughtWeapons[0]) { forbiddenID.Insert(0, 0); }
        if (boughtWeapons[1]) { forbiddenID.Insert(1, 1); }
        if (boughtWeapons[2]) { forbiddenID.Insert(2, 2); }
    }

    void AddForbiddenID() { forbiddenID.Insert(weaponID, weaponID); }

    public void CheckID()
    {
        if (boughtWeapons[0]) { weaponID = 1; }
        if (boughtWeapons[1]) { weaponID = 2; }
        if (boughtWeapons[2]) { weaponID = 0; }
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

    public int GetWeaponID() { return weaponID; }
}
