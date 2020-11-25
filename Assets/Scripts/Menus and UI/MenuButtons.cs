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
        if (Input.GetKeyDown(KeyCode.Escape) && (panels[1].activeSelf || panels[2].activeSelf || panels[3].activeSelf))
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

    public void ToMainPanel()
    {
        panels[0].SetActive(true);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
        panels[3].SetActive(false);
    }

    public void GoTolevel1()
    {
        int slotAvailable = load.IsAvailableSLot();

        if (slotAvailable != -1)
        {
            SaveManager.ShiftSlotPath(slotAvailable);
            SceneManager.LoadScene("NewLV1Test");
        }
        else
        {
            print("There's no more slot to save!");
        }      
    }  

    public void QuitGame()
    {
        Application.Quit();
    }
}
