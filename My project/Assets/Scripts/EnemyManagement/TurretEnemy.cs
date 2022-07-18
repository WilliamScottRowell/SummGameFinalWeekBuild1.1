using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{


    /*
     ###################################################


    Important notice: Using Colliders could improve performance.
    Use the onTriggerStay function of colliders could improve the framerate slightly.


    ###################################################
     
     
     */

    public class TurretEnemy : Enemies
    {

        public Transform turretModel;
        public float rotateSpeed = 10;
        public float attackInterval = 1;
        public GameObject projectile;
        public float detectionRange = 40;
        public Transform playerTransform;
        private float nextAttackTime;
        public float projectileSpeed = 200;
        // Start is called before the first frame update
        void Start()
        {
            nextAttackTime = Time.time;
            playerTransform = GameObject.Find("PlayerCenter").transform;
        }

        public override void Attack()
        {
            //Debug.Log("Next attack in: " + nextAttackTime);
            GameObject proj = Instantiate(projectile, transform.position, new Quaternion());
            proj.GetComponent<Projectile>().initialVelocity = (playerTransform.position - proj.transform.position).normalized * projectileSpeed;
            //Debug.Log("Attacking player " + playerTransform.position + " Launching at " + (proj.transform.position + playerTransform.position));
            //Debug.Log("Attacking player " + playerTransform.position + " Launching at " + (proj.transform.position + playerTransform.position));

        }

        // Update is called once per frame
        void Update()
        {

            RaycastHit hit;
            Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, detectionRange);

            //Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.red);
            //Debug.Log(hit.collider);
            if (hit.collider)
            {
                if (hit.collider.gameObject.GetComponentInParent<Mover>() || hit.collider.gameObject.GetComponent<Mover>())
                    turretModel.rotation = Quaternion.Slerp(turretModel.rotation,Quaternion.LookRotation(playerTransform.position - turretModel.position), Time.deltaTime * rotateSpeed);
            }
            //Debug.Log(turretModel.rotation.eulerAngles);

            

            if (Time.time >= nextAttackTime && hit.collider)
            {

                if (hit.collider.gameObject.GetComponentInParent<Mover>() || hit.collider.gameObject.GetComponent<Mover>())
                //Debug.Log("Attack called at time " + nextAttackTime);
                    Attack();
                    nextAttackTime =Time.time +  attackInterval;
            }
        }
    }
}
