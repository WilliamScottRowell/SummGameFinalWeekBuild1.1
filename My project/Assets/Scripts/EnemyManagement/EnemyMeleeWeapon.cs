using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CMF
{
    public class EnemyMeleeWeapon : MonoBehaviour
    {


        public float damage = 5;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {

            Debug.Log(other.gameObject);
            if (other.gameObject.GetComponent<CombatSystem>() && transform.parent.GetComponentInParent<MeleeEnemy>().isSlashing)
            {
                other.gameObject.GetComponent<CombatSystem>().onHit(damage);
            }
        }
    }
}