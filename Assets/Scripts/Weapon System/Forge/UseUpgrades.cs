﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Created by Arttu Paldán 16.9.2020: This script allows the player to place upgrades into his weapon. 
public class UseUpgrades : MonoBehaviour
{
    private SetUpScreens setUp;
    private WeaponStates weaponStates;
    private Money money;
    private StatsCalculator calculator;
    private PlayerCurrency playerCurrency;
   
    private List<AbstractWeapon> weapons;
    private List<AbstractUpgrades> upgrades;

    private int upgradeID, upgradeCost;

    void Awake()
    {
        SetUpScripts();
    }

    void SetUpScripts()
    {
        setUp = GetComponent<SetUpScreens>();
        weaponStates = GetComponent<WeaponStates>();
        money = GetComponent<Money>();
        calculator = GetComponent<StatsCalculator>();
        playerCurrency = GetComponent<PlayerCurrency>();
    }

    // Button function for selecting the amount of upgrades to be inserted into the weapon.
    //public void SelectUpgradeComponentsAmount()
    //{
    //    arrowButtonName = EventSystem.current.currentSelectedGameObject.name;

    //    int speedAmount = calculator.GetSpeedAmount();

    //    if (arrowButtonName == "Arrow1" && playerCurrency.speedUpgrades > 0)
    //    {
    //        upgradeID = upgrades[0].GetID();

    //        speedAmount++;
    //        amountTexts.text = "" + speedAmount;
    //        upgradeCost = upgrades[upgradeID].GetUpgradeCost() * speedAmount;
    //        playerCurrency.speedUpgrades--;
    //    }
    //    else if (arrowButtonName == "Arrow2" && speedAmount > 0)
    //    {
    //        upgradeID = upgrades[0].GetID();

    //        speedAmount--;
    //        amountTexts.text = "" + speedAmount;
    //        upgradeCost = upgrades[upgradeID].GetUpgradeCost() * speedAmount;
    //        playerCurrency.speedUpgrades++;
    //    }

    //    weaponCostText.text = "Upgrade Cost: " + upgradeCost;
    //    upgradeHolderUpgradeScreen.text = "Speed: " + playerCurrency.speedUpgrades;

    //    calculator.SetSpeedAmount(speedAmount);
    //    UpdateWeaponStats();
    //}

    // Confirm button function for confirming the updates. 
    public void ConfirmUpgrade()
    {
        int weaponID = weaponStates.GetChosenWeaponID();
        int currency = money.GetCurrentCurrency();

        if(currency >= upgradeCost)
        {
            money.ChangeCurrencyAmount(upgradeCost);
            weaponStates.WhatWeaponWasUpgraded(weaponID);

            setUp.UpdateWeaponStats();
            calculator.SaveStats();
            
            calculator.SetSpeedAmount(0);

            SaveManager.SaveCurrency(playerCurrency);
        }
    }

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
    public void  SetUpgradeList(List<AbstractUpgrades> list) { upgrades = list; }
}
