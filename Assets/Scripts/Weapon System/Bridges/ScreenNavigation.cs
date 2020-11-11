using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 1.10.2020: This script used to navigate the Forge scene. Aka these button methods set screens active and false. 
public class ScreenNavigation : MonoBehaviour
{
    public GameObject[] panels;

    private GameObject shop, forge, forgeTitle;

    void Awake()
    {
        shop = GameObject.FindGameObjectWithTag("Shop");
        forge = GameObject.FindGameObjectWithTag("Forge");
        forgeTitle = GameObject.FindGameObjectWithTag("ForgeTitle");
    }

    void Start()
    {
        shop.SetActive(false);
        forge.SetActive(false);
        forgeTitle.SetActive(false);
        panels[0].SetActive(false);
        panels[1].SetActive(false);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }

        //if (Input.GetKeyDown(KeyCode.Escape) && panels[0].activeSelf)
        //{
        //    CloseSettings();
        //}
    }

    public void OpenShop()
    {
        forge.SetActive(false);
        shop.SetActive(true);
        forgeTitle.SetActive(true);
    }

    public void OpenForge()
    {
        panels[0].SetActive(false);
        shop.SetActive(false);
        forge.SetActive(true);
        forgeTitle.SetActive(true);
    }

    public void BackToGame()
    {
        forgeTitle.SetActive(false);
        forge.SetActive(false);
        panels[0].SetActive(false);

        Time.timeScale = 0.95f;
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        panels[0].SetActive(true);
    }

    public void ToSettingsPanel()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }

    void CloseSettings()
    {
        Time.timeScale = 0.95f;
        panels[1].SetActive(false);
        panels[0].SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
