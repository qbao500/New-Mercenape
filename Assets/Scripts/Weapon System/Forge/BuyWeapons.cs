using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Created by Arttu Paldán 11.9.2020: This script will handle buying or unlocking component pieces.
public class BuyWeapons : MonoBehaviour
{
    private ChooseWeapon chooseWeapon;
    private Money money;
    private WeaponStates weaponStates;

    private List<AbstractWeapon> weapons;

    private int weaponID;

    private List<Image> weaponImagesHolder = new List<Image>();

    private GameObject buyWeaponScreen;
    private Text weaponName, weaponDescription, weaponCostText;
    private Image weaponImageBuyScreen;

    private bool cantBuy;

    private float counterStart, originalStart;
    public float counterEnd;

    void Awake()
    {
        BuyOperations.SetUpImportantComponents(this, counterStart);
        BuyOperations.GetWeaponHolders(this, weaponImagesHolder);

        buyWeaponScreen.SetActive(false);
    }

    void Start()
    {
        SetWeaponsHolder();
    }

    void Update()
    {
        if (cantBuy)
        {
            CantBuyCounter();
        }

        if (buyWeaponScreen.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWeapon();
        }
    }

    // Function for setting up images in the shop.
    void SetWeaponsHolder()
    {
        List<bool> ownedWeapons = weaponStates.GetOwnedWeapons();

        BuyOperations.SetWeaponsHolder(weapons, ownedWeapons, weaponImagesHolder);
    }

    // Sets the sprites and texts in the buy screen. These things are gotten from the abstract components.
    void SetBuyWeaponScreen()
    {
        BuyOperations.SetBuyScreen(weapons, weaponID, weaponImageBuyScreen, weaponName, weaponDescription, weaponCostText);
    }
    
    // Button function that recognizes, which button has been pressed and based on this gives out the weaponID we need to execute rest of the code. 
    public void WeaponButton()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        BuyOperations.WeaponButtonPress(this, buttonName, weapons);

        OpenWeapon();
    }
    
    // Function for buying the components.
    public void Buy()
    {
        BuyOperations.BuyWeapon(this, weaponStates, money, weapons, weaponImagesHolder, chooseWeapon.GetOwnedWeaponsList(), buyWeaponScreen, weaponID); 
    }

    // Function that announces, that player can't buy component and keeps this message going for couple of frames. 
    void CantBuyCounter()
    {
        weaponDescription.text = "Don't have enough money for this component";

        counterStart += Time.deltaTime;
        if (counterStart >= counterEnd)
        {
            SetCantBuy(false);

            weaponDescription.text = weapons[weaponID].GetDescription();
            SetCounterStart(originalStart);
        }
    }

    // Function to open the buy component screen.
    void OpenWeapon()
    {
        buyWeaponScreen.SetActive(true);

        SetBuyWeaponScreen();
    }

    // Function to close the buy component screen.
    public void CloseWeapon()
    {
        buyWeaponScreen.SetActive(false);

        SetWeaponID(-1);
    }

    // Set functions
    public void SetWeaponList(List<AbstractWeapon> list) { weapons = list; }
    public void SetChooseWeapon(ChooseWeapon thisChoose) { chooseWeapon = thisChoose; }
    public void SetMoney(Money thisMoney) { money = thisMoney; }
    public void SetWeaponStates(WeaponStates weapon) { weaponStates = weapon; }
    public void SetCantBuy(bool cant) { cantBuy = cant; }
    public void SetWeaponID(int id) { weaponID = id; }
    public void SetCounterStart(float start) { counterStart = start; }
    public void SetOriginalCounterStart(float start) { originalStart = start; }
    public void SetWeaponNameText(Text name) { weaponName = name; }
    public void SetWeaponDescText(Text desc) { weaponDescription = desc; }
    public void SetWeaponCostText(Text cost) { weaponCostText = cost; }
    public void SetBuyScreenWeaponImage(Image weapon) { weaponImageBuyScreen = weapon; }
    public void SetWeaponHolders(List<Image> weaponHolder) { weaponImagesHolder = weaponHolder; }
    public void SetBuyScreen(GameObject screen) { buyWeaponScreen = screen;}
}
