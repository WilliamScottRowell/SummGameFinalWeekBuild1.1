using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : TrapsFather
{
    public bool shoot = false;
    public float shootSpeed = 8.0f;

    public bool left = false;
    public bool right = false;
    public bool back = false;
    public bool forward = false;
    public bool up = false;
    public bool down = false;

    public bool destory = false;

    float aTime = 0.0f;
    public float removeObjectTime = 3.0f;

 

    // Start is called before the first frame update
    void Shoot()
    {
        if (shoot == true)
        {
            if (left == false)
            { 
                transform.Translate(Vector3.left * shootSpeed * Time.deltaTime, Space.World);
            }
            if (right == false)
            {
                transform.Translate(Vector3.right * shootSpeed * Time.deltaTime, Space.World);
            }
            if (back == false)
            {
                transform.Translate(Vector3.back * shootSpeed * Time.deltaTime, Space.World);
            }
            if (forward == false)
            {
                transform.Translate(Vector3.forward * shootSpeed * Time.deltaTime, Space.World);
            }
            if (up == false)
            {
                transform.Translate(Vector3.up * shootSpeed * Time.deltaTime, Space.World);
            }
            if (down == false)
            {
                transform.Translate(Vector3.down * shootSpeed * Time.deltaTime, Space.World);
            }
            aTime += Time.deltaTime;
            RemoveObject();
        }

    }


    void RemoveObject()
    {
        if (aTime > removeObjectTime)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        base.Update();
    }
}
