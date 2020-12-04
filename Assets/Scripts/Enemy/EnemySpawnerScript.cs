using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Created by Bao: Only script for spawning enemies
public class EnemySpawnerScript : MonoBehaviour
{
    public SpawnerDataSO spawnerData;

    private Material material;

    public enum SpawnState { Spawning, Waiting, Counting }
    public SpawnState state;
    
    private float groupCountdown;        // Count down to next group
    private float searchCountdown = 2f;  // Count down for searching any alive enemy
  
    [SerializeField] private GameObject completeWaveScreen;

    private PlayerCurrency playerCurrency;

    private void Start()
    {
        SetRef();

        LoadSpawner();
        spawnerData.SetupEnemyStats();

        ToCountingState();      

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

    #region State Changing
    private void ToCountingState()
    {
        state = SpawnState.Counting;
        StartCoroutine(NoSpawningEffect());
    }

    private void ToSpawningState()
    {
        state = SpawnState.Spawning;
        StartCoroutine(SpawningEffect());
    }

    private void ToWaitingState()
    {
        state = SpawnState.Waiting;
        StartCoroutine(NoSpawningEffect());
    }

    private IEnumerator SpawningEffect()
    {
        float elapsed = 0f;
        
        while (elapsed < .5f)
        {
            elapsed += Time.deltaTime;
            material.SetFloat("_Scale", Mathf.Lerp(0.5f, 2f, elapsed / .5f));
            material.SetFloat("_Speed", Mathf.Lerp(0.3f, 0.5f, elapsed / .5f));
            material.SetFloat("_Dissolve", Mathf.Lerp(5f, 2.5f, elapsed / .5f));
            yield return null;
        }
    }

    private IEnumerator NoSpawningEffect()
    {
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            material.SetFloat("_Scale", Mathf.Lerp(2f, 0.5f, elapsed / .5f));
            material.SetFloat("_Speed", Mathf.Lerp(0.5f, 0.3f, elapsed / .5f));
            material.SetFloat("_Dissolve", Mathf.Lerp(2.5f, 5f, elapsed / .5f));
            yield return null;
        }
    }
    #endregion

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
        ToCountingState();

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
        ToSpawningState();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < spawnerData.SpawnList.Count; i++)
        {
            if (spawnerData.SpawnList[i] == spawnerData.EnemyInfo[0].name) // Shred
            {
                SpawnEnemy(spawnerData.SpawnList[i], transform.position + (Vector3.left * 5));
                yield return new WaitForSeconds(1f / RandomSpawnRate);
            }
            else // Mower
            {
                SpawnEnemy(spawnerData.SpawnList[i], transform.position + (Vector3.left * 10));
                yield return new WaitForSeconds(7f);
            }
        }

        ToWaitingState(); // Move to Waiting state after finishing spawning

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

    private void SetRef()
    {
        spawnerData.WaveText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        spawnerData.GroupText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        playerCurrency = FindObjectOfType<PlayerCurrency>();
        material = transform.GetChild(1).GetComponent<MeshRenderer>().material;
    }

    #region Buttons
    public void NextWaveButton()
    {
        Time.timeScale = 1;
        completeWaveScreen.SetActive(false);
        
        playerCurrency.SetKarmaBar();
        SaveManager.SaveCurrency(playerCurrency);
    }

    public void ForgeButton()
    {
        FindObjectOfType<Menu>().ToForge();
        completeWaveScreen.SetActive(false);
    }

    public void NextLevelButton()
    {

    }
    #endregion
}
