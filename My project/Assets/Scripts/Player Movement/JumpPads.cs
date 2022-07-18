using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CMF
{
    public class JumpPads : MonoBehaviour
    {

        public float jumpStrength = 10;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerLowerBoundTrigger")) {
                if (other.transform.parent.CompareTag("Player")) {
                    ModifiedWalkerController AWC = other.transform.parent.GetComponent<ModifiedWalkerController>();
                    AWC.SetMomentum(new Vector3(AWC.GetMomentum().x, AWC.GetMomentum().y + jumpStrength, AWC.GetMomentum().z));


                }
            }
        }
    }
    
}