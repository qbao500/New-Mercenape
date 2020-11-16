using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Created by Arttu Paldán 16.9.2020: This script allows the player to place upgrades into his weapon. 
public class UseUpgrades : MonoBehaviour
{
    SetUpForge setUpForge;
    WeaponStates weaponStates;
    Money money;
    StatsCalculator calculator;
    PlayerCurrency playerCurrency;
   
    List<AbstractUpgrades> upgrades;

    [SerializeField] int upgradeID, upgradeCost;

    public Text amountText;

    void Awake()
    {
        SetUpScripts();
        upgradeID = upgrades[0].GetID();
    }

    void SetUpScripts()
    {
        setUpForge = GetComponent<SetUpForge>();
        weaponStates = GetComponent<WeaponStates>();
        money = GetComponent<Money>();
        calculator = GetComponent<StatsCalculator>();
        playerCurrency = GetComponent<PlayerCurrency>();
    }

    // Button function for selecting the amount of upgrades to be inserted into the weapon.
    public void SelectUpgradeComponentsAmount()
    {
        string arrowButtonName = EventSystem.current.currentSelectedGameObject.name;
        int speedAmount = calculator.GetSpeedAmount();

        if (arrowButtonName == "Arrow1" && playerCurrency.speedUpgrades > 0)
        {
            speedAmount++;
            amountText.text = "" + speedAmount;
            upgradeCost = upgrades[upgradeID].GetUpgradeCost() * speedAmount;
            setUpForge.SetUpgradeCost(upgradeCost);
            playerCurrency.speedUpgrades--;
        }
        else if (arrowButtonName == "Arrow2" && speedAmount > 0)
        {
            upgradeID = upgrades[0].GetID();

            speedAmount--;
            amountText.text = "" + speedAmount;
            upgradeCost = upgrades[upgradeID].GetUpgradeCost() * speedAmount;
            setUpForge.SetUpgradeCost(upgradeCost);
            playerCurrency.speedUpgrades++;
        }

        calculator.SetSpeedAmount(speedAmount);
        setUpForge.SetScreen(weaponStates.GetChosenWeaponID());
    }

    // Confirm button function for confirming the updates. 
    public void ConfirmUpgrade()
    {
        int weaponID = weaponStates.GetChosenWeaponID();
        int currency = money.GetCurrentCurrency();

        if(currency >= upgradeCost)
        {
            money.ChangeCurrencyAmount(upgradeCost);
            weaponStates.WhatWeaponWasUpgraded(weaponID);
            weaponStates.SetUpWeapon();

            amountText.text = "" + 0;
            setUpForge.SetUpgradeCost(0);

            setUpForge.SetScreen(weaponStates.GetChosenWeaponID());
            calculator.SaveStats();
            calculator.SetSpeedAmount(0);

            SaveManager.SaveWeapons(weaponStates);
            SaveManager.SaveCurrency(playerCurrency);
        }
    }

    public void  SetUpgradeList(List<AbstractUpgrades> list) { upgrades = list; }
}
