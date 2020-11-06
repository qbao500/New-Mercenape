using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public GameObject[] panels;

    void Start()
    {
        panels[1].SetActive(false);
        panels[2].SetActive(false);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && panels[1].activeSelf || panels[2].activeSelf)
        {
            ToMainPanel();
        }
    }

    public void ToLevelPanel()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }

    public void ToSettingsPanel()
    {
        panels[0].SetActive(false);
        panels[2].SetActive(true);
    }

    public void ToMainPanel()
    {
        panels[0].SetActive(true);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
