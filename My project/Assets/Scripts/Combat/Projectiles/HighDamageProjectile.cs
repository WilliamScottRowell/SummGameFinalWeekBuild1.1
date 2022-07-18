using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF {
    public class HighDamageProjectile :Projectile
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Rigidbody>().velocity = initialVelocity;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Enemies>())
            {
                other.gameObject.GetComponent<Enemies>().onHit(damage);
            }


            if (!(other.gameObject.layer == 10 || other.gameObject.GetComponentInParent<Mover>() || other.gameObject.GetComponent<Mover>()))
                Destroy(gameObject);
        }
    }

}
