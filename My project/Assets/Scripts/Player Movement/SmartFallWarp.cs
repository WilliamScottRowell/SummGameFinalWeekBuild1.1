using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartFallWarp : MonoBehaviour
{
    GameObject player;
    public int warpHeight;
    Vector3 respawnPoint;
    bool grounded = false;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    public void SmartWarpWaypointSet()
    {
        if(grounded)
        {
            respawnPoint = gameObject.transform.position;
        }  
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= warpHeight)
        {
            player.transform.position = respawnPoint;
        }
    }
}
