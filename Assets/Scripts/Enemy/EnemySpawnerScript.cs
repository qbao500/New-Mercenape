using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Created by Bao: Only script for spawning enemies
public class EnemySpawnerScript : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting }

    [System.Serializable]
    public class Group
    {
        public int shredCount;
        public int mowerCount;
        public int enemyIncreasedHP;
        public int enemyIncreasedDamage;
    }

    public Group group = new Group();
    //public List<Group> groups = new List<Group>();

    private int currentGroup = 0;
    public int currentWave = 1;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI groupText;

    public float timeBetweenGroups = 3f;

    private float groupCountdown;        // Count down to next group
    private float searchCountdown = 2f;  // Count down for searching any alive enemy

    public string[] enemies = new string[2];
    private List<string> spawnList = new List<string>();

    public SpawnState state;

    [SerializeField] private GameObject completeWaveScreen;

    private PlayerCurrency playerCurrency;
    private GameMaster gameMaster;

    private void Start()
    {
        LoadSpawner();

        state = SpawnState.Counting;

        playerCurrency = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerCurrency>();
        gameMaster = GameObject.FindGameObjectWithTag("gamemaster").GetComponent<GameMaster>();

        groupCountdown = timeBetweenGroups;

        IncreaseDifficulty();
    }

    private void Update()
    {
        // When player is fighting a group
        if (state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
            {
                // Finish group when player kill all enemy
                GroupCompleted();                
            }
            else
            {
                // If there's still enemy alive, wait for player to kill them all
                return;
            }
        }

        // Spawn group when count down is finished
        if (groupCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {
                // Start spawning group
                StartCoroutine(SpawnWave());
            }
        }
        else
        {
            // Otherwise count down
            groupCountdown -= Time.deltaTime;
        }   
              
    }

    // Group completed and prepare new group
    void GroupCompleted()
    {
        // Prepare a new group
        state = SpawnState.Counting;
        groupCountdown = timeBetweenGroups;

        CheckWaveEnd();
        
        IncreaseDifficulty();
    }

    // When player get enough karma
    void CheckWaveEnd()
    {
        if (playerCurrency.karma >= gameMaster.lvMaxKarma)
        {
            Time.timeScale = 0;
            completeWaveScreen.SetActive(true);

            currentWave++;
            SaveManager.SaveSpawner(this);

            currentGroup = 0;   // Reset group
            groupCountdown = timeBetweenGroups * 2; // Wait a bit longer than normal          
        }
    }    

    // Check if enemies are still alive
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 2f;   // Check every 2 seconds
            if (!ObjectPooler.Instance.IsAnyActiveObject(enemies))
            {
                return false;
            }
        }
              
        return true;
    }

    // Spawn enemies one by one with rate
    IEnumerator SpawnWave()
    {
        state = SpawnState.Spawning;

        // Spawn
        for (int i = 0; i < spawnList.Count; i++)
        {                    
            if (spawnList[i] == enemies[0]) // Shred
            {
                SpawnEnemy(spawnList[i], transform.position + (Vector3.left * 5));
                yield return new WaitForSeconds(1f / RandomSpawnRate());
            }
            else // Mower
            {
                SpawnEnemy(spawnList[i], transform.position + (Vector3.left * 20));
                yield return new WaitForSeconds(5f);
            }
        }

        state = SpawnState.Waiting;

        yield break;
    }

    private void SpawnEnemy(string enemy, Vector3 pos)
    {
        ObjectPooler.Instance.SpawnFromPool(enemy, pos, Quaternion.Euler(0, -180, 0));
    }

    // Next group more difficult, spawn pattern
    void IncreaseDifficulty()
    {
        currentGroup++;
        groupText.SetText("Group " + currentGroup);
        waveText.SetText("Wave " + currentWave);

        if (currentGroup == 1) { group.shredCount = 2; group.mowerCount = 0; }

        if (currentGroup == 2) { group.shredCount = 3; group.mowerCount = 0; }

        if (currentGroup == 3) { group.shredCount = 4; group.mowerCount = 1; }
        
        if (currentGroup >= 4)
        {
            group.shredCount = Random.Range(4, 6 + 1);
            group.mowerCount = RandomMower();
        }

        MakeSpawnList();

        group.enemyIncreasedHP += 2;
        group.enemyIncreasedDamage += 1;
    }

    private void MakeSpawnList()
    {
        spawnList.Clear();

        for (int i = 0; i < group.shredCount; i ++)
        {
            // Add Shred
            spawnList.Add(enemies[0]);
        }

        for (int i = 0; i < group.mowerCount; i++)
        {
            // Add Mower
            spawnList.Add(enemies[1]);
        }
    }

    private int RandomMower()
    {
        if (Random.value > 0.85f)   // 85%
        {
            return 1;
        }
        else                   // else 15%
        {
            return 2;
        }
    }

    private float RandomSpawnRate() => Random.Range(0.2f, 0.5f);

    private void LoadSpawner()
    {
        SpawnerData spawnerData = SaveManager.LoadSpawner();

        currentWave = spawnerData.currentWave;
    }


    public void NextWaveButton()
    {
        Time.timeScale = 1;
        completeWaveScreen.SetActive(false);
        groupText.SetText("Group " + currentGroup);
        waveText.SetText("Wave " + currentWave);

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
}
