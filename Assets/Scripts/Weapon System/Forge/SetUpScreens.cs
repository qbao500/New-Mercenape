using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Created by Arttu Paldán on 13.11.2020: Made this replace all of the screen setups scattered into other scripts and made this more centralized system. 
public class SetUpScreens : MonoBehaviour
{
    WeaponStates weaponStates;
    StatsCalculator statsCalculator;
    PlayerCurrency playerCurrency;

    List<AbstractWeapon> weapons;

    [SerializeField] List<GameObject> weaponInventory, weaponImages;
    GameObject toShopButton, toForgeButton;

    [SerializeField] GameObject shopTitleInactive, shopTitleActive, forgeTitleInactive, forgeTitleActive;
    Text costText, upgradeText;

    [SerializeField] List<Text> weaponStatTexts;
    public int amountOfTexts;

    int weaponID, upgradeCost;

    void Awake()
    {
        GetScripts();
    }

    void Start()
    {
        GetRequiredComponents();
        SetForgeScreen();
    }

    void GetScripts()
    {
        weaponStates = GetComponent<WeaponStates>();
        statsCalculator = GetComponent<StatsCalculator>();
        playerCurrency = GetComponent<PlayerCurrency>();
    }

    void GetRequiredComponents()
    {
        weaponID = weaponStates.GetChosenWeaponID();

        weaponInventory.InsertRange(0, GameObject.FindGameObjectsWithTag("WeaponHolder"));
        weaponImages.InsertRange(0, GameObject.FindGameObjectsWithTag("ChosenWeaponHolder"));

        toShopButton = GameObject.FindGameObjectWithTag("ToShop");
        toForgeButton = GameObject.FindGameObjectWithTag("ToForge");

        shopTitleInactive = GameObject.FindGameObjectWithTag("ShopTitleInactive");
        shopTitleActive = GameObject.FindGameObjectWithTag("ShopTitleActive");
        forgeTitleInactive = GameObject.FindGameObjectWithTag("ForgeTitleInactive");
        forgeTitleActive = GameObject.FindGameObjectWithTag("ForgeTitleActive");

        costText = GameObject.FindGameObjectWithTag("CostText").GetComponent<Text>();
        upgradeText = GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<Text>();

        GameObject[] statTextObjects = GameObject.FindGameObjectsWithTag("WeaponStatText");
        Text[] statTexts = new Text[amountOfTexts];

        for(int i = 0; i < statTextObjects.Length; i++)
        {
            statTexts[i] = statTextObjects[i].GetComponent<Text>();
        }
    
        weaponStatTexts.InsertRange(0, statTexts);
    }

    public void SetForgeScreen()
    {
        SwitchModels();
        UpdateWeaponStats();
        UpdateTexts();
    }

    public void UpdateWeaponStats()
    {
        statsCalculator.SetRequestFromUpgrades(true);
        statsCalculator.CalculateStats();
    }

    void UpdateTexts()
    {
        AbstractWeapon weaponsArray = weapons[weaponID];

        weaponStatTexts[0].text = weaponsArray.GetName();
        weaponStatTexts[1].text = weaponsArray.GetDescription();
        weaponStatTexts[2].text = weaponsArray.GetWeight().ToString();
        weaponStatTexts[3].text = weaponsArray.GetSpeed().ToString();
        weaponStatTexts[4].text = statsCalculator.GetSpeed().ToString();
        weaponStatTexts[5].text = statsCalculator.GetImpactDamage().ToString();
        weaponStatTexts[6].text = weaponsArray.GetBleedDamage().ToString();
        weaponStatTexts[7].text = weaponsArray.GetBleedDuration().ToString();
        weaponStatTexts[8].text = weaponsArray.GetStaggerDuration().ToString();

        costText.text = "Upgrade Cost: " + upgradeCost;
        upgradeText.text = "Speed Upgrades: " + playerCurrency.speedUpgrades;
    }

    void SwitchModels()
    {
        List<bool> ownedWeaponsList = weaponStates.GetOwnedWeapons();

        for (int i = 0; i < weaponInventory.Count; i++)
        {
            switch (ownedWeaponsList[i])
            {
                case true:
                    weaponInventory[i].SetActive(true);
                    break;

                case false:
                    weaponInventory[i].SetActive(false);
                    break;
            }
        }

        for (int i = 0; i < weaponImages.Count; i++)
        {
            weaponImages[i].SetActive(false);
        }

        weaponImages[weaponID].SetActive(true);
    }

    public void ChangeTitleImages()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        if(buttonName == "weapon")
        {
            shopTitleActive.SetActive(false);
            shopTitleInactive.SetActive(true);

            forgeTitleActive.SetActive(true);
            forgeTitleInactive.SetActive(false);

            toShopButton.SetActive(true);
            toForgeButton.SetActive(false);

        }
        else if(buttonName == "ToShop")
        {
            shopTitleActive.SetActive(true);
            shopTitleInactive.SetActive(false);

            forgeTitleActive.SetActive(false);
            forgeTitleInactive.SetActive(true);

            toShopButton.SetActive(false);
            toForgeButton.SetActive(true);
        }
        else if (buttonName == "ToForge")
        {
            shopTitleActive.SetActive(false);
            shopTitleInactive.SetActive(true);

            forgeTitleActive.SetActive(true);
            forgeTitleInactive.SetActive(false);

            toShopButton.SetActive(true);
            toForgeButton.SetActive(false);
        }
    }

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
}
