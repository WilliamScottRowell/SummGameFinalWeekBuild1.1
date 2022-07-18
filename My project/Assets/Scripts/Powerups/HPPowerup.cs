using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CMF
{
    public class HPPowerup: MonoBehaviour
    {
        public float addedHP = 3;

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
            //Debug.Log("player in range");
            if (other.gameObject.GetComponent<AdvancedWalkerController>())
            {
                
                other.gameObject.GetComponent<CombatSystem>().health += addedHP;
                other.gameObject.GetComponent<CombatSystem>().maxHealth += addedHP;
                Destroy(gameObject);
            }
        }
    }
}
