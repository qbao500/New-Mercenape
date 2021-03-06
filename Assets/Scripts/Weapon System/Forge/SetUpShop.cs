﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Arttu Paldán on 13.11.2020: 
public class SetUpShop : SetUpScreens
{
    BuyWeapons buyWeapons;

    protected override void Awake() { base.Awake(); buyWeapons = GetComponent<BuyWeapons>(); }

    protected override void Start() { base.Start(); }

    protected override void UpdateWeaponStats() { statsCalculator.SetRequestFromShop(true); base.UpdateWeaponStats(); }

    protected override void UpdateTexts()
    {
        AbstractWeapon weapon;
        List<bool> boughtWeapons = weaponStates.GetBoughtWeapons();

        if (boughtWeapons[0] && boughtWeapons[1]) { weapon = weaponsShop[2]; }
        else { weapon = weaponsShop[buyWeapons.GetWeaponID()]; }
      
        for (int i = 0; i < weaponNames.Count; i++) { weaponNames[i].SetActive(false); }

        if (boughtWeapons[0] && boughtWeapons[1]) { for (int i = 0; i < weaponNames.Count; i++) { weaponNames[i].SetActive(false); } }
        else { weaponNames[weaponID].SetActive(true); }   
       
        weaponStatTexts[0].text = weapon.GetDescription();
        weaponStatTexts[1].text = weapon.GetWeight().ToString();
        weaponStatTexts[2].text = weapon.GetSpeed().ToString();

        if(boughtWeapons[0] && boughtWeapons[1]) 
        { 
            weaponStatTexts[3].text = weapon.GetSpeed().ToString();
            weaponStatTexts[4].text = weapon.GetImpactDamage().ToString();
        }
        else
        {
            weaponStatTexts[3].text = weaponStates.GetWeaponSpeedShop().ToString();
            weaponStatTexts[4].text = weaponStates.GetWeaponImpactDamageShop().ToString();
        }
        
        weaponStatTexts[5].text = weapon.GetBleedDamage().ToString();
        weaponStatTexts[6].text = weapon.GetBleedDuration().ToString();
        weaponStatTexts[7].text = weapon.GetStaggerDuration().ToString();

        cost = weaponsShop[buyWeapons.GetWeaponID()].GetCost();
        costText.text = cost.ToString();
        upgradeText.text = playerCurrency.speedUpgrades.ToString();
    }

    protected override void SwitchModels()
    {
        List<bool> boughtWeapons = weaponStates.GetBoughtWeapons();

        for (int i = 0; i < chooseButtons.Count; i++)
        {
            switch (boughtWeapons[i])
            {
                case true:
                    chooseButtons[i].SetActive(false);
                    break;

                case false:
                    chooseButtons[i].SetActive(true);
                    break;
            }
        }

        for (int i = 0; i < weaponImages.Count; i++)
        {
            if (boughtWeapons[0] && boughtWeapons[1]) { weaponImages[i].SetActive(false); }
            else
            {
                weaponImages[i].SetActive(false);
                weaponImages[buyWeapons.GetWeaponID()].SetActive(true);
            }
        }
    }

    public Text GetStatText() { return weaponStatTexts[0]; }
}