using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{

    public float damage = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemies>() && GetComponentInParent<Melee>().isSlashing)
        {
            other.gameObject.GetComponent<Enemies>().onHit(damage);
        }
    }
}
