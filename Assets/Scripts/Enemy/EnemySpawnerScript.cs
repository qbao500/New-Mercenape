﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Created by Bao: Only script for spawning enemies
public class EnemySpawnerScript : MonoBehaviour
{
    public SpawnerDataSO spawnerData;

    public enum SpawnState { Spawning, Waiting, Counting }
    public SpawnState state;
    
    private float groupCountdown;        // Count down to next group
    private float searchCountdown = 2f;  // Count down for searching any alive enemy
  
    [SerializeField] private GameObject completeWaveScreen;

    private PlayerCurrency playerCurrency;

    private void Start()
    {
        SetTextRef();

        LoadSpawner();
        spawnerData.SetupEnemyStats();

        state = SpawnState.Counting;

        playerCurrency = FindObjectOfType<PlayerCurrency>();

        groupCountdown = spawnerData.TimeBetweenGroups;

        spawnerData.SetGroup(0);
        spawnerData.PrepareGroup();
    }

    private void Update()
    {
        // When player is fighting a group
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive()) // Finish group when player kill all enemy
            {               
                GroupCompleted();                
            }
            else // If there's still enemy alive, wait for player to kill them all
            {               
                return;
            }
        }

        // Spawn group when count down is finished
        if (groupCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {               
                StartCoroutine(SpawnWave());    // Start spawning group
            }
        }
        else
        {            
            groupCountdown -= Time.deltaTime;   // Otherwise count down
        }   
              
    }

    // Check if enemies are still alive
    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 2f;   // Check every 2 seconds
            if (!ObjectPooler.Instance.IsAnyActiveObject(spawnerData.SpawnList))
            {
                return false;
            }
        }

        return true;
    }

    // Group completed and prepare new group
    private void GroupCompleted()
    {
        state = SpawnState.Counting;
        groupCountdown = spawnerData.TimeBetweenGroups;

        CheckWaveEnd();
        
        spawnerData.PrepareGroup();
    }

    // When player get enough karma and finish wave
    private void CheckWaveEnd()
    {
        if (playerCurrency.karma >= spawnerData.MaxKarma)
        {
            Invoke("ShowScreen", 1.5f);
            spawnerData.GroupText.gameObject.SetActive(false);
            spawnerData.WaveText.gameObject.SetActive(false);

            spawnerData.SetWave(spawnerData.CurrentWave + 1);
            SaveManager.SaveSpawner(this);

            spawnerData.SetupEnemyStats();  // Re-setup enemy stats
            spawnerData.SetGroup(0);        // Reset group          
        }
    }

    private void ShowScreen()
    {
        Time.timeScale = 0;
        completeWaveScreen.SetActive(true);
    }

    // Spawn enemies one by one with rate
    private IEnumerator SpawnWave()
    {
        state = SpawnState.Spawning;

        for (int i = 0; i < spawnerData.SpawnList.Count; i++)
        {
            if (spawnerData.SpawnList[i] == spawnerData.EnemyInfo[0].name) // Shred
            {
                SpawnEnemy(spawnerData.SpawnList[i], transform.position + (Vector3.left * 8));
                yield return new WaitForSeconds(1f / RandomSpawnRate);
            }
            else // Mower
            {
                SpawnEnemy(spawnerData.SpawnList[i], transform.position + (Vector3.left * 20));
                yield return new WaitForSeconds(5f);
            }
        }

        state = SpawnState.Waiting; // Move to Waiting state after finishing spawning

        yield break;
    }

    private void SpawnEnemy(string enemy, Vector3 pos)
    {
        ObjectPooler.Instance.SpawnFromPool(enemy, pos, Quaternion.Euler(0, -180, 0));
    }   

    private float RandomSpawnRate => Random.Range(0.2f, 0.5f);

    private void LoadSpawner()
    {
        SpawnerData waveData = SaveManager.LoadSpawner();

        if (waveData == null) 
        {
            spawnerData.SetWave(1); // Set wave = 1 if no saved data          
        }
        else
        {
            spawnerData.SetWave(waveData.currentWave);  // Otherwise, set to saved wave
        }
       
        SaveManager.SaveSpawner(this);  // Then save (useful for fist time)
    }

    private void SetTextRef()
    {
        spawnerData.WaveText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        spawnerData.GroupText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    #region Buttons
    public void NextWaveButton()
    {
        Time.timeScale = 1;
        completeWaveScreen.SetActive(false);
        spawnerData.GroupText.gameObject.SetActive(true);
        spawnerData.WaveText.gameObject.SetActive(true);

        playerCurrency.SetKarmaBar();
        SaveManager.SaveCurrency(playerCurrency);
    }

    public void ForgeButton()
    {
        Time.timeScale = 1;
        SaveManager.SaveCurrency(playerCurrency);
        SceneManager.LoadScene("Forge");
    }

    public void NextLevelButton()
    {

    }
    #endregion
}
