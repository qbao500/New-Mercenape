using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Bao 2.11.20: Object pooling, based on Brackeys: https://www.youtube.com/watch?v=tdSmKaJvCoA&ab_channel=Brackeys
// Go to this prefab and add an item to spawn
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int amount;
    }

    #region Singleton

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.amount; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                MakeParent(pool, obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Call ObjectPooler.Instance.SpawnFromPool to spawn
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);
        
        return objectToSpawn;
    }

    public bool IsAnyActiveObject(string[] tag)
    {
        for (int i = 0; i < tag.Length; i++)
        {
            foreach (var obj in poolDictionary[tag[i]])
            {
                // Return true if there is an active object
                if (obj.activeSelf) { return true; }
            }
        }
        
        return false; // If nothing return true, just return false
    }

    private void MakeParent(Pool pool, GameObject go)
    {        
        if (pool.tag != "Shred" && pool.tag != "Mower")
        {
            go.transform.SetParent(transform, false);
        }
    }
}
