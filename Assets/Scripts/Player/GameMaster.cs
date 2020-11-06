using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Created by Thuyet Pham.
public class GameMaster : MonoBehaviour
{
    public int lvMaxKarma;
    PlayerCurrency playerCurrency;

    // Start is called before the first frame update
    void Awake()
    {
        CheckScene4MaxKarma();
    }

    // Update is called once per frame

    private void Update()
    {
        SwitchLV();
    }

    void CheckScene4MaxKarma()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        playerCurrency = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerCurrency>();
        string sceneName = currentScene.name;

        int buildIndex = currentScene.buildIndex;

        if (buildIndex > 0)
        {
            lvMaxKarma = buildIndex * 1000;
        }
        else
        {
            lvMaxKarma = 500;
        }
    }

    void SwitchLV()
    {
        if (playerCurrency.karma >= lvMaxKarma)
        {
           
            if (Input.GetKeyDown(KeyCode.H))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        }
    }
}
