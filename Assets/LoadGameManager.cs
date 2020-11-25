using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGameManager : MonoBehaviour
{
    public Button[] slotButtons;
    public Button[] deleteButtons;

    private void Start()
    {        
        this.SetButtonsActive();
    }

    public void LoadGameChosen(Button loadButton)
    {
        SaveManager.ShiftSlotPath(loadButton.transform.GetSiblingIndex());
        SceneManager.LoadScene("NewLV1Test");
    }

    public void DeleteSavedGame(Button deleteButton)
    {
        SaveManager.ShiftSlotPath(deleteButton.transform.GetSiblingIndex());
        SaveManager.DeleteSavedPath();
        this.SetButtonsActive();
    }

    public int IsAvailableSLot()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (!slotButtons[i].interactable)
            {
                return i;
            }
        }
        return -1;
    }
  
}
