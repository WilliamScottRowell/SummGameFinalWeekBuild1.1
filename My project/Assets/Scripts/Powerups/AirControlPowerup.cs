using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    public class AirControlPowerup : MonoBehaviour
    {
        public float addedAirControlRate = 25;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<ModifiedWalkerController>())
            {

                other.gameObject.GetComponent<ModifiedWalkerController>().airControlRate += addedAirControlRate;
                Destroy(gameObject);
            }
        }
    }
}