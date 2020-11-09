using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by ???
public class EnemyLootDrop : MonoBehaviour
{
    public GameObject floatMoney;
    public GameObject floatKarma;
    public GameObject floatUpgrade;
    public GameObject healthDrop;
    
    //Ossi: These are the % chance of Health and Upgrade drops.
    public int healthChance = 40;
    public int upgradeChance = 30;

    //The value of HP and upgrade orbs. I'm thinking that each enemy has a different multiplier on the value of the orbs they spawn upon death. Something that's swirling in my noggin'.
    public int HPvalue = 15, UPvalue = 1;

    void InstantiateKarmaDrop()
    {
        ObjectPooler.Instance.SpawnFromPool("KarmaDrop", transform.position, Quaternion.identity);
    }

    void InstantiateGoldDrop()
    {
        ObjectPooler.Instance.SpawnFromPool("GoldDrop", transform.position, Quaternion.identity);
    }

    void InstantiateUpgrade()
    {
        if (Random.Range(0, 101) <= upgradeChance)
        {
            ObjectPooler.Instance.SpawnFromPool("UpgradeDrop", transform.position, Quaternion.identity);
        }
    }
    void InstantiateHealthDrop()
    {
        if(Random.Range(0, 101) <= healthChance)
        {
            ObjectPooler.Instance.SpawnFromPool("HealthDrop", transform.position, Quaternion.identity);
        }
    }

     public void GiveLoot()
     {
        InstantiateGoldDrop();
        InstantiateKarmaDrop();
        InstantiateUpgrade();
        InstantiateHealthDrop();
     }
}
