using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrinker : MonoBehaviour
{
    // Variables to set scaling logic parameters
    public float minXSize;
    public float minYSize;
    public float minZSize;
    public float maxXSize;
    public float maxYSize;
    public float maxZSize;
    public float scalingTime;
    public float scaleEndTolerance = 0.1f;
    public bool scaleUp;
    public bool scaleDown;

    // Time management
    public float startDelayTime = 0.0f;
    bool started = false;


    // Private variables to control scaling logic
    Vector3 minScaling;
    Vector3 maxScaling;
    bool onlyDown;

    // Private variable used for smooth damp function
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if(!scaleUp && scaleDown)
        {
            onlyDown = true;
        }
        Invoke("StartGrowShrink", startDelayTime);
    }

    void StartGrowShrink()
    {
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            // Update scale references each frame in case changes are made in the inspector
            minScaling = new Vector3(minXSize, minYSize, minZSize);
            maxScaling = new Vector3(maxXSize, maxYSize, maxZSize);

            // Core scaling logic!
            if (scaleUp)
            {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, maxScaling, ref velocity, scalingTime);
                if (transform.localScale.x >= maxScaling.x - scaleEndTolerance)
                {
                    scaleUp = false;
                }
            }
            else if (scaleDown)
            {
                transform.localScale = Vector3.SmoothDamp(transform.localScale, minScaling, ref velocity, scalingTime);
                if (transform.localScale.x <= minScaling.x + scaleEndTolerance)
                {
                    if (!onlyDown)
                    {
                        scaleUp = true;
                    }
                }
            }
        }
    }
}
