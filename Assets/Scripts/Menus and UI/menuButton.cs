using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButton : MonoBehaviour
{
    public GameObject paneltoOpen, sceneToload;

    // Start is called before the first frame update
    
    public void changePanel()
    {
        Menu menu = transform.parent.transform.parent.GetComponent<Menu>();
        if(menu != null)
        {       
            // menu.switchPanel(paneltoOpen);
        }
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
