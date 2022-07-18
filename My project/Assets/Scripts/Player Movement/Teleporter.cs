using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF {
    public class Teleporter : MonoBehaviour
    {
        public GameObject teleportLocation;
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.GetComponentInParent<Mover>())
            {
                other.gameObject.GetComponentInParent<Mover>().gameObject.transform.position = teleportLocation.transform.position;
            }
            
        }

    }
}
