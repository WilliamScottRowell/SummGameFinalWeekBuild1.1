using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerTesting : MonoBehaviour
{
    PlayerStats stats;
    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        if(gameManager != null)
        {
            stats = gameManager.GetComponent<PlayerStats>();
        }
        else
        {
            Debug.Log("Major issue!!! Cannot find the game manager properly!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            stats.health++;
        }
    }
}
