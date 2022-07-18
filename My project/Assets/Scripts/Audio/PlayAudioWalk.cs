using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using CMF;

public class PlayAudioWalk : MonoBehaviour
{
    AudioControl controller;
    
    string normal = "normal"; // probably using for like walking on harder normal surfaces, wood, cement etc
    string snow = "snow";
    string sewer = "sewer";
    string grass = "grass";

    private void Start()
    {
        controller = GameObject.Find("Player").GetComponent<AudioControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerLowerBoundTrigger" && this.gameObject.tag == "Ground")
        {
            controller.ChangeSoundSurface(normal);

        }
        else if (other.gameObject.tag == "PlayerLowerBoundTrigger" && this.gameObject.tag == "Snow")
        {
            controller.ChangeSoundSurface(snow);
        }
        else if (other.gameObject.tag == "PlayerLowerBoundTrigger" && this.gameObject.tag == "Sewer")
        {
            controller.ChangeSoundSurface(sewer);
        }
        else if (other.gameObject.tag == "PlayerLowerBoundTrigger" && this.gameObject.tag == "Grass")
        {
            controller.ChangeSoundSurface(grass);
        }
        else //just reuse default if things dont go well honestly or maybe dont play idk yet decide later
        {
            controller.ChangeSoundSurface(normal);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerLowerBoundTrigger")
        {
            controller.ChangeSoundSurface(normal);
        }
    }
       

}
