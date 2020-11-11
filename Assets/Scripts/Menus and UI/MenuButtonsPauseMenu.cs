using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonsPauseMenu : MonoBehaviour
{
    public GameObject[] panels;

    void Start()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(false);
    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }

        //if (Input.GetKeyDown(KeyCode.Escape) && panels[0].activeSelf)
        //{
        //    CloseSettings();
        //}
    }

    public void OpenPauseMenu()
    {
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
