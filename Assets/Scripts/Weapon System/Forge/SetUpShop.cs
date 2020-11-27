using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Created by Arttu Paldán on 13.11.2020: 
public class SetUpShop : SetUpScreens
{
    BuyWeapons buyWeapons;

    [SerializeField] List<GameObject> arrowButtons;

    protected override void Awake() { base.Awake(); buyWeapons = GetComponent<BuyWeapons>(); arrowButtons.InsertRange(0, GameObject.FindGameObjectsWithTag("ArrowButton")); }

    protected override void Start() { base.Start(); }

    protected override void UpdateWeaponStats()
    {
        statsCalculator.SetRequestFromShop(true);
        base.UpdateWeaponStats();
    }

    protected override void UpdateTexts()
    {
        AbstractWeapon weapon = weaponsShop[buyWeapons.GetWeaponID()];
        List<bool> boughtWeapons = weaponStates.GetBoughtWeapons();
        
        if (boughtWeapons[0])
        {
            //for (int i = 0; i < weaponNames.Count; i++) { weaponNames[i].SetActive(false); }

            weaponStatTexts[0].text = "Out of Stock!";
            weaponStatTexts[1].text = "Our apologies, we have run out of new weapons.";
            weaponStatTexts[2].text = "";
            weaponStatTexts[3].text = "";
            weaponStatTexts[4].text = "";
            weaponStatTexts[5].text = "";
            weaponStatTexts[6].text = "";
            weaponStatTexts[7].text = "";

            costText.text = " " + 0;
            upgradeText.text = playerCurrency.speedUpgrades.ToString();

            // for(int i = 0; i < arrowButtons.Count; i++) { arrowButtons[i].SetActive(false); }
        }
        else
        {
            for (int i = 0; i < weaponNames.Count; i++) { weaponNames[i].SetActive(false); }
            weaponNames[weaponID].SetActive(true);

            weaponStatTexts[0].text = weapon.GetDescription();
            weaponStatTexts[1].text = weapon.GetWeight().ToString();
            weaponStatTexts[2].text = weapon.GetSpeed().ToString();
            weaponStatTexts[3].text = weaponStates.GetWeaponSpeedShop().ToString();
            weaponStatTexts[4].text = weaponStates.GetWeaponImpactDamageShop().ToString();
            weaponStatTexts[5].text = weapon.GetBleedDamage().ToString();
            weaponStatTexts[6].text = weapon.GetBleedDuration().ToString();
            weaponStatTexts[7].text = weapon.GetStaggerDuration().ToString();

            cost = weaponsShop[buyWeapons.GetWeaponID()].GetCost();
            costText.text = cost.ToString();
            upgradeText.text = playerCurrency.speedUpgrades.ToString();
        }
    }

    protected override void SwitchModels()
    {
        List<bool> boughtWeapons = weaponStates.GetBoughtWeapons();

        for (int i = 0; i < weaponInventory.Count; i++)
        {
            switch (boughtWeapons[i])
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
            if (boughtWeapons[0] && boughtWeapons[1] && boughtWeapons[2])
            {
                weaponImages[i].SetActive(false);
            }
            else
            {
                weaponImages[i].SetActive(false);
                weaponImages[buyWeapons.GetWeaponID()].SetActive(true);
            }
        }
    }

    public Text GetStatText() { return weaponStatTexts[0]; }
}