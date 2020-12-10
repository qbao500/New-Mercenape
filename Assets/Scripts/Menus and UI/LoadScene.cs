using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Created by Arttu Paldán 25.9.2020: Temprorary system for changing the scene when required. 
public class LoadScene : MonoBehaviour
{
    private PlayerCurrency playerCurrency;

    void Awake()
    {
        playerCurrency = GetComponent<PlayerCurrency>();  
    }

    public void GoTolevel1()
    {
        SceneManager.LoadScene("NewLV1Test");
    }


    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SaveManager.SaveCurrency(playerCurrency);
        LevelLoader.instace.LoadLevel(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
