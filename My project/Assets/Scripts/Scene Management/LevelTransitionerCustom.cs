using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTransitionerCustom : MonoBehaviour
{

    //Attach this to the where the original LevelTransitioner script was. This script also needs an animation controller and animation.
    //I have also included those two, however the animation will need to be modified for whatever button object will be made
    //It is possible to scrap the animation method and simply just make a translate function, which would be much easier to implement.
    //If you want that change, just tell me and I can modify this script (it is an easy fix)


    public string nextSceneName;
    public string key;
    public Animator anim;
    public Transform player;

    bool inRangeToMove = false;
    TMP_Text floatingText;

    private void Start()
    {
        floatingText = GetComponentInChildren<TMP_Text>();
        floatingText.text = "Press '" + key.ToUpper() + "' to move to a new level!";
        floatingText.enabled = false;
        player = GameObject.Find("Player").transform;
        Debug.Log(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("In range");
            inRangeToMove = true;
            floatingText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRangeToMove = false;
            floatingText.enabled = false;
        }
    }

    private void Update()
    {
        if(inRangeToMove)
        {
            if(Input.GetKey(key))
            {
                anim.SetBool("Press", true);
                Invoke("SceneChange", 1.4f);
            }
        }

        floatingText.transform.LookAt(player);
    }
    private void SceneChange()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
