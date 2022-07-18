using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikes : Projectile
{
    float time = 0f;
    public IceSpikes() : base("IceSpikes", Type.MAGIC) 
    {
    }
    void Start()
    {
        base.Start();
        rigid.isKinematic = true;
        Debug.Log(InitialVelocity);
    }
    void Update()
    {
        time = time + Time.deltaTime;
        if(time >= 1f && rigid.isKinematic) {
            rigid.isKinematic = false;
            rigid.velocity = InitialVelocity;
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Player")
        {
            base.OnCollisionEnter(collision);
        }
    }
}
