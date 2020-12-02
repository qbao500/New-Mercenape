using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    //Written by Ossi Uusitalo

    //8.10 changes: I'll make this into a general menu panel control system whcih manages the panel placement. I'll add separate scripts for Options and other panels, since not all of them may be on the same scene.
    [SerializeField]
    //These vectors point to where each panel except menuCenter goes to when disabled. Sure, it may not be necessary, but I like the filing cabinet aesthetic.
    // I unwittingly made this so compact, that I added the pause menu on the same script. I will chang the name of the script ASAP since it is doing my head in.

    public SetUpForge setUpForge;
    public SetUpShop setUpShop;
    public BuyWeapons buyWeapons;
    public WeaponStates weaponStates;

    public bool isPaused = false;
    public Canvas mainCanvas;
    public GameObject[] panels;
    public Vector2[] startPos;
    public GameObject currentPanel, pausepanel;

    public GameObject[] cameras;
    // Add a panel you want to be the "pause menu" on the editor

    void Start()
    {
        // mainCanvas = transform.GetComponent<Canvas>();
        startPos = new Vector2[panels.Length];
        for(int i = 0; i < startPos.Length; i++)
        {
            //Set the return positions of each panel when dismissed.
            startPos[i] = panels[i].transform.position;
        }
        //Set the position each panel returns to when not selected.
        panels[0].transform.position = mainCanvas.transform.position;
        currentPanel = panels[0];
    }

    private void Update()
    {
        //Pauses or resumes the game if the pause panel is on/off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }
    }

    //This returns the current panel to its starting position when a new panel is put up. 
    private void returnPanel(GameObject panelReturn)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] == panelReturn)
            {
                panelReturn.transform.position = startPos[i];
                break;
            }
        }
    }

    public void pauseGame()
    {
        //First we check if a pausepanel's been assigned.
        if (pausepanel != null)
        {
            if (!isPaused)
            {
                //Pause the game
                if (currentPanel != null)
                {
                    returnPanel(currentPanel);
                }
                pausepanel.transform.position = mainCanvas.transform.position;
                cameras[0].SetActive(false);
                cameras[1].SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
                currentPanel = pausepanel;
            }
            else
            {
                //Resume the game
                returnPanel(currentPanel);
                isPaused = false;
                Time.timeScale = 1;
                currentPanel = null;
                cameras[0].SetActive(true);
                cameras[1].SetActive(false);
                setUpForge.ResetSpeedUpgrades();
                weaponStates.SetUpWeapon();
            }
        }
    }

    public void switchPanel(GameObject newPanel)
    {
        returnPanel(currentPanel);
        newPanel.transform.position = mainCanvas.transform.position;
        
        currentPanel = newPanel;
    }

    public void ButtonPress()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        if(buttonName == "options")
        {
            switchPanel(panels[2]);
            Debug.Log("Options pressed");
        }
        else if(buttonName == "weapon")
        {
            switchPanel(panels[3]);
            setUpForge.SetScreen(setUpForge.GetWeaponID());
        }
        else if(buttonName == "ToShop")
        {
            switchPanel(panels[4]);
            buyWeapons.CheckID();
            setUpForge.ResetSpeedUpgrades();
            setUpShop.SetScreen(buyWeapons.GetWeaponID());
        }
        else if(buttonName == "ToForge")
        {
            switchPanel(panels[3]);
            setUpForge.SetScreen(setUpForge.GetWeaponID());
        }
    }

    public void backButton()
    {
        
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] == currentPanel.transform.parent.gameObject)
            {
                returnPanel(currentPanel);
                panels[i].transform.position = mainCanvas.transform.position;
                currentPanel = panels[i];

                break;
            }
        }
    }

    public void WipeWeaponMemory()
    {
        SaveManager.DeleteWeapons();
    }

    public void WipeCurrencyMemory()
    {
        SaveManager.DeleteCurrency();
    }
}
