using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlades : Projectile
{
    public WindBlades() : base("WindBlades", Type.MAGIC)
    {  
    }
    void Start()
    {
        base.Start();
    }
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "WindBlade")
        {
            base.OnCollisionEnter(collision);
        }
    }
}