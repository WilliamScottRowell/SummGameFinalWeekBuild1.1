using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF {
    public class BounceProjectile : Projectile
    {

        
        private int bounceTimes = 0;
        public int maxBounceTimes = 5;


        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Rigidbody>().velocity = initialVelocity;
            Destroy(gameObject, 5);
        }

        // Update is called once per frame

        private void OnCollisionEnter(Collision collision)
        {



            if (collision.gameObject.GetComponent<Enemies>())
            {
                collision.gameObject.GetComponent<Enemies>().onHit(damage);
                Destroy(gameObject);

            }
            bounceTimes++;
            if (bounceTimes >= maxBounceTimes)
            {
                Destroy(gameObject);
            }
        }
    }
}