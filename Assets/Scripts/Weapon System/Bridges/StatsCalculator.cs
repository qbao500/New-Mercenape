using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 23.9.2020: This script handles weapon stat calculations. 
public class StatsCalculator : MonoBehaviour
{
    private WeaponStates weaponStates;
    
    private List<AbstractWeapon> weapons;

    private bool requestCameFromUseUpgrades, requestCameFromSetActualWeapon;

    private int weaponID;

    private int amountOfSpeed, savedAmountOfSpeed;

    [SerializeField] private float weaponWeight, weaponSpeed, weaponImpactDamage;

    private float actualWeaponSpeed, actualWeaponImpactDamage;

    void Awake()
    {
        weaponStates = GetComponent<WeaponStates>();
    }

    // Public function that other scripts can call to handle the weapon stat calculations.
    public void CalculateStats()
    {
        if (requestCameFromUseUpgrades)
        {
            weaponID = weaponStates.GetChosenWeaponID();
        }
        else if (requestCameFromSetActualWeapon)
        {
            weaponID = weaponStates.GetChosenWeaponID();
        }

        List<bool> upgradedWeaponsList = weaponStates.GetUpgradedWeapons();
        bool hasBeenUpgraded = upgradedWeaponsList[weaponID];

        if (weaponID == 0 && hasBeenUpgraded)
        {
            CalculateWithSavedStats();
        }
        else if (weaponID == 1 && hasBeenUpgraded)
        {
            CalculateWithSavedStats();
        }
        else if (weaponID == 2 && hasBeenUpgraded)
        {
            CalculateWithSavedStats();
        }
        else if (weaponID == 3 && hasBeenUpgraded)
        {
            CalculateWithSavedStats();
        }
        else
        {
            CalculateNormally();
        }
    }

    // Function for calculating stats in situations, where player has upgraded the weapon.
    void CalculateWithSavedStats()
    {
        AbstractWeapon weaponsArray = weapons[weaponID];

        LoadSaveFiles();

        savedAmountOfSpeed = savedAmountOfSpeed + amountOfSpeed;
        weaponWeight = weaponsArray.GetWeight();
        weaponSpeed = weaponsArray.GetSpeed();
        weaponImpactDamage = weaponsArray.GetImpactDamage();

        actualWeaponSpeed = weaponSpeed + (savedAmountOfSpeed * 0.1f) - (weaponWeight * 0.1f);
        actualWeaponImpactDamage = weaponImpactDamage + weaponWeight;

        weaponStates.UpdateStats(actualWeaponSpeed, actualWeaponImpactDamage);
    }

    // Function for calculating weapon stats when changes have not been made or when player upgrades the weapon.
    void CalculateNormally()
    {
        AbstractWeapon weaponsArray = weapons[weaponID];

        weaponWeight = weaponsArray.GetWeight();
        weaponSpeed = weaponsArray.GetSpeed();
        weaponImpactDamage = weaponsArray.GetImpactDamage();

        actualWeaponSpeed = weaponSpeed + (amountOfSpeed * 0.1f) - (weaponWeight/10);
      
        actualWeaponImpactDamage =  weaponImpactDamage + weaponWeight;

        weaponStates.UpdateStats(actualWeaponSpeed, actualWeaponImpactDamage);
    }

    // Function for saving the amount of upgrades.
    public void SaveStats()
    {
        List<int> savedSpeedsList = weaponStates.GetSavedSpeeds();

        switch (weaponID)
        {
            case 0:
                savedSpeedsList[0] = savedSpeedsList[0] + amountOfSpeed;
                break;

            case 1:
                savedSpeedsList[1] = savedSpeedsList[1] + amountOfSpeed;
                break;

            case 2:
                savedSpeedsList[2] = savedSpeedsList[2] + amountOfSpeed;
                break;

            case 3:
                savedSpeedsList[3] = savedSpeedsList[3] + amountOfSpeed;
                break;
        }

        weaponStates.SetSavedSpeeds(savedSpeedsList);
        SaveManager.SaveWeapons(weaponStates);
    }

    // Function for loading the amount of upgrades. 
    void LoadSaveFiles()
    {
        List<int> savedSpeedsList = weaponStates.GetSavedSpeeds();

        switch (weaponID)
        {
            case 0:
                savedAmountOfSpeed = savedSpeedsList[0];
                break;

            case 1:
                savedAmountOfSpeed = savedSpeedsList[1];
                break;

            case 3:
                savedAmountOfSpeed = savedSpeedsList[2];
                break;

            case 4:
                savedAmountOfSpeed = savedSpeedsList[3];
                break;
        }
    }

    // Fetch functions for other scripts to get their hands on these private values.
    public float GetSpeed() { return actualWeaponSpeed; }
    public float GetImpactDamage() { return actualWeaponImpactDamage; }

    public int GetSpeedAmount() { return amountOfSpeed; }
    public int GetSavedSpeedAmount() { return savedAmountOfSpeed; }


    // Set functions.
    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
    public void SetSpeedAmount(int amount) { amountOfSpeed = amount; }
    public void SetSavedSpeedAmount(int amount) { savedAmountOfSpeed = amount; }

    public void SetSpeed(float speed) { actualWeaponSpeed = speed; }
    public void SetImpactDamage(float damage) { actualWeaponImpactDamage = damage; }

    public void SetRequestFromUpgrades(bool request) { requestCameFromUseUpgrades = request; }
    public void SetRequestFromActualWeapon(bool request) { requestCameFromSetActualWeapon = request; }
}
