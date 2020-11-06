using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using System.Security.Cryptography.X509Certificates;

// Arttu paldán edited 25.9.2020: Added functions for wiping the weapons and currency data for testing purposes. 
public class mainMenu : MonoBehaviour
{
    [SerializeField]  
    //These vectors point to where each panel except menuCenter goes to when disabled. Sure, it may not be necessary, but I like the filing cabinet aesthetic.
    // I unwittingly made this so compact, that I added the pause menu on the same script. I will chang the name of the script ASAP since it is doing my head in.
    

    public bool isPaused = false;
    public Canvas mainCanvas;
    public GameObject[] panels, inputButtons;
    public Vector2[] startPos;
    public GameObject currentPanel;
    public GameObject pausePanel;

    void Start()
    {

        mainCanvas = transform.GetComponent<Canvas>();
        startPos = new Vector2[panels.Length];  
        //Set the position each panel returns to when not selected.
        for(int i = 0; i < panels.Length; i++)
        {
            if (panels[i].name.Contains("pause"))
            {
                pausePanel = panels[i];              
            }
            startPos[i] = panels[i].transform.position;         
        }
        panels[0].transform.position = mainCanvas.transform.position;
        currentPanel = panels[0];
        for(int i = 0; i < inputButtons.Length; i++)
        {

        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause pressed");
            pauseGame();
        }
    }

    

    //This returns the current panel to its starting position when a new panel is put up. 
    private void returnPanel(GameObject panelReturn)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if(panels[i] == panelReturn)
            {
                panelReturn.transform.position = startPos[i];
                break;
            }

        }
    }

    public void pauseGame()
    {
        //First we check if a pausepanel's been assigned.
        if (pausePanel != null)
        {
            if (!isPaused)
            {
                //Pause the game
                if (currentPanel != null)
                {
                    returnPanel(currentPanel);
                }
                pausePanel.transform.position = mainCanvas.transform.position;
                isPaused = true;
                Time.timeScale = 0;
                currentPanel = pausePanel;
            }
            else
            {
                //Resume the game
                returnPanel(currentPanel);
                isPaused = false;
                Time.timeScale = 1;
                currentPanel = null;
            }
        }

    }

    public void switchPanel(GameObject newPanel)
    {
        Debug.Log("Changing panel");
        returnPanel(currentPanel);

        if (newPanel.name.Contains("options") || newPanel.name.Contains("level"))
        {
            GameObject backBtn = newPanel.transform.Find("back").gameObject;
            menuButton btnScript;
            if(backBtn != null)
            {
                Debug.Log("Back button found.");
                btnScript = backBtn.GetComponent<menuButton>();
                if(btnScript != null)
                {
                    Debug.Log("Back script found");
                    if (isPaused)
                    {
                        //Back button will now open the Pause Panel (panels[1])
                        btnScript.paneltoOpen = panels[1];
                    } else
                    {
                        //Back button will open Main panel, panels[0].
                        btnScript.paneltoOpen = panels[0];
                    }
                }
            }      
        }
        newPanel.transform.position = mainCanvas.transform.position;
        currentPanel = newPanel;
        if(currentPanel == panels[0])
        {
            isPaused = false;
            Time.timeScale = 1;
        }
    }
    
   

    public void backButton()
    {
        returnPanel(currentPanel);
        for(int i = 0; i < panels.Length; i++)
        {
            if(panels[i] == currentPanel.transform.parent.gameObject)
            {
                Debug.Log("Panel parent found: " + panels[i]);
                panels[i].transform.position = mainCanvas.transform.position;            
                currentPanel = panels[i];
                break;
            }
        }
    }

        
    public void startGame()
    {
        Debug.Log("Starting game");
        SceneManager.LoadScene("Bao_Enemy");
        //Load game Scene
    }

    public void quitGame()
    {
        Debug.Log("Closing game...");
        Application.Quit();
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
