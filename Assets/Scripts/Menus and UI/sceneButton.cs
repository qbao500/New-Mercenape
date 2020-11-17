using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneButton : MonoBehaviour
{
    [SerializeField]
    private Object Scenetoload;
    private PlayerCurrency playerCurrency;

    private EnemySpawnerScript spawner;

    private void Awake()
    {
        playerCurrency = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCurrency>();
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawnerScript>();
    }
    public void changeScene()
    {
        //check if player have enough karma
        if (playerCurrency.karma >= spawner.GetMaxKarma() && spawner.state == EnemySpawnerScript.SpawnState.Counting)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(Scenetoload.name);
        }

        //place holder
        else
        {
            print("cant go to next scene");
        }
    }
}
