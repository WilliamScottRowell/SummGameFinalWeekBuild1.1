using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCycler : MonoBehaviour
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

    private void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }

    public void Update()
    {
        currSpeed = speed * Time.deltaTime;  // Get current speed based on frame rate
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
                countUp = !countUp;
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
                countUp = !countUp;
            }
        }

        transform.position = new Vector3(x, y, z);
    }
}
