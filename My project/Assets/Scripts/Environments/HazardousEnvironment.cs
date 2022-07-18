using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CMF {
    public class HazardousEnvironment : MonoBehaviour
    {

        public float damage = 1;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponentInParent<CombatSystem>())
            {
                other.gameObject.GetComponentInParent<CombatSystem>().onHit(damage);

            }
        }
    }
}