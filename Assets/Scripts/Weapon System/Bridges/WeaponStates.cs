using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 17.9.2020: A script that handles the ownership issues of this system and sets up the weapon for level scene. 
public class WeaponStates: MonoBehaviour
{
    private AssetManager assetManager;
    private StatsCalculator calculator;
    private PlayerAttackTrigger playerAttack;

    private List<AbstractWeapon> weapons, weaponsShop;
    [SerializeField] private List<bool> ownedWeaponsList, boughtWeaponsList, upgradedWeaponsList;
    [SerializeField] private List<int> savedSpeedAmountsList;
    [SerializeField] private List<WeaponInUse> weaponModels;

    private int weaponID;

    private float weaponSpeedForge, weaponImpactDamageForge, weaponSpeedShop, weaponImpactDamageShop, bleedDamage, bleedDuration;
    private int bleedTicks;

    void Awake()
    {
        assetManager = GetComponent<AssetManager>();
        calculator = GetComponent<StatsCalculator>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackTrigger>();
        SetUpBoolLists();
        LoadWeaponData();
    }

    void Start()
    {
        SetUpWeapon();
    }

    void SetUpBoolLists()
    {
        ownedWeaponsList = new List<bool>(new bool[weapons.Count]);
        boughtWeaponsList = new List<bool>(new bool[weaponsShop.Count]);
        upgradedWeaponsList = new List<bool>(new bool[weapons.Count]);
        savedSpeedAmountsList = new List<int>(new int[weapons.Count]);

        ownedWeaponsList[0] = true;
    }

    // Function for setting the ownership status of a weapon.
    public void WhatWeaponWasBought(int id) 
    {
        if(id == 0)
        {
            boughtWeaponsList[0] = true;
            ownedWeaponsList[1] = true;
        }
        else if (id == 1)
        {
            boughtWeaponsList[1] = true;
            ownedWeaponsList[2] = true;
        }
        else if (id == 2)
        {
            boughtWeaponsList[2] = true;
            ownedWeaponsList[3] = true;
        }
    }

    // Function for setting the upgrade status of a weapon. 
    public void WhatWeaponWasUpgraded(int id) { upgradedWeaponsList[id] = true; }
    public void UpdateStatsForge(float speed, float damage) { weaponSpeedForge = speed; weaponImpactDamageForge = damage; }
    public void UpdateStatsShop(float speed, float damage) { weaponSpeedShop = speed; weaponImpactDamageShop = damage; }

    // This function sets up the weapon to be used in level scenes. 
    public void SetUpWeapon()
    {
        AbstractWeapon weaponsArray = weapons[weaponID];

        calculator.SetRequestFromActualWeapon(true);
        calculator.CalculateStats();

        bleedDamage = weaponsArray.GetBleedDamage();
        bleedDuration = weaponsArray.GetBleedDuration();
        bleedTicks = weaponsArray.GetBleedTicks();

        playerAttack.SetWeaponStats();

        SwitchModels();
    }

    void SwitchModels()
    {
        weaponModels = assetManager.GetWeaponModels();

        for (int i = 0; i < weaponModels.Count; i++) { weaponModels[i].gameObject.SetActive(false); }

        weaponModels[weaponID].gameObject.SetActive(true);
    }

    // Function for loading necessary data.
    void LoadWeaponData()
    {
        WeaponsData data = SaveManager.LoadWeapons();

        if(data != null)
        {
            weaponID = data.weaponID;
            ownedWeaponsList = data.ownedWeaponsList;
            boughtWeaponsList = data.bougthWeaponList;
            upgradedWeaponsList = data.upgradedWeaponsList;
            savedSpeedAmountsList = data.savedSpeedAmountsList;
        }
    }

    // Set functions
    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
    public void SetWeaponsForShop(List<AbstractWeapon> list) { weaponsShop = list; }
    public void SetChosenWeaponID(int id) { weaponID = id; }
    public void SetSavedSpeeds (List<int> list){ savedSpeedAmountsList = list; }


    // Get Functions
    public int GetChosenWeaponID() { return weaponID; }
    public List<bool> GetOwnedWeapons() { return ownedWeaponsList; }
    public List<bool> GetBoughtWeapons() { return boughtWeaponsList; }
    public List<bool> GetUpgradedWeapons() { return upgradedWeaponsList; }
    public List<int> GetSavedSpeeds() { return savedSpeedAmountsList; }
    public float GetWeaponSpeedForge() { return weaponSpeedForge; }
    public float GetWeaponImpactDamageForge() { return weaponImpactDamageForge; }
    public float GetWeaponSpeedShop() { return weaponSpeedShop; }
    public float GetWeaponImpactDamageShop() { return weaponImpactDamageShop; }
    public float GetWeaponBleedDamage() { return bleedDamage; }
    public float GetBleedDuration() { return bleedDuration; }
    public int GetWeaponBleedTicks() { return bleedTicks; }
}
