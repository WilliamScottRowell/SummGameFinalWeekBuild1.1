using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRotate : TrapsFather
{
    public bool begin = false;

    public float speed = 2f;
    public float maxRotation = 45f;
    float NewTime = 3.8f;

    void Trigger()
    {
         
        if (begin == true)
        {

            NewTime += Time.deltaTime;
            transform.rotation = Quaternion.Euler(maxRotation * Mathf.Sin(NewTime * speed), 0f, 0f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Trigger();
        base.Update();
    }
}
