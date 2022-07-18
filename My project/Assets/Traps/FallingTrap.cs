using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : TrapsFather
{
    public bool fall = false;
    public float fallSpeed = 8.0f;

    float fTime = 0.0f;
    public float removeObjectTime = 3.0f;


    // Start is called before the first frame update

    void Fall()
    {
        if(fall == true)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
            GetComponent<Rigidbody>().useGravity = true;
            fTime += Time.deltaTime;
            RemoveObject();
        }
       
    }


    void RemoveObject()
    {
        if(fTime > removeObjectTime)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Fall();
        base.Update();
    }
}
