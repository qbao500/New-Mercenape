using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Created by Arttu Paldán 16.9.2020: This class allows the player to choose his weapon from among the ones he has unlocked. 
public class ChooseWeapon : MonoBehaviour
{
    private WeaponStates weaponStates;
    private UseUpgrades useUpgrades;
   
    private List<AbstractWeapon> weapons;

    private int chosenWeaponID, buttonID;

    private string buttonName;

    [SerializeField] private List<Image> ownedWeapons;

    void Awake()
    {
        weaponStates = GetComponent<WeaponStates>();
        useUpgrades = GetComponent<UseUpgrades>();
    }

    void Start()
    {
        GetChodenHolders();
        SetOwnedWeaponImages();
    }

    public void GetChodenHolders()
    {
        GameObject[] holderObjects = GameObject.FindGameObjectsWithTag("ChosenWeaponHolder");
        Image[] holderImage = new Image[holderObjects.Length];
        
        for (int i = 0; i < holderImage.Length; i++) { holderImage[i] = holderObjects[i].GetComponent<Image>(); }

        ownedWeapons.InsertRange(0, holderImage);
    }

    public void SetOwnedWeaponImages()
    {
        List<bool> ownedBools = weaponStates.GetOwnedWeapons();

        chosenWeaponID = weaponStates.GetChosenWeaponID();

        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            switch (ownedBools[i])
            {
                case true:
                    ownedWeapons[i].sprite = weapons[i].GetWeaponImage();
                    break;

                case false:
                    ownedWeapons[i].sprite = null;
                    break;
            }
        }

        ownedWeapons[chosenWeaponID].sprite = weapons[chosenWeaponID].GetChosenWeaponImage();
    }


    // Button function, which detects which button has been pressed and gives us the chosenWeaponID based on that. 
    public void PlayersChoice()
    {
        buttonName = EventSystem.current.currentSelectedGameObject.name;

        List<bool> ownedWeaponsList = weaponStates.GetOwnedWeapons();

        if (buttonName == "ChooseButton1" && ownedWeaponsList[0])
        {
            buttonID = 0;
            chosenWeaponID = weapons[0].GetID();
            SwitchImages();
        }
        else if (buttonName == "ChooseButton2" && ownedWeaponsList[1])
        {
            buttonID = 1;
            chosenWeaponID = weapons[1].GetID();
            SwitchImages();
        }
        else if (buttonName == "ChooseButton3" && ownedWeaponsList[2])
        {
            buttonID = 2;
            chosenWeaponID = weapons[2].GetID();
            SwitchImages();
        }
        else if (buttonName == "ChooseButton4" && ownedWeaponsList[3])
        {
            buttonID = 3;
            chosenWeaponID = weapons[3].GetID();
            SwitchImages();
        }

        weaponStates.SetChosenWeaponID(chosenWeaponID);
        weaponStates.SetUpWeapon();
        SaveManager.SaveWeapons(weaponStates);
        useUpgrades.SetUpUpgradeScreen();
    }

    // Function that simply changes the image of an weapon to indicate, that this one is one the player is equipping. 
    void SwitchImages()
    {
        List<bool> ownedWeaponsList = weaponStates.GetOwnedWeapons();

        for (int i = 0; i < ownedWeapons.Count; i++)
        {
              switch (ownedWeaponsList[i])
              {
                case true:
                    ownedWeapons[i].sprite = weapons[i].GetWeaponImage();
                    break;

                case false:
                    ownedWeapons[i].sprite = null;
                    break;
              }
        }

        ownedWeapons[buttonID].sprite = weapons[chosenWeaponID].GetChosenWeaponImage();
    }

    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }

    public List<Image> GetOwnedWeaponsList() { return ownedWeapons; }
}
