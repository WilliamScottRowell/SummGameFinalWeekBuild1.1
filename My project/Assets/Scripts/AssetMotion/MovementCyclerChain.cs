using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCyclerChain : MonoBehaviour
{
    public float speed;
    public float distance;
    float currSpeed;

    float distanceTraveled;

    public bool xMove;
    public bool yMove;
    public bool zMove;

    bool countUp = true;

    float x;
    float y;
    float z;

    // Logic for chaining cyclers together
    public bool running = false;

    public MovementCyclerChain inputCycle;
    public MovementCyclerChain outputCycle;

    private void Start()
    {
        UpdateCoordinates();

        // If this is the first loop in the cycle, start it turned on!
        if (inputCycle == null)
        {
            running = true;
        }
    }

    void UpdateCoordinates()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }

    public void Update()
    {
        currSpeed = speed * Time.deltaTime;  // Get current speed based on frame rate

        if(running)
        {
            CycleInChain();
        }
    }

    // Method to be called by the end of the previous chain to continue the cycle
    public void ContinueChain()
    {
        UpdateCoordinates();
        running = true;
    }

    void CycleInChain()
    {
        if (countUp)
        {
            if (xMove)
            {
                x += currSpeed;
            }
            if (yMove)
            {
                y += currSpeed;
            }
            if (zMove)
            {
                z += currSpeed;
            }
            distanceTraveled += currSpeed;

            if (distanceTraveled >= distance)
            {
                countUp = !countUp; // Flips the direction of travel for the next time the algorithm is run
                if(outputCycle != null)  // Only pause the looping if this isn't the final part of the cycle
                {
                    outputCycle.ContinueChain();
                    running = false; // Stop this chain running once this section has ended
                }
                
            }
        }
        else
        {
            if (xMove)
            {
                x -= currSpeed;
            }
            if (yMove)
            {
                y -= currSpeed;
            }
            if (zMove)
            {
                z -= currSpeed;
            }
            distanceTraveled -= currSpeed;

            if (distanceTraveled <= 0)
            {
                countUp = !countUp; // Flips the direction of travel for the next time the algorithm is run
                if(inputCycle != null) // Only pause the looping if this isn't the first part of the cycle
                {
                    inputCycle.ContinueChain();
                    running = false; // Stop this chain running once this section has ended
                }
               
            }
        }

        transform.position = new Vector3(x, y, z);
    }
}
