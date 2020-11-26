using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Created by Arttu Paldán 17.9.2020: Static class that handles the saving of weapon related data. 
// Edited by Bao 19.11.20: Add functions for slot-save system
public static class SaveManager
{
    private static int slotIndex;
    private static string slotPath = "Slot";
    private static DirectoryInfo dirInf = new DirectoryInfo(Path.Combine(Application.persistentDataPath, slotPath + slotIndex.ToString()));

    // Change to chosen slot number. This must be called first before doing anything
    public static void ShiftSlotPath(int slot)
    {
        slotIndex = slot;
        dirInf = new DirectoryInfo(Path.Combine(Application.persistentDataPath, slotPath + slotIndex.ToString()));

        if (!dirInf.Exists) { dirInf.Create(); }    // First time will create slot folder
    }

    public static void DeleteSavedPath() => Directory.Delete(dirInf.FullName, true);

    // Function for saving the data we want to save. It uses the binary formatter to save data. 
    public static void SaveWeapons(WeaponStates weaponStates)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string weaponsPath = Path.Combine(dirInf.FullName, "Weapons.data");
        FileStream stream = new FileStream(weaponsPath, FileMode.Create);
        
        WeaponsData weaponsData = new WeaponsData(weaponStates);

        formatter.Serialize(stream, weaponsData);
        stream.Close();

        SetModifiedDate(weaponsPath);
    }

    // Load function.
    public static WeaponsData LoadWeapons()
    {
        string weaponsPath = Path.Combine(dirInf.FullName, "Weapons.data");
        if (File.Exists(weaponsPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(weaponsPath, FileMode.Open);

            WeaponsData weaponsData = formatter.Deserialize(stream) as WeaponsData;
            stream.Close();

            return weaponsData;
        }
        else
        {
            return null;
        }
    }

    // Delete data function. 
    public static void DeleteWeapons()
    {
        string weaponsPath = Path.Combine(dirInf.FullName, "Weapons.data");
        File.Delete(weaponsPath);
    }

    // Function for saving the data we want to save. It uses the binary formatter to save data. 
    public static void SaveCurrency(PlayerCurrency playerCurrency)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string currencyPath = Path.Combine(dirInf.FullName, "Currency.data");
        FileStream stream = new FileStream(currencyPath, FileMode.Create);

        CurrencyData currencyData = new CurrencyData(playerCurrency);

        formatter.Serialize(stream, currencyData);
        stream.Close();

        SetModifiedDate(currencyPath);
    }

    // Load function.
    public static CurrencyData LoadCurrency()
    {
        string currencyPath = Path.Combine(dirInf.FullName, "Currency.data");
        if (File.Exists(currencyPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(currencyPath, FileMode.Open);

            CurrencyData currencyData = formatter.Deserialize(stream) as CurrencyData;
            stream.Close();

            return currencyData;
        }
        else
        {
            return null;
        }
    }

    // Delete data function. 
    public static void DeleteCurrency()
    {
        string currencyPath = Path.Combine(dirInf.FullName, "Currency.data");
        File.Delete(currencyPath);
    }

    // Function for saving the data we want to save. It uses the binary formatter to save data. 
    public static void SaveSpawner(EnemySpawnerScript enemySpawner)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string spawnerPath = Path.Combine(dirInf.FullName, "Spawner.data");
        FileStream stream = new FileStream(spawnerPath, FileMode.Create);

        SpawnerData spawnerData = new SpawnerData(enemySpawner);

        formatter.Serialize(stream, spawnerData);
        stream.Close();

        SetModifiedDate(spawnerPath);
    }

    // Load function.
    public static SpawnerData LoadSpawner()
    {
        string spawnerPath = Path.Combine(dirInf.FullName, "Spawner.data");
        if (File.Exists(spawnerPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(spawnerPath, FileMode.Open);

            SpawnerData spawnerData = formatter.Deserialize(stream) as SpawnerData;
            stream.Close();

            return spawnerData;
        }
        else
        {
            return null;
        }
    }

    private static void SetModifiedDate(string path)
    {
        Directory.SetLastWriteTime(dirInf.FullName, File.GetLastWriteTime(path));
    }

    // Extension methods
    public static void SetButtonsActive(this LoadGameManager loadGameManager)
    {     
        for (int i = 0; i < loadGameManager.slotButtons.Length; i++)
        {
            dirInf = new DirectoryInfo(Path.Combine(Application.persistentDataPath, slotPath + i.ToString()));
            
            if (dirInf.Exists)
            {
                loadGameManager.slotButtons[i].interactable = true;
                loadGameManager.deleteButtons[i].gameObject.SetActive(true);
                loadGameManager.dateTexts[i].gameObject.SetActive(true);
                loadGameManager.waveTexts[i].gameObject.SetActive(true);
                loadGameManager.dateTexts[i].SetText(dirInf.LastWriteTime.ToString("dd.MM.yy"));
                loadGameManager.waveTexts[i].SetText(LoadSpawner()?.currentWave.ToString());              
            }            
            else
            {
                loadGameManager.slotButtons[i].interactable = false;
                loadGameManager.deleteButtons[i].gameObject.SetActive(false);
                loadGameManager.dateTexts[i].gameObject.SetActive(false);
                loadGameManager.waveTexts[i].gameObject.SetActive(false);
            }
        }
    }

}

#region Save Data Classes
// Save data class, which contains the infor we want to save. 
[System.Serializable]
public class WeaponsData
{
    public int weaponID;

    public List<bool> ownedWeaponsList, bougthWeaponList, upgradedWeaponsList;
    public List<int> savedSpeedAmountsList;

    public WeaponsData(WeaponStates weaponStates)
    {
        weaponID = weaponStates.GetChosenWeaponID();

        ownedWeaponsList = weaponStates.GetOwnedWeapons();
        bougthWeaponList = weaponStates.GetBoughtWeapons();
        upgradedWeaponsList = weaponStates.GetUpgradedWeapons();
        savedSpeedAmountsList = weaponStates.GetSavedSpeeds();
    }
}

[System.Serializable]
public class CurrencyData
{
    public int playerMoney, playerKarma, speedUpgrades;

    public CurrencyData(PlayerCurrency playerCurrency)
    {
        playerMoney = playerCurrency.gold;
        playerKarma = playerCurrency.karma;
        speedUpgrades = playerCurrency.speedUpgrades;
    }
}

[System.Serializable]
public class SpawnerData
{
    public int currentWave;

    public SpawnerData(EnemySpawnerScript enemySpawner)
    {
        currentWave = enemySpawner.spawnerData.CurrentWave;
    }
}
#endregion