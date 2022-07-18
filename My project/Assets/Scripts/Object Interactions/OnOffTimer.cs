using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffTimer : MonoBehaviour
{
    bool objectOn = true;
    public float timerObjectOnLength = 2f;
    public float timerObjectOffLength = 3f;
    float timeLeftOn;
    float timeLeftOff;

    new Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();

        timeLeftOn = timerObjectOnLength;
        timeLeftOff = timerObjectOffLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(objectOn)
        {
            timeLeftOn -= Time.deltaTime;
            if(timeLeftOn <= 0)
            {
                timeLeftOff = timerObjectOffLength;
                objectOn = false;
                light.enabled = false;
            }
        }
        else
        {
            timeLeftOff -= Time.deltaTime;
            if(timeLeftOff <= 0)
            {
                timeLeftOn = timerObjectOnLength;
                objectOn = true;
                light.enabled = true;
            }
        }

    }
}
