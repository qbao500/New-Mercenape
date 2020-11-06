using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Arttu Paldán on 28.9.2020: Static class that handles the heavier method operations of BuyWeapons script. Not super necessary, mainly just a experiment of mine to see how static classes work.
// Really this only cleans up the BuyWeapon script by hiding these heavy operations to the background.
public static class BuyOperations
{
    // Gets the important components for BuyWeapon script, like for example the other scripts used by BuyWeapons.
    public static void SetUpImportantComponents(BuyWeapons buy, float start)
    {
        ChooseWeapon chooseWeapon = buy.GetComponent<ChooseWeapon>();
        Money money = buy.GetComponent<Money>();
        WeaponStates weaponStates = buy.GetComponent<WeaponStates>();

        buy.SetChooseWeapon(chooseWeapon);
        buy.SetMoney(money);
        buy.SetWeaponStates(weaponStates);
        
        Text weaponName = GameObject.FindGameObjectWithTag("WeaponName").GetComponent<Text>();
        Text weaponDescription = GameObject.FindGameObjectWithTag("WeaponDescription").GetComponent<Text>();
        Text weaponCostText = GameObject.FindGameObjectWithTag("WeaponCost").GetComponent<Text>();
        Image weaponImageBuyScreen = GameObject.FindGameObjectWithTag("BuyScreenWeaponImage").GetComponent<Image>();
        GameObject buyWeaponScreen = GameObject.FindGameObjectWithTag("BuyScreenWeaponImage");

        buy.SetWeaponNameText(weaponName);
        buy.SetWeaponDescText(weaponDescription);
        buy.SetWeaponCostText(weaponCostText);
        buy.SetBuyScreenWeaponImage(weaponImageBuyScreen);
        buy.SetBuyScreen(buyWeaponScreen);

        buy.SetWeaponID(-1);
        buy.SetOriginalCounterStart(start);
    }

    public static void GetWeaponHolders(BuyWeapons buy, List<Image> weaponHolders)
    {
        GameObject[] holdersObjects = GameObject.FindGameObjectsWithTag("WeaponHolder");
        Image[] holderImages = new Image[holdersObjects.Length];

        for (int i = 0; i < holderImages.Length; i++)
        {
            holderImages[i] = holdersObjects[i].GetComponent<Image>();
        }

        weaponHolders.Add(holderImages[0]);
        weaponHolders.Add(holderImages[1]);
        weaponHolders.Add(holderImages[2]);

        buy.SetWeaponHolders(weaponHolders);
    }

    public static void SetWeaponsHolder(List<AbstractWeapon> weapons, List<bool> ownedBools, List<Image> weaponImagesHolder)
    {
        for (int i = 0; i < weaponImagesHolder.Count; i++)
        {
            switch (ownedBools[i + 1])
            {
                case true:
                    weaponImagesHolder[i].sprite = null;
                    weaponImagesHolder[i].enabled = false;
                    break;

                case false:
                    weaponImagesHolder[i].sprite = weapons[i + 1].GetWeaponImage();
                    break;
            }
        }
    }

    // Sets up the buyweapon screen in game.
    public static void SetBuyScreen(List<AbstractWeapon> weaponsList, int id, Image weaponImage, Text weaponName, Text weaponDescription, Text weaponCostText)
    {
        int weaponCost = weaponsList[id].GetCost();

        weaponImage.sprite = weaponsList[id].GetWeaponImage();
        weaponName.text = weaponsList[id].GetName();
        weaponDescription.text = weaponsList[id].GetDescription();
        weaponCostText.text = "Cost: " + weaponCost;
    }

    // Handles the buying action.
    public static void BuyWeapon(BuyWeapons buy, WeaponStates weaponStates, Money money, List<AbstractWeapon> weaponsList, List<Image> weaponHolders, List<Image> ownedWeapons, GameObject buyScreen, int id)
    {
        int holderID = id - 1;
        int weaponCost = weaponsList[id].GetCost();
        int currency = money.GetCurrentCurrency();

        if (currency >= weaponCost)
        {
            money.ChangeCurrencyAmount(weaponCost);
            weaponStates.WhatWeaponWasBought(id);

            weaponHolders[holderID].enabled = false;
            ownedWeapons[id].sprite = weaponsList[id].GetWeaponImage();

            SaveManager.SaveWeapons(weaponStates);
            buyScreen.SetActive(false);
        }
        else
        { 
            buy.SetCantBuy(true);
        }
    }

    // Checks which button was pressed and returns id for the Buyweapon script.
    public static void WeaponButtonPress(BuyWeapons buy, string button, List<AbstractWeapon> weapons)
    {
        int id;

        if (button == "WeaponA")
        {
            id = weapons[1].GetID();
            buy.SetWeaponID(id);
        }
        else if (button == "WeaponB")
        {
            id = weapons[2].GetID();
            buy.SetWeaponID(id);
        }
        else if (button == "WeaponC")
        {
            id = weapons[3].GetID();
            buy.SetWeaponID(id);
        }
    }
}
