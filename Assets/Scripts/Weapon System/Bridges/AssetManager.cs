using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 24.9.2020: Script for handling asset set ups shared by multiple scripts, like for example setting up the abstract weapons. 
public class AssetManager : MonoBehaviour
{
    // Scripts
    private WeaponStates weaponStates;
    private BuyWeapons buyWeapons;
    private UseUpgrades useUpgrades;
    private StatsCalculator calculator;
    private ChooseWeapon chooseWeapon;
    private PlayerAttackTrigger playerAttack;

    // Abstract object lists
    private List<AbstractWeapon> weapons = new List<AbstractWeapon>();
    private List<AbstractUpgrades> upgrades = new List<AbstractUpgrades>();

    // Sprite lists
    [SerializeField] private List<Sprite> weaponImages, chosenWeaponImages, upgradeImages;

    // MeshRenrerer lists
    [SerializeField] private List<GameObject> weaponModels;

    //Sound effect array
    public SoundAudioClip[] soundAudioclipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public soundManager.Sound sound;
        public AudioClip audioclip;
    }

    void Awake()
    {
        GetNecessaryScripts();
        SetUpSprites();
        SetUpModels();
        SetUpWeaponsAndUpgrades();
        SetUpListsForOtherScripts();
        soundManager.Initialize();
    }

    // Function for getting all the other scripts needed by this script. 
    void GetNecessaryScripts()
    {
        weaponStates = GetComponent<WeaponStates>();
        buyWeapons = GetComponent<BuyWeapons>();
        useUpgrades = GetComponent<UseUpgrades>();
        calculator = GetComponent<StatsCalculator>();
        chooseWeapon = GetComponent<ChooseWeapon>();

        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackTrigger>();
    }

    // Function for setting up images from the editor menu.
    void SetUpSprites()
    {
        weaponImages.InsertRange(0, new List<Sprite>(Resources.LoadAll<Sprite>("Weapons")));
        chosenWeaponImages.InsertRange(0, new List<Sprite>(Resources.LoadAll<Sprite>("ChosenWeapons"))); 
        upgradeImages.InsertRange(0, new List<Sprite>(Resources.LoadAll<Sprite>("Upgrades")));
    }

    public void SetUpModels()
    {
        weaponModels.InsertRange(0, new List<GameObject>(GameObject.FindGameObjectsWithTag("WeaponInUse")));

        foreach (GameObject weapon in weaponModels) { weapon.SetActive(false); }
    }

    // Constructs weapons and upgrades and adds them to their own lists. 
    void SetUpWeaponsAndUpgrades()
    {
        weapons.Add(new TestWeapon("Weapon 1", "Does things", 0, 0, 1.5f, 5, 5, 1, 2, 1, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponImages[0], chosenWeaponImages[0], weaponModels[0]));
        weapons.Add(new TestWeapon("Weapon 2", "Does things", 1, 25, 1.0f, 7, 15, 2, 3,  1, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponImages[1], chosenWeaponImages[1], weaponModels[1]));
        weapons.Add(new TestWeapon("Weapon 3", "Does things", 2, 100, 1.5f, 3, 13, 2, 3, 1, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponImages[2], chosenWeaponImages[2], weaponModels[2]));
        weapons.Add(new TestWeapon("Weapon 4", "Does things", 3, 150, 2.5f, 1, 5, 2, 3, 1, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponImages[3], chosenWeaponImages[3], weaponModels[3]));

        upgrades.Add(new TestUpgrade("Speed Upgrade", "Increases the Speed of your attacks", 0, 25, upgradeImages[0]));
    }

    // Basically this sends the weapons and upgrade lists to the scripts that use them. 
    public void SetUpListsForOtherScripts() 
    { 
        if (weaponStates != null) { weaponStates.SetWeaponList(weapons); }
        if (buyWeapons != null) { buyWeapons.SetWeaponList(weapons); }
        if (useUpgrades != null) { useUpgrades.SetWeaponList(weapons); useUpgrades.SetUpgradeList(upgrades); }
        if (calculator != null) { calculator.SetWeaponList(weapons); }
        if (chooseWeapon != null) { chooseWeapon.SetWeaponList(weapons); }
        if (playerAttack != null) { playerAttack.SetWeaponList(weapons); }
    }
}
