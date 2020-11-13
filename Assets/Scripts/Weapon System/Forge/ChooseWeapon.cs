using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Created by Arttu Paldán 16.9.2020: This class allows the player to choose his weapon from among the ones he has unlocked. 
public class ChooseWeapon : MonoBehaviour
{
    private SetUpScreens setUp;
    private WeaponStates weaponStates;
   
    private List<AbstractWeapon> weapons;

    private int chosenWeaponID;

    private string buttonName;

    [SerializeField] private List<GameObject> ownedWeapons;

    void Awake()
    {
        SetUpScripts();
    }


    void SetUpScripts()
    {
        setUp = GetComponent<SetUpScreens>();
        weaponStates = GetComponent<WeaponStates>();
    }


    // Button function, which detects which button has been pressed and gives us the chosenWeaponID based on that. 
    public void PlayersChoice()
    {
        buttonName = EventSystem.current.currentSelectedGameObject.name;

        List<bool> ownedWeaponsList = weaponStates.GetOwnedWeapons();

        if (buttonName == "OwnedWeaponSword" && ownedWeaponsList[0])
        {
            chosenWeaponID = weapons[0].GetID();
        }
        else if (buttonName == "OwnedWeaponMace" && ownedWeaponsList[1])
        {
            chosenWeaponID = weapons[1].GetID();
        }
        else if (buttonName == "ChooseButton3" && ownedWeaponsList[2])
        {
            chosenWeaponID = weapons[2].GetID();
        }
        else if (buttonName == "ChooseButton4" && ownedWeaponsList[3])
        {
            chosenWeaponID = weapons[3].GetID();
        }

        weaponStates.SetChosenWeaponID(chosenWeaponID);
        weaponStates.SetUpWeapon();
        SaveManager.SaveWeapons(weaponStates);
        setUp.SetScreen();
    }

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }

    public List<GameObject> GetOwnedWeaponsList() { return ownedWeapons; }
}
