using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGameManager : MonoBehaviour
{
    public Button[] slotButtons;
    public Button[] deleteButtons;

    public GameObject levelsHolder;
    private Button nextArrow;
    private Button backArrow;
    private int levelCounter = 1;

    private void Start()
    {        
        this.SetButtonsActive();

        nextArrow = levelsHolder.transform.parent.gameObject.transform.parent.Find("Next Arrow").GetComponent<Button>();
        backArrow = levelsHolder.transform.parent.gameObject.transform.parent.Find("Back Arrow").GetComponent<Button>();
        SetArrowInteractable();
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

    public void NextArrow()
    {
        levelCounter++;
        StartCoroutine(LevelAnim(-350f));
        SetArrowInteractable();
    }

    public void BackArrow()
    {
        levelCounter--;
        StartCoroutine(LevelAnim(350f));
        SetArrowInteractable();
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
        float startPoint = levelsHolder.transform.localPosition.x;
        float destination = levelsHolder.transform.localPosition.x + distance;
        float elapsed = 0f;

        while (elapsed < .3f)
        {
            elapsed += Time.deltaTime;
            levelsHolder.transform.localPosition = new Vector2(Mathf.Lerp(startPoint, destination, elapsed / .3f), levelsHolder.transform.localPosition.y);
            yield return null;
        }

        levelsHolder.transform.localPosition = new Vector2(destination, levelsHolder.transform.localPosition.y);
    }

}
