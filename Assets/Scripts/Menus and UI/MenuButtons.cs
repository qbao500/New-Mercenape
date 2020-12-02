using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject[] panels;

    private LoadGameManager load;

    void Start()
    {
        load = GetComponent<LoadGameManager>();       

        ToMainPanel();       
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CheckActivePanels())
        {
            ToMainPanel();
        }
    }

    public void ToLevelPanel()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }

    public void ToLoadPanel()
    {
        panels[0].SetActive(false);
        panels[2].SetActive(true);
    }

    public void ToSettingsPanel()
    {
        panels[0].SetActive(false);
        panels[3].SetActive(true);
    }

    public void ToCreditsPanel()
    {
        panels[0].SetActive(false);
        panels[4].SetActive(true);
    }

    public void ToMainPanel()
    {
        panels[0].SetActive(true);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
        panels[3].SetActive(false);
        panels[4].SetActive(false);
    }

    public void GoTolevel1()
    {
        if (load.IsAvailableSLot() != -1)
        {
            SaveManager.ShiftSlotPath(load.IsAvailableSLot());

            if (load.IsEmpty())
            {
                LevelLoader.instace.LoadLevel(1);
            }
            else
            {
                LevelLoader.instace.LoadLevel(2);
            }
        }           
    }  

    public void QuitGame()
    {
        Application.Quit();
    }

    private bool CheckActivePanels()
    {
        return panels[1].activeSelf || panels[2].activeSelf || panels[3].activeSelf || panels[4].activeSelf;
    }
}
