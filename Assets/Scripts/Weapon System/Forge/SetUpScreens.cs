using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Created by Arttu Paldán on 13.11.2020: Made this replace all of the screen setups scattered into other scripts and made this more centralized system. 
public abstract class SetUpScreens : MonoBehaviour
{
    protected WeaponStates weaponStates;
    protected StatsCalculator statsCalculator;
    protected PlayerCurrency playerCurrency;

    protected List<AbstractWeapon> weapons;

    [SerializeField] protected List<GameObject> weaponInventory, weaponImages;

    protected Text costText, upgradeText;

    [SerializeField] protected List<Text> weaponStatTexts;
    public int amountOfTexts;

    public string weaponHolderTag, chosenWeaponTag, costTag, upgradeTag, weaponStatsTag;

    [SerializeField] protected int weaponID, cost;


    protected virtual void Awake()
    {
        GetScripts();
    }

    protected virtual void Start()
    {
        GetRequiredComponents();
        SetScreen();
    }

    void GetScripts()
    {
        weaponStates = GetComponent<WeaponStates>();
        statsCalculator = GetComponent<StatsCalculator>();
        playerCurrency = GetComponent<PlayerCurrency>();
    }

    void GetRequiredComponents()
    {
        weaponInventory.InsertRange(0, GameObject.FindGameObjectsWithTag(weaponHolderTag));
        weaponImages.InsertRange(0, GameObject.FindGameObjectsWithTag(chosenWeaponTag));

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

    public virtual void SetScreen()
    {
        SwitchModels();
        UpdateWeaponStats();
        UpdateTexts();
    }

    public virtual void UpdateWeaponStats()
    {
        statsCalculator.SetRequestFromUpgrades(true);
        statsCalculator.CalculateStats();
    }

    protected virtual void UpdateTexts()
    {
        AbstractWeapon weaponsArray = weapons[weaponID];

        weaponStatTexts[0].text = weaponsArray.GetName();
        weaponStatTexts[1].text = weaponsArray.GetDescription();
        weaponStatTexts[2].text = "Weight: " + weaponsArray.GetWeight().ToString();
        weaponStatTexts[3].text = "Default Speed: " + weaponsArray.GetSpeed().ToString();
        weaponStatTexts[4].text = "Upgrades speed: " + statsCalculator.GetSpeed().ToString();
        weaponStatTexts[5].text = "Impact Damage: " + statsCalculator.GetImpactDamage().ToString();
        weaponStatTexts[6].text = "Bleed Damage: " + weaponsArray.GetBleedDamage().ToString();
        weaponStatTexts[7].text = "Bleed Duration: " + weaponsArray.GetBleedDuration().ToString();
        weaponStatTexts[8].text = "Stagger Duration Increase: " + weaponsArray.GetStaggerDuration().ToString();
    }

    protected abstract void SwitchModels();

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
    public void SetUpgradeCost(int loss) { cost = loss; }
}
