using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Spawn Data", menuName = "Spawn Data")]
public class SpawnerDataSO : ScriptableObject
{
    [SerializeField] private List<WaveSO> wavesInfo;
    [SerializeField] private List<EnemyStatsSO> enemyInfo;

    public TextMeshProUGUI WaveText { get; set; }
    public TextMeshProUGUI GroupText { get; set; }

    private AnimationCurve maxKarmaEachWave;
    public int MaxKarma => (int)maxKarmaEachWave.Evaluate(CurrentWave);

    public int CurrentWave { get; private set; } = 1;
    public int CurrentGroup { get; private set; } = 0;
    public int ShredCount { get; private set; } = 0;
    public int MowerCount { get; private set; } = 0;
    public float TimeBetweenGroups { get; private set; } = 3f;
    public List<string> SpawnList { get; set; } = new List<string>();

    public List<WaveSO> WavesInfo { get => wavesInfo; }
    public List<EnemyStatsSO> EnemyInfo { get => enemyInfo; }

    private void OnEnable()
    {
        CurrentGroup = 0;
        SetMaxKarma();
    }

    // Call in the beginning and when wave completed
    public void SetupEnemyStats()
    {
        for (int i = 0; i < enemyInfo.Count; i++)
        {
            enemyInfo[i].SetupStats(CurrentWave);
        }
    }

    // Setup for spawn list and pattern
    public void PrepareGroup()
    {
        CurrentGroup++;
        GroupText.SetText("Group " + CurrentGroup);
        WaveText.SetText("Wave " + CurrentWave);

        MakeSpawnList();
    }

    public void SetWave(int wave) => CurrentWave = wave;

    public void SetGroup(int group) => CurrentGroup = group;

    private void MakeSpawnList()
    {
        GetSpawnPattern();

        SpawnList.Clear();  // Re-use list, so clear it before making new one

        for (int i = 0; i < ShredCount; i++)
        {
            SpawnList.Add(enemyInfo[0].name); // Add Shred
        }

        for (int i = 0; i < MowerCount; i++)
        {
            SpawnList.Add(enemyInfo[1].name); // Add Mower
        }
    }

    private void GetSpawnPattern()
    {
        if (CurrentWave > wavesInfo.Count || CurrentGroup > wavesInfo[CurrentWave - 1].groups.Count)
        {
            // It current wave/group is not in pattern, randomize spawn
            ShredCount = Random.Range(4, 6 + 1);
            MowerCount = RandomMower;
            return;
        }

        // Otherwise get numbers of enemy accordingly
        ShredCount = wavesInfo[CurrentWave - 1].groups[CurrentGroup - 1].shred;
        MowerCount = wavesInfo[CurrentWave - 1].groups[CurrentGroup - 1].mower;
    }

    private void SetMaxKarma()
    {
        maxKarmaEachWave = new AnimationCurve();
        // Shred: 50  Mower: 100
        maxKarmaEachWave.AddKey(1, 350);    // 5S  1M = 350
        maxKarmaEachWave.AddKey(2, 900);    // 7S  2M = 550  (+ 350  =  900)
        maxKarmaEachWave.AddKey(3, 2000);   // 16S 4M = 1200 (+ 900  = 2100)
        maxKarmaEachWave.AddKey(4, 3300);   // 15S 6M = 1350 (+ 2000 = 3450)
        maxKarmaEachWave.AddKey(5, 5000);   // Random = 1550 (+ 3450 = 5000)

        maxKarmaEachWave.AddKey(60, 115000);    // +2000 each wave 
        maxKarmaEachWave.AddKey(100, 215000);   // +2500 each wave
    }

    private int RandomMower => Random.value > 0.85f ? 1 : 2;    // 85% => 1 Mower, otherwise 2

}
