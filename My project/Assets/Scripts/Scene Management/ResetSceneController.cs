using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneController : MonoBehaviour
{
    public ResetScript[] allScriptsNeedingResetOnLoad;

    string currentScene;

    private void Start()
    {
        allScriptsNeedingResetOnLoad = GetComponents<ResetScript>();

        currentScene = SceneManager.GetActiveScene().name;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetAllScripts();
        Time.timeScale = 1;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != currentScene)
        {
            Debug.Log("New scene!");
            currentScene = SceneManager.GetActiveScene().name;
            ResetAllScripts();
        }
    }

    public void ResetAllScripts()
    {
        if(allScriptsNeedingResetOnLoad != null)
        {
            foreach (ResetScript behaviour in allScriptsNeedingResetOnLoad)
            {
                behaviour.SetAllData();
            }
        }

    }
}
