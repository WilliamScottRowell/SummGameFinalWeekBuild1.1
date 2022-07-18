using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CMF
{
    public class EnemyProjectileUnguided : Projectile
    {

        public Rigidbody rb;

        public int safeLayer = 10;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = initialVelocity;
            Destroy(gameObject, 5);
        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Mover>())
            {
                other.GetComponent<CombatSystem>().onHit(damage);
            }
            if (!(other.gameObject.layer == safeLayer)){
                Destroy(gameObject);
            }
        }
    }
}
