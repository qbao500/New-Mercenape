using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Created by Arttu Paldán 16.9.2020: This script allows the player to place upgrades into his weapon. 
public class UseUpgrades : MonoBehaviour
{
    private WeaponStates weaponStates;
    private Money money;
    private StatsCalculator calculator;
    private PlayerCurrency playerCurrency;
   
    private List<AbstractWeapon> weapons;
    private List<AbstractUpgrades> upgrades;

    private int upgradeID, upgradeCost;

    private string upgradeButtonName, arrowButtonName;

    private Text weaponName, weaponDescription, weaponSpeedText, weaponWeightText, weaponImpactDamageText, weaponCostText, amountTexts;
    private Text upgradeHolderUpgradeScreen, upgradeName, upgradeDescription;
   
    private Image weaponImage, upgradeImage;
    private Image upgradeImagesHolder;
   
    private GameObject upgradeScreen, upgradeComponentScreen;

    void Awake()
    {
        GetRequiredObjects();
        SetScreensInactive();
    }

    void Start()
    {
        SetUpUpgradeScreen();
    }

    void Update()
    {
        if(upgradeComponentScreen.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUpgradeComponentMenu();
        }
    }

    void GetRequiredObjects()
    {
        weaponStates = GetComponent<WeaponStates>();
        money = GetComponent<Money>();
        calculator = GetComponent<StatsCalculator>();
        playerCurrency = GetComponent<PlayerCurrency>();

        weaponName = GameObject.FindGameObjectWithTag("UpgradeScreenWeaponName").GetComponent<Text>();
        weaponDescription = GameObject.FindGameObjectWithTag("UpgradeScreenWeaponDescription").GetComponent<Text>();
        weaponSpeedText = GameObject.FindGameObjectWithTag("WeaponSpeed").GetComponent<Text>();
        weaponWeightText = GameObject.FindGameObjectWithTag("WeaponWeight").GetComponent<Text>();
        weaponImpactDamageText = GameObject.FindGameObjectWithTag("ImpactDamage").GetComponent<Text>();
        weaponCostText = GameObject.FindGameObjectWithTag("UpgradeCost").GetComponent<Text>();
        weaponImage = GameObject.FindGameObjectWithTag("UpgradeScreenWeaponImage").GetComponent<Image>();
        upgradeImagesHolder = GameObject.FindGameObjectWithTag("UpgradeImageHolder").GetComponent<Image>();
        amountTexts = GameObject.FindGameObjectWithTag("AmountText").GetComponentInChildren<Text>();

        upgradeScreen = GameObject.FindGameObjectWithTag("Forge");
        upgradeComponentScreen = GameObject.FindGameObjectWithTag("UpgradeImage");
        upgradeName = GameObject.FindGameObjectWithTag("UpgradeName").GetComponent<Text>();
        upgradeDescription = GameObject.FindGameObjectWithTag("UpgradeDescription").GetComponent<Text>();
        upgradeImage = GameObject.FindGameObjectWithTag("UpgradeImage").GetComponent<Image>();
        upgradeHolderUpgradeScreen = GameObject.FindGameObjectWithTag("UpgradesUpgradeScreen").GetComponentInChildren<Text>();
    }

    void SetScreensInactive()
    {
        upgradeComponentScreen.SetActive(false);
    }

    public void OpenUpgradeScreen()
    {
        upgradeScreen.SetActive(true);
    }
    
    // This function sets up the upgrade screen based on what weapon player has chosen from his owned weapons inventory. 
    public void SetUpUpgradeScreen()
    {
        int weaponID = weaponStates.GetChosenWeaponID();

        AbstractWeapon weaponsArray = weapons[weaponID];

        weaponName.text = weaponsArray.GetName();
        weaponDescription.text = weaponsArray.GetDescription();
        weaponCostText.text = "Upgrade Cost: " + upgradeCost;
        weaponImage.sprite = weaponsArray.GetWeaponImage();

        upgradeHolderUpgradeScreen.text = "Speed: " + playerCurrency.speedUpgrades;

        UpdateWeaponStats();

        upgradeImagesHolder.sprite = upgrades[0].GetUpgradeImage();
       
        amountTexts.text = "0";
    }

    // Button function for opening an screen that will explain, what the upgrade does when put into the weapon. 
    public void OpenUpgradeComponent()
    {
        upgradeButtonName = EventSystem.current.currentSelectedGameObject.name;

        if (upgradeButtonName == "UpgradeComponent1")
        {
            upgradeID = upgrades[0].GetID();
        }
        
        SetUpUpgradeComponentScreen();
    }

    // Sets up the upgrade explanation screen. 
    void SetUpUpgradeComponentScreen()
    {
        AbstractUpgrades upgradesArray = upgrades[upgradeID];

        upgradeComponentScreen.SetActive(true);

        upgradeName.text = upgradesArray.GetName();
        upgradeDescription.text = upgradesArray.GetDescription();
        upgradeImage.sprite = upgradesArray.GetUpgradeImage();
    }

    //Function for closing the componentscreen.
    public void CloseUpgradeComponentMenu()
    {
        upgradeComponentScreen.SetActive(false);
        upgradeID = -1;
    }

    // Button function for selecting the amount of upgrades to be inserted into the weapon.
    public void SelectUpgradeComponentsAmount()
    {
        arrowButtonName = EventSystem.current.currentSelectedGameObject.name;

        int speedAmount = calculator.GetSpeedAmount();

        if (arrowButtonName == "Arrow1" && playerCurrency.speedUpgrades > 0)
        {
            upgradeID = upgrades[0].GetID();

            speedAmount++;
            amountTexts.text = "" + speedAmount;
            upgradeCost = upgrades[upgradeID].GetUpgradeCost() * speedAmount;
            playerCurrency.speedUpgrades--;
        }
        else if (arrowButtonName == "Arrow2" && speedAmount > 0)
        {
            upgradeID = upgrades[0].GetID();

            speedAmount--;
            amountTexts.text = "" + speedAmount;
            upgradeCost = upgrades[upgradeID].GetUpgradeCost() * speedAmount;
            playerCurrency.speedUpgrades++;
        }

        weaponCostText.text = "Upgrade Cost: " + upgradeCost;
        upgradeHolderUpgradeScreen.text = "Speed: " + playerCurrency.speedUpgrades;

        calculator.SetSpeedAmount(speedAmount);
        UpdateWeaponStats();
    }

    // Function for updating the info on weapon stats in upgrade screen.
    void UpdateWeaponStats()
    {
        int weaponID = weaponStates.GetChosenWeaponID();

        calculator.SetRequestFromUpgrades(true);
        calculator.CalculateStats();

        float weight = weapons[weaponID].GetWeight();
        float speed = weaponStates.GetWeaponSpeed();
        float damage = weaponStates.GetWeaponImpactDamage();

        weaponWeightText.text = " Weight: " + weight;
        weaponSpeedText.text = "Speed: " + speed;
        weaponImpactDamageText.text = " Impact Damage: " + damage;
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
            
            upgradeCost = 0;
            weaponCostText.text = "Upgrade Cost: " + upgradeCost;

            UpdateWeaponStats();
            calculator.SaveStats();
            
            calculator.SetSpeedAmount(0);

            amountTexts.text = "0";

            SaveManager.SaveCurrency(playerCurrency);
        }
    }

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
    public void  SetUpgradeList(List<AbstractUpgrades> list) { upgrades = list; }
}
