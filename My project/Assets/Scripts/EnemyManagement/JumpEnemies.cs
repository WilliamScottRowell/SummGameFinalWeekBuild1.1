using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CMF {
    public class JumpEnemies : Enemies
    {

        public float jumpStrength = 10;
        public float attackDamage = 1;
        [SerializeField]


        // knockback stuff
        public int knockbackStrength = 20;
        public int knockbackHeight = 10;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerLowerBoundTrigger"))
            {
                if (other.transform.parent.CompareTag("Player"))
                {

                    ModifiedWalkerController AWC = other.GetComponentInParent<ModifiedWalkerController>();
                    AWC.SetMomentum(new Vector3(AWC.GetMomentum().x, AWC.GetMomentum().y + jumpStrength, AWC.GetMomentum().z));
                    CombatSystem CS = other.GetComponentInParent<CombatSystem>();
                    if (canBeAttacked)
                    {
                        health -= CS.attackDamage;
                    }
                    if (!waiting)
                    {
                        StartCoroutine(waitCoroutine());
                    }

                    if (health <= 0)
                    {
                        Destroy(gameObject);
                    }
                }

            }

        }

    }
}