using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán on 13.11.2020: 
public class SetUpShop : SetUpScreens
{
    protected override void Awake() { base.Awake(); }

    protected override void Start() 
    {
        List<bool> ownedWeapons = weaponStates.GetOwnedWeapons();
        int firstfalse = ownedWeapons.IndexOf(false);
        weaponID = firstfalse;

        base.Start();
    }

    protected override void UpdateTexts()
    {
        base.UpdateTexts();

        costText.text = "Buy Cost: " + cost;
        upgradeText.text = "Speed Upgrades: " + playerCurrency.speedUpgrades;
    }

    protected override void SwitchModels()
    {
        List<bool> ownedWeapons = weaponStates.GetOwnedWeapons();

        for (int i = 0; i < weaponInventory.Count; i++)
        {
            switch (ownedWeapons[i + 1])
            {
                case true:
                    weaponInventory[i].SetActive(false);
                    break;

                case false:
                    weaponInventory[i].SetActive(true);
                    break;
            }
        }

        for (int i = 0; i < weaponImages.Count; i++)
        {
            switch (ownedWeapons[i])
            {
                case true:
                    weaponImages[i].SetActive(false);
                    break;

                case false:
                    weaponImages[i].SetActive(false);
                    int firstfalse = ownedWeapons.IndexOf(false);
                    weaponImages[firstfalse].SetActive(true);
                    break;
            }
        }
    }
}
