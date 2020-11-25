using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by ???
public class EnemyLootDrop : MonoBehaviour
{
    private PlayerCurrency currency;
  
    //Ossi: These are the % chance of Health and Upgrade drops.
    public int healthChance = 40;
    public int upgradeChance = 30;

    //The value of HP and upgrade orbs. I'm thinking that each enemy has a different multiplier on the value of the orbs they spawn upon death. Something that's swirling in my noggin'.
    public int HPValue = 15, UPValue = 1;

    public int karmaValue;
    public int goldValue;

    private void Awake()
    {
        currency = FindObjectOfType<PlayerCurrency>();
    }

    void DropKarma()
    {
        ObjectPooler.Instance.SpawnFromPool("KarmaDrop", transform.position, Quaternion.identity);
        AddKarma();
    }

    void DropGold()
    {
        ObjectPooler.Instance.SpawnFromPool("GoldDrop", transform.position, Quaternion.identity);
        AddMoney();
    }

    void DropUpgrade()
    {
        if (Random.Range(0, 101) <= upgradeChance)
        {
            ObjectPooler.Instance.SpawnFromPool("UpgradeDrop", transform.position, Quaternion.identity);
            AddUpgrades();
        }
    }
    void DropHealth()
    {
        if (Random.Range(0, 101) <= healthChance)
        {
            ObjectPooler.Instance.SpawnFromPool("HealthDrop", transform.position, Quaternion.identity);
        }
    }

    public void GiveLoot()
    {
        DropGold();
        DropKarma();
        DropUpgrade();
        DropHealth();
    }

    void AddKarma()
    {
        if (currency == null) { return; }
        
        currency.AddKarma(karmaValue);
    }

    void AddMoney()
    {
        if (currency == null) { return; }

        currency.AddGold((int)Random.Range(goldValue * 0.9f, goldValue * 1.1f));
    }

    void AddUpgrades()
    {
        if (currency == null) { return; }

        currency.AddUpgrades(UPValue);
    }
}
