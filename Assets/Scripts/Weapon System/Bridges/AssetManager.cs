using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 24.9.2020: Script for handling asset set ups shared by multiple scripts, like for example setting up the abstract weapons. 
public class AssetManager : MonoBehaviour
{
    // Scripts
    private WeaponStates weaponStates;
    private SetUpForge setUpForge;
    private SetUpShop setUpShop;
    private BuyWeapons buyWeapons;
    private StatsCalculator calculator;
    private ChooseWeapon chooseWeapon;
    private PlayerAttackTrigger playerAttack;
    private UseUpgrades useUpgrades;

    // Abstract object lists
    private List<AbstractWeapon> weapons = new List<AbstractWeapon>();
    private List<AbstractWeapon> weaponsShop = new List<AbstractWeapon>();
    private List<AbstractUpgrades> upgrades = new List<AbstractUpgrades>();

    // Sprite lists
    [SerializeField] private List<Sprite> upgradeImages;

    // MeshRenrerer lists
    public List<WeaponInUse> weaponModels;

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
        soundManager.Initialize(this);
    }

    // Function for getting all the other scripts needed by this script. 
    void GetNecessaryScripts()
    {
        setUpForge = GetComponent<SetUpForge>();
        setUpShop = GetComponent<SetUpShop>();
        weaponStates = GetComponent<WeaponStates>();
        buyWeapons = GetComponent<BuyWeapons>();
        calculator = GetComponent<StatsCalculator>();
        chooseWeapon = GetComponent<ChooseWeapon>();
        useUpgrades = GetComponent<UseUpgrades>();

        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackTrigger>();
    }

    // Function for setting up images from the editor menu.
    void SetUpSprites()
    {
        upgradeImages.InsertRange(0, new List<Sprite>(Resources.LoadAll<Sprite>("Upgrades")));
    }

    public void SetUpModels() { foreach (WeaponInUse weapon in weaponModels) { weapon.gameObject.SetActive(false); } }

    // Constructs weapons and upgrades and adds them to their own lists. 
    void SetUpWeaponsAndUpgrades()
    {
        weapons.Add(new TestWeapon("Sword", "Does things", 0, 0, 1.5f, 5, 5, 1, 2, 1, 0, new Vector3(0, 3, 4), new Vector3(2, 0.5f, 1), weaponModels[0]));
        weapons.Add(new TestWeapon("Mace", "Does things", 1, 25, 1.5f, 10, 10, 0, 0,  0, 1, new Vector3(0, 3, 4), new Vector3(1, 0.7f, 0.5f), weaponModels[1]));
        weapons.Add(new TestWeapon("Weapon 3", "Does things", 2, 100, 1.5f, 3, 13, 2, 3, 1, 0, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponModels[2]));
        weapons.Add(new TestWeapon("Weapon 4", "Does things", 3, 150, 2.5f, 1, 5, 2, 3, 1, 0, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponModels[3]));

        weaponsShop.Add(new TestWeapon("Mace", "Does things", 1, 25, 1.5f, 10, 10, 0, 0, 0, 1, new Vector3(0, 3, 4), new Vector3(1, 0.7f, 0.5f), weaponModels[1]));
        weaponsShop.Add(new TestWeapon("Weapon 3", "Does things", 2, 100, 1.5f, 3, 13, 2, 3, 1, 0, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponModels[2]));
        weaponsShop.Add(new TestWeapon("Weapon 4", "Does things", 3, 150, 2.5f, 1, 5, 2, 3, 1, 0, new Vector3(0, 3, 4), new Vector3(4, 1, 2), weaponModels[3]));

        upgrades.Add(new TestUpgrade("Speed Upgrade", "Increases the Speed of your attacks", 0, 25, upgradeImages[0]));
    }

    // Basically this sends the weapons and upgrade lists to the scripts that use them. 
    public void SetUpListsForOtherScripts() 
    { 
        if (weaponStates != null) { weaponStates.SetWeaponList(weapons); }
        if (buyWeapons != null) { buyWeapons.SetWeaponList(weaponsShop); }
        if (calculator != null) { calculator.SetWeaponList(weapons, weaponsShop); }
        if (chooseWeapon != null) { chooseWeapon.SetWeaponList(weapons); }
        if (playerAttack != null) { playerAttack.SetWeaponList(weapons); }
        if(setUpForge != null) { setUpForge.SetWeaponList(weapons, weaponsShop); }
        if (setUpShop != null) { setUpShop.SetWeaponList(weapons, weaponsShop); }
        if(useUpgrades != null) { useUpgrades.SetUpgradeList(upgrades); }
    }

    public List<WeaponInUse> GetWeaponModels() { return weaponModels; }
}
