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

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private Dictionary<string, GameObject> prefabDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabDictionary = new Dictionary<string, GameObject>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.amount; i++)
            {
                GameObject obj;

                if (pool.prefab == null)
                {
                    obj = new GameObject(pool.tag);
                }
                else
                {
                    obj = Instantiate(pool.prefab);
                }
                
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
                MakeParent(pool, obj);  // For cleaner hierarchy
            }

            poolDictionary.Add(pool.tag, objectQueue);
            prefabDictionary.Add(pool.tag, pool.prefab);
        }
    }

    // Call ObjectPooler.Instance.SpawnFromPool to spawn
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count == 0 || poolDictionary[tag].Peek().activeSelf)
        {
            objectToSpawn = Instantiate(prefabDictionary[tag], position, rotation);
        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
       
        poolDictionary[tag].Enqueue(objectToSpawn);
        
        return objectToSpawn;
    }

    public bool IsAnyActiveObject(List<string> tag)
    {
        for (int i = 0; i < tag.Count; i++)
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
        if (pool.tag == "Shred" || pool.tag == "Mower") { return; }

        go.transform.SetParent(transform, false);
    }

}
