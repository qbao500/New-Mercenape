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

    Text weaponStat;

    void Awake() { SetUpImportantComponents(); }

    void Start() {  weaponStat = setUpShop.GetStatText(); }

    void SetUpImportantComponents()
    {
        setUpShop = GetComponent<SetUpShop>();
        money = GetComponent<Money>();
        weaponStates = GetComponent<WeaponStates>();
        playerCurrency = GetComponent<PlayerCurrency>();

        boughtWeapons = weaponStates.GetBoughtWeapons();
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

            if(boughtWeapons[0] && boughtWeapons[1] ) { weaponID = -1; }
            else if(boughtWeapons[0] && boughtWeapons[1] == false) { weaponID = 1; }
            else if(boughtWeapons[0] == false && boughtWeapons[1]){ weaponID = 0;}
            
            setUpShop.SetScreen(weaponID);
        }
    }

    public void ScrollCatalogue()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        if (buttonName == "ChooseButton1Shop") { weaponID = 0;}
        else if (buttonName == "ChooseButton2Shop") { weaponID = 1;}

        setUpShop.SetScreen(weaponID);
    }

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }

    public int GetWeaponID() { return weaponID; }
}
