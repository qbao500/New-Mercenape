using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 16.9.2020: An abstract class for creating weapon upgrades. 
public abstract class AbstractUpgrades
{
    // Upgrade details
    protected string upgradeName, upgradeDescription;
   
    // Upgrade image
    protected Sprite upgradeImage;

    // Upgrade id and cost
    protected int upgradeID, upgradeCost;

    // Fetch functions for the variables.
    public string GetName() { return upgradeName;}

    public string GetDescription() { return upgradeDescription; }

    public Sprite GetUpgradeImage() { return upgradeImage;}

    public int GetID() { return upgradeID;}

    public int GetUpgradeCost() { return upgradeCost; }
}

public class TestUpgrade : AbstractUpgrades
{
    //Constructor for this particular upgradde. 
    public TestUpgrade(string name, string description, int id, int cost, Sprite upgrade)
    {
        upgradeName = name;
        upgradeDescription = description;
        upgradeID = id;
        upgradeCost = cost;
        upgradeImage = upgrade;
    }
}
