using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Created by Bao 27.11.20: singleton for loading screen
public class LevelLoader: MonoBehaviour
{
    public static LevelLoader instace;

    public GameObject loadingPanel;
    public Image fillImage;

    private void Awake()
    {
        instace = this;
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public IEnumerator LoadAsync(int sceneIndex)
    {
        loadingPanel.SetActive(true);   
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            fillImage.fillAmount = progress;

            yield return null;
        }
    }
}
