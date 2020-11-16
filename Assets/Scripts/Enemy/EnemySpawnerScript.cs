using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Created by Bao: Only script for spawning enemies
public class EnemySpawnerScript : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting }

    [SerializeField] private List<WaveSO> wavesInfo;
    [SerializeField] private List<EnemyStatsSO> enemyInfo;

    private int shredCount = 0;
    private int mowerCount = 0;
        
    private int currentGroup = 0;
    [HideInInspector] public int currentWave = 1;
    
    [SerializeField] private float timeBetweenGroups = 3f;

    private float groupCountdown;        // Count down to next group
    private float searchCountdown = 2f;  // Count down for searching any alive enemy

    private List<string> spawnList = new List<string>();

    public SpawnState state;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI groupText;
    [SerializeField] private GameObject completeWaveScreen;

    private PlayerCurrency playerCurrency;
    private GameMaster gameMaster;

    private void Start()
    {
        LoadSpawner();
        SetupEnemyStats();

        state = SpawnState.Counting;

        playerCurrency = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerCurrency>();
        gameMaster = GameObject.FindGameObjectWithTag("gamemaster").GetComponent<GameMaster>();

        groupCountdown = timeBetweenGroups;

        Setup();
    }

    private void SetupEnemyStats()
    {
        for (int i = 0; i < enemyInfo.Count; i++)
        {
            enemyInfo[i].SetupStats(currentWave);
        }
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
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 2f;   // Check every 2 seconds
            if (!ObjectPooler.Instance.IsAnyActiveObject(spawnList))
            {
                return false;
            }
        }

        return true;
    }

    // Group completed and prepare new group
    void GroupCompleted()
    {
        state = SpawnState.Counting;
        groupCountdown = timeBetweenGroups;

        CheckWaveEnd();
        
        Setup();
    }

    // When player get enough karma and finish wave
    void CheckWaveEnd()
    {
        if (playerCurrency.karma >= gameMaster.lvMaxKarma)
        {
            Time.timeScale = 0;
            completeWaveScreen.SetActive(true);

            currentWave++;
            SaveManager.SaveSpawner(this);

            SetupEnemyStats();  // Re-setup enemy stats
            currentGroup = 0;   // Reset group          
        }
    }    

    // Check for spawn pattern and set
    private void Setup()
    {
        currentGroup++;
        groupText.SetText("Group " + currentGroup);
        waveText.SetText("Wave " + currentWave);

        GetSpawnPattern();

        MakeSpawnList();     
    }

    private void GetSpawnPattern()
    {
        if (currentWave > wavesInfo.Count || currentGroup > wavesInfo[currentWave - 1].groups.Count)
        {
            // It current wave/group is not in pattern, randomize spawn
            shredCount = Random.Range(4, 6 + 1);
            mowerCount = RandomMower;
            return;
        }

        // Otherwise get numbers of enemy accordingly
        shredCount = wavesInfo[currentWave - 1].groups[currentGroup - 1].shred;
        mowerCount = wavesInfo[currentWave - 1].groups[currentGroup - 1].mower;
    }

    private void MakeSpawnList()
    {
        spawnList.Clear();  // Re-use list, so clear it before making new one

        for (int i = 0; i < shredCount; i ++)
        {           
            spawnList.Add(enemyInfo[0].name); // Add Shred
        }

        for (int i = 0; i < mowerCount; i++)
        {           
            spawnList.Add(enemyInfo[1].name); // Add Mower
        }
    }

    // Spawn enemies one by one with rate
    IEnumerator SpawnWave()
    {
        state = SpawnState.Spawning;

        for (int i = 0; i < spawnList.Count; i++)
        {
            if (spawnList[i] == enemyInfo[0].name) // Shred
            {
                SpawnEnemy(spawnList[i], transform.position + (Vector3.left * 8));
                yield return new WaitForSeconds(1f / RandomSpawnRate);
            }
            else // Mower
            {
                SpawnEnemy(spawnList[i], transform.position + (Vector3.left * 20));
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

    private int RandomMower => Random.value > 0.85f ? 1 : 2;    // 85% => 1 Mower, otherwise 2

    private float RandomSpawnRate => Random.Range(0.2f, 0.5f);

    private void LoadSpawner()
    {
        SpawnerData spawnerData = SaveManager.LoadSpawner();

        if (spawnerData == null) { return; }

        currentWave = spawnerData.currentWave;
    }

    #region Buttons
    public void NextWaveButton()
    {
        Time.timeScale = 1;
        completeWaveScreen.SetActive(false);
        
        playerCurrency.karma = 0;
        playerCurrency.SetKarmaBar();
        SaveManager.SaveCurrency(playerCurrency);
    }

    public void ForgeButton()
    {
        Time.timeScale = 1;
        SaveManager.SaveCurrency(playerCurrency);
        playerCurrency.karma = 0;
        SceneManager.LoadScene("Forge");
    }

    public void NextLevelButton()
    {

    }
    #endregion
}
