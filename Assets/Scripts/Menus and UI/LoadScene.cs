using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Created by Arttu Paldán 25.9.2020: Temprorary system for changing the scene when required. 
public class LoadScene : MonoBehaviour
{
    private PlayerCurrency playerCurrency;
    GameMaster gamemaster;
    private EnemySpawnerScript spawner;

    void Awake()
    {
        playerCurrency = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCurrency>();
        gamemaster = GameObject.FindGameObjectWithTag("Player").GetComponent<GameMaster>();
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawnerScript>();
    }

    public void GoTolevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("NewLV1Test");
    }

    public void GoToForge()
    { //check if player have enough karma
        if (playerCurrency.karma >= gamemaster.lvMaxKarma && spawner.state == EnemySpawnerScript.SpawnState.Counting)
        {
            Time.timeScale = 1;
            SaveManager.SaveCurrency(playerCurrency);
            SceneManager.LoadScene("Forge");
        }
        //place holder
        else 
        { 
            print("cant go"); 
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SaveManager.SaveCurrency(playerCurrency);
        SceneManager.LoadScene("mainMenu");
    }
}
