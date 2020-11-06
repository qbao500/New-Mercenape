using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonsPauseMenu : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && panel.activeSelf)
        {
            CloseSettings();
        }
    }

    public void ToSettingsPanel()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }

    void CloseSettings()
    {
        Time.timeScale = 0.95f;
        panel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
