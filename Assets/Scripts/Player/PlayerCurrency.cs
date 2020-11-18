using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Created by Thuyet Pham.
// Edited by Arttu Paldán 29.10.2020: Basically I just merged all the diffrent counting script into a one. 
// Edited by Bao 18.11.20: Remove "animation" and clean up
public class PlayerCurrency : MonoBehaviour
{
    public int karma, gold, speedUpgrades;    
    
    public TextMeshProUGUI moneyText, upgradeText, karmaText;

    public Slider karmaBar;  

    [SerializeField] private SpawnerDataSO spawner;

    public static event Action<int> OnAddKarma = delegate { };
    public static event Action<int> OnAddGold = delegate { };

    void Awake()
    {
        LoadSaveFile();
    }

    void Start()
    {
        SetTexts();
        SetKarmaBar();
    }

    void SetTexts()
    {
        karmaText.SetText(karma.ToString());
        moneyText.SetText(gold.ToString());       
        upgradeText.SetText(speedUpgrades.ToString());
    }

    public void SetKarmaBar()
    {
        karmaBar.maxValue = spawner.MaxKarma;
        karmaBar.value = karma;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        SaveManager.SaveCurrency(this);
        OnAddGold(amount);
        moneyText.SetText(gold.ToString());
    }

    public void AddKarma(int amount)
    {
        karma += amount;   
        SaveManager.SaveCurrency(this);
        OnAddKarma(amount);
        SetKarmaBar();
        karmaText.SetText(karma.ToString());
    }

    public void AddUpgrades(int amount)
    {
        speedUpgrades += amount; 
        SaveManager.SaveCurrency(this);
        upgradeText.SetText(speedUpgrades.ToString());
    }

    public void LoseGold(int amount)
    {
        gold -= amount;
        moneyText.SetText(gold.ToString());
    }

    public void LoseKarma(int amount)
    {
        karma -= amount;
        karmaText.SetText(karma.ToString());
    }


    public void UpdateCurrencies()
    {
        karmaText.SetText(karma.ToString());
        SetTexts();
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
