using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWarp : MonoBehaviour
{
    public int warpHeight;
    public GameObject respawnPoint;

    private void Start()
    {
        respawnPoint = GameObject.Find("RespawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y <= warpHeight)
        {
            gameObject.transform.position = respawnPoint.transform.position;
        }
    }
}
