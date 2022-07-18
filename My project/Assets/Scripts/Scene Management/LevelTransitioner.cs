using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTransitioner : MonoBehaviour
{
    public string nextSceneName;

    bool inRangeToMove = false;
    TMP_Text floatingText;

    private void Start()
    {
        floatingText = GetComponentInChildren<TMP_Text>();
        if(floatingText != null)
        {
            floatingText.enabled = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("In range");
            inRangeToMove = true;
            if (floatingText != null)
            {
                floatingText.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRangeToMove = false;
            if (floatingText != null)
            {
                floatingText.enabled = false;
            }
        }
    }

    private void Update()
    {
        if(inRangeToMove)
        {
            if(Input.GetKey(KeyCode.E))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
