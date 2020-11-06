using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

// Created by Thuyet Pham.
// Edited by Arttu Paldán 29.10.2020: Basically I just merged all the diffrent counting script into a one. 
public class PlayerCurrency : MonoBehaviour
{
    private GameMaster gameMaster;

    public int karma, gold, speedUpgrades;
    private int gainedGoldAmount, gainedKarmaAmount, gainedSpeedAmount;
    private int goldCount, karmaCount, speedCount;
    
    public Text moneyText, upgradeText, karmaText;

    public Slider karmaBar, moneyBar, upgradeBar;
    public Image karmaFill;

    private bool gainedGold, gainedKarma, gainedUpgrade;

    void Awake()
    {
        gameMaster = GetComponent<GameMaster>();

        LoadSaveFile();
    }

    void Start()
    {
        SetTexts();
        SetKarmaBar();
    }

    void LateUpdate()
    {
        moneyText.text = moneyBar.value.ToString();
        karmaText.text = karmaBar.value.ToString();
        upgradeText.text = upgradeBar.value.ToString();

        if (gainedGold) { FillTheCoffers(); }
        if (gainedKarma) { FillKarma(); }
        if (gainedUpgrade) { FillUpgrades();  }
    }

    void SetTexts()
    {
        moneyBar.value = gold;
        moneyText.text = moneyBar.value.ToString();

        upgradeBar.value = speedUpgrades;
        upgradeText.text = upgradeBar.value.ToString();
    }

    public void SetKarmaBar()
    {
        if (gameMaster != null)
        {
            karmaBar.maxValue = gameMaster.lvMaxKarma;
        }
        else
        {
            karmaBar.maxValue = 1000;

        }

        karmaBar.value = karma;
        karmaText.text = karma.ToString();
    }

    public void AddGold(int amount)
    {
        gainedGold = true;
        gainedGoldAmount = amount;
    }

    public void AddKarma(int amount)
    {
        gainedKarma = true;
        gainedKarmaAmount = amount;
    }

    public void AddUpgrades(int amount)
    {
        gainedUpgrade = true;
        gainedSpeedAmount = amount;
    }

    public void LoseGold(int amount)
    {
        gold -= amount;
        moneyText.text = gold.ToString();
    }

    public void LoseKarma(int amount)
    {
        karma -= amount;
        karmaText.text = karma.ToString();
    }

    void FillTheCoffers()
    {
        if(goldCount != gainedGoldAmount)
        {
            gold++;
            goldCount++;
            moneyBar.value++;
        }
        else
        {
            gainedGold = false;
            goldCount = 0;
        }
    }

    void FillKarma()
    {
        if (karmaCount != gainedKarmaAmount)
        {
            karma++;
            karmaCount++;
            karmaBar.value++;
        }
        else
        {
            gainedKarma = false;
            karmaCount = 0;
        }
    }

    void FillUpgrades()
    {
        if (speedCount != gainedSpeedAmount)
        {
            speedUpgrades++;
            speedCount++;
            upgradeBar.value++;
        }
        else
        {
            gainedUpgrade = false;
            speedCount = 0;
        }
    }

    void LoadSaveFile()
    {
        CurrencyData data = SaveManager.LoadCurrency();

        if (data != null)
        {
            karma = data.playerKarma;
            gold = data.playerMoney;
            speedUpgrades = data.speedUpgrades;
        }
    }
}
