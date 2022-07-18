using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : MonoBehaviour
{
    public float health = 2;
    public float maxHealth = 2;
    public float attackInvulnerabilityTime = 0.2f;
    [SerializeReference]
    public bool canBeAttacked = true;
    [SerializeReference]
    public bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public IEnumerator waitCoroutine()
    {
        waiting = true;
        canBeAttacked = false;
        yield return new WaitForSeconds(attackInvulnerabilityTime);
        canBeAttacked = true;
        waiting = false;
    }
    
    public virtual void Attack()
    {


    }

    public virtual void onHit(float damageAmount)
    {

        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
