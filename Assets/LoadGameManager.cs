using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Created by Bao 19.11.20: Load saved slots
public class LoadGameManager : MonoBehaviour
{
    public Button[] slotButtons;
    public Button[] deleteButtons;
    [HideInInspector] public TextMeshProUGUI[] dateTexts;
    [HideInInspector] public TextMeshProUGUI[] waveTexts;

    public GameObject levelsHolder;
    private Button nextArrow;
    private Button backArrow;
    private int levelCounter = 1;

    public Button newLevelButton;

    private void Start()
    {
        SetTextRef();

        this.SetButtonsActive();
        IsAvailableSLot();

        nextArrow = levelsHolder.transform.parent.gameObject.transform.parent.Find("Next Arrow").GetComponent<Button>();
        backArrow = levelsHolder.transform.parent.gameObject.transform.parent.Find("Back Arrow").GetComponent<Button>();
        SetArrowInteractable();
    }

    public void LoadGameChosen(Button loadButton)
    {
        SaveManager.ShiftSlotPath(loadButton.transform.GetSiblingIndex());
        LevelLoader.instace.LoadLevel(2);
    }

    public void DeleteSavedGame(Button deleteButton)
    {
        SaveManager.ShiftSlotPath(deleteButton.transform.GetSiblingIndex());
        SaveManager.DeleteSavedPath();
        this.SetButtonsActive();
        IsAvailableSLot();
    }

    public int IsAvailableSLot()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (!slotButtons[i].interactable)
            {
                newLevelButton.interactable = true;
                newLevelButton.GetComponent<TooltipTrigger>().enabled = false;
                return i;
            }
        }

        newLevelButton.interactable = false;
        newLevelButton.GetComponent<TooltipTrigger>().enabled = true;
        return -1;
    }

    public bool IsEmpty()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (slotButtons[i].interactable)
            {
                return false;
            }
        }

        return true;
    }

    public void NextArrow()
    {
        levelCounter++;
        StartCoroutine(LevelAnim(-350f));
    }

    public void BackArrow()
    {
        levelCounter--;
        StartCoroutine(LevelAnim(350f));
    }

    private void SetArrowInteractable()
    {
        if (levelCounter <= 1) { backArrow.interactable = false; }
        else { backArrow.interactable = true; }

        if (levelCounter >= levelsHolder.transform.childCount) { nextArrow.interactable = false; }
        else { nextArrow.interactable = true; }
    }

    private IEnumerator LevelAnim(float distance)
    {
        backArrow.interactable = false;
        nextArrow.interactable = false;

        float startPoint = levelsHolder.transform.localPosition.x;
        float destination = levelsHolder.transform.localPosition.x + distance;
        float elapsed = 0f;

        while (elapsed < .2f)
        {
            elapsed += Time.deltaTime;
            levelsHolder.transform.localPosition = new Vector2(Mathf.Lerp(startPoint, destination, elapsed / .2f), levelsHolder.transform.localPosition.y);
            yield return null;
        }

        levelsHolder.transform.localPosition = new Vector2(destination, levelsHolder.transform.localPosition.y);
        SetArrowInteractable();
    }

    private void SetTextRef()
    {
        dateTexts = new TextMeshProUGUI[slotButtons.Length];
        waveTexts = new TextMeshProUGUI[slotButtons.Length];

        for (int i = 0; i < slotButtons.Length; i++)
        {
            dateTexts[i] = slotButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            waveTexts[i] = slotButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
    }
}
