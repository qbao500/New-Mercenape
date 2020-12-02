using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButton : MonoBehaviour
{
    public GameObject paneltoOpen, sceneToload;
    Menu menu;
    // Start is called before the first frame update

    private void Start()
    {
        menu = GameObject.Find("GameManager").GetComponent<Menu>();
        if (menu != null)
        {
            Debug.Log("Menu script found");
        }
    }

    public void changePanel()
    {
        menu.switchPanel(paneltoOpen);
    }


    public void changeScene()
    {
        if(sceneToload != null)
        {
            SceneManager.LoadScene(sceneToload.name);
        } 
    }

    public void closeGame()
    {
        Application.Quit();
    }
}
