using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán on 13.11.2020: 
public class SetUpForge : SetUpScreens
{
    protected override void UpdateWeaponStats()
    {
        statsCalculator.SetRequestFromForge(true);
        base.UpdateWeaponStats();
    }

    protected override void UpdateTexts()
    {
        AbstractWeapon weapon = weapons[weaponID];

        weaponStatTexts[0].text = weapon.GetName();
        weaponStatTexts[1].text = weapon.GetDescription();
        weaponStatTexts[2].text = weapon.GetWeight().ToString();
        weaponStatTexts[3].text = weapon.GetSpeed().ToString();
        weaponStatTexts[4].text = weaponStates.GetWeaponSpeedForge().ToString();
        weaponStatTexts[5].text = weaponStates.GetWeaponImpactDamageForge().ToString();
        weaponStatTexts[6].text = weapon.GetBleedDamage().ToString();
        weaponStatTexts[7].text = weapon.GetBleedDuration().ToString();
        weaponStatTexts[8].text = weapon.GetStaggerDuration().ToString();

        costText.text = cost.ToString();
        upgradeText.text = playerCurrency.speedUpgrades.ToString();
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

        for(int i = 0; i < chooseButtons.Count; i++)
        {
            switch (ownedWeapons[i])
            {
                case true:
                    chooseButtons[i].SetActive(true);
                    break;

                case false:
                    chooseButtons[i].SetActive(false);
                    break;
            }
        }

        for (int i = 0; i < weaponImages.Count; i++)
        {
            weaponImages[i].SetActive(false);
            weaponImages[weaponID = weaponStates.GetChosenWeaponID()].SetActive(true);
        }
    }
}
