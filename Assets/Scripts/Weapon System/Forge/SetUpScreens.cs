using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Arttu Paldán on 13.11.2020: Made this replace all of the screen setups scattered into other scripts and made this more centralized system. 
public abstract class SetUpScreens : MonoBehaviour
{
    protected WeaponStates weaponStates;
    protected StatsCalculator statsCalculator;
    protected PlayerCurrency playerCurrency;
    protected UseUpgrades useUpgrades;

    protected List<AbstractWeapon> weapons, weaponsShop;

    [SerializeField] protected List<GameObject> weaponInventory, weaponImages, chooseButtons;
    [SerializeField] protected List<Text> weaponStatTexts;
    
    protected Text costText, upgradeText;

    public int amountOfTexts;

    public string weaponHolderTag, chosenWeaponTag, costTag, upgradeTag, weaponStatsTag, chooseButtonTag;

    [SerializeField] protected int weaponID, cost;


    protected virtual void Awake() { GetScripts(); }

    protected virtual void Start() { GetRequiredComponents(); }

    protected void GetScripts()
    {
        weaponStates = GetComponent<WeaponStates>();
        statsCalculator = GetComponent<StatsCalculator>();
        playerCurrency = GetComponent<PlayerCurrency>();
        useUpgrades = GetComponent<UseUpgrades>();
    }

    protected void GetRequiredComponents()
    {
        // weaponInventory.InsertRange(0, GameObject.FindGameObjectsWithTag(weaponHolderTag));
        // weaponImages.InsertRange(0, GameObject.FindGameObjectsWithTag(chosenWeaponTag));
        // chooseButtons.InsertRange(0, GameObject.FindGameObjectsWithTag(chooseButtonTag));

        costText = GameObject.FindGameObjectWithTag(costTag).GetComponent<Text>();
        upgradeText = GameObject.FindGameObjectWithTag(upgradeTag).GetComponent<Text>();

        GameObject[] statTextObjects = GameObject.FindGameObjectsWithTag(weaponStatsTag);
        Text[] statTexts = new Text[amountOfTexts];

        for(int i = 0; i < statTextObjects.Length; i++)
        {
            statTexts[i] = statTextObjects[i].GetComponent<Text>();
        }
    
        weaponStatTexts.InsertRange(0, statTexts);
    }

    public virtual void SetScreen(int ID)
    {
        weaponID = ID;
        SwitchModels();
        UpdateWeaponStats();
        UpdateTexts();
    }

    public virtual void ResetSpeedUpgrades()
    {
        playerCurrency.speedUpgrades = playerCurrency.speedUpgrades + statsCalculator.GetSpeedAmount();
        statsCalculator.SetSpeedAmount(0);
        useUpgrades.amountText.text = "" + 0;
    }

    protected virtual void UpdateWeaponStats()
    {
        statsCalculator.CalculateStats();
    }

    protected abstract void UpdateTexts();

    protected abstract void SwitchModels();

    public void SetWeaponList(List<AbstractWeapon> list, List<AbstractWeapon> listII) { weapons = list; weaponsShop = listII; }
    public void SetUpgradeCost(int loss) { cost = loss; }

    public int GetWeaponID() { return weaponID; }
}
