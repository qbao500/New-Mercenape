using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán on 13.11.2020: 
public class SetUpForge : SetUpScreens
{
    protected override void Awake() { base.Awake(); }

    protected override void Start() { weaponID = weaponStates.GetChosenWeaponID(); base.Start();  }

    protected override void UpdateTexts()
    {
        base.UpdateTexts();

        costText.text = "Upgrade Cost: " + cost;
        upgradeText.text = "Speed Upgrades: " + playerCurrency.speedUpgrades;
    }

    protected override void SwitchModels()
    {
        List<bool> ownedWeapons = weaponStates.GetOwnedWeapons();

        for(int i = 0; i < weaponInventory.Count; i++)
        {
            switch (ownedWeapons[i])
            {
                case true:
                    weaponInventory[i].SetActive(true);
                    break;

                case false:
                    weaponInventory[i].SetActive(false);
                    break;
            }
        }

        for(int i = 0; i < weaponImages.Count; i++)
        {
            weaponImages[i].SetActive(false);
        }

        weaponImages[weaponID].SetActive(true);
    }
}
