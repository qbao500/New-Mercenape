using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 17.9.2020: A script that handles the ownership issues of this system and sets up the weapon for level scene. 
public class WeaponStates: MonoBehaviour
{
    private StatsCalculator calculator;

    private List<AbstractWeapon> weapons;
    [SerializeField] private List<bool> ownedWeaponsList , upgradedWeaponsList;
    [SerializeField] private List<int> savedSpeedAmountsList;

    private int weaponID;

    [SerializeField] private float weaponSpeed, weaponImpactDamage, bleedDamage, bleedDuration;
    private int bleedTicks;

    void Awake()
    {
        calculator = GetComponent<StatsCalculator>();
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
        upgradedWeaponsList = new List<bool>(new bool[weapons.Count]);
        savedSpeedAmountsList = new List<int>(new int[weapons.Count]);

        ownedWeaponsList[0] = true;
    }

    // Function for setting the ownership status of a weapon.
    public void WhatWeaponWasBought(int id) { ownedWeaponsList[id] = true; }

    // Function for setting the upgrade status of a weapon. 
    public void WhatWeaponWasUpgraded(int id) { upgradedWeaponsList[id] = true; }

    public void UpdateStats(float speed, float damage) { weaponSpeed = speed; weaponImpactDamage = damage; }

    // This function sets up the weapon to be used in level scenes. 
    private void SetUpWeapon()
    {
        AbstractWeapon weaponsArray = weapons[weaponID];

        GameObject weaponModel = weaponsArray.GetWeaponModel();

        weaponModel.SetActive(true);

        calculator.SetRequestFromActualWeapon(true);
        calculator.CalculateStats();

        bleedDamage = weaponsArray.GetBleedDamage();
        bleedDuration = weaponsArray.GetBleedDuration();
        bleedTicks = weaponsArray.GetBleedTicks();
    }

    // Function for loading necessary data.
    void LoadWeaponData()
    {
        WeaponsData data = SaveManager.LoadWeapons();

        if(data != null)
        {
            weaponID = data.weaponID;
            ownedWeaponsList = data.ownedWeaponsList;
            upgradedWeaponsList = data.upgradedWeaponsList;
            savedSpeedAmountsList = data.savedSpeedAmountsList;
        }
    }

    // Set functions
    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
    public void SetChosenWeaponID(int id) { weaponID = id; }
    public void SetSavedSpeeds (List<int> list){ savedSpeedAmountsList = list; }


    // Get Functions
    public int GetChosenWeaponID() { return weaponID; }
    public List<bool> GetOwnedWeapons() { return ownedWeaponsList; }
    public List<bool> GetUpgradedWeapons() { return upgradedWeaponsList; }
    public List<int> GetSavedSpeeds() { return savedSpeedAmountsList; }
    public float GetWeaponSpeed() { return weaponSpeed; }
    public float GetWeaponImpactDamage() { return weaponImpactDamage; }
    public float GetWeaponBleedDamage() { return bleedDamage; }
    public float GetBleedDuration() { return bleedDuration; }
    public int GetWeaponBleedTicks() { return bleedTicks; }
}
