using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Bao 1.11.20: Referencing asset through code, following tutorial: https://www.youtube.com/watch?v=EI1KJv8owCg&ab_channel=GameDevHQ
// Access by: GameAssets.instance.variable
public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;
    public static GameAssets instance
    {
        get
        {
            if (_instance == null)
            {                           
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameAssets>();
            }
            return _instance;
        }
    }

    // Instances to reference or create
    public Transform damagePopUp;

    private void Awake()
    {
        _instance = this;
    }
}
