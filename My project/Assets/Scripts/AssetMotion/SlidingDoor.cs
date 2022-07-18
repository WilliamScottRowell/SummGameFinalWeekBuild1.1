using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF {
    public class SlidingDoor : MonoBehaviour
    {
        bool isCoroutineOver = true;

        bool opened = false;

        public float slideDistance = 10;

        public Transform doorModel;

        public Vector3 beginPosition;

        // Start is called before the first frame update
        void Start()
        {
            beginPosition = doorModel.position;
        }

        // Update is called once per frame
        private void OnTriggerStay(Collider other)
        {
            //Debug.Log(other.gameObject);
            if (other.gameObject.GetComponentInParent<AdvancedWalkerController>() && Input.GetKey(KeyCode.E))
            {
                //Debug.Log("player in range");
                if (isCoroutineOver)
                {
                    StartCoroutine(slide());
                }
            }
        }


        public float rateOfOpening = 2f;
        IEnumerator slide()
        {
            isCoroutineOver = false;
            Vector3 target;
            if (!opened)
            {
                target = doorModel.position + slideDistance * -doorModel.forward;
            }
            else
            {
                target = beginPosition;
            }
            
            //Debug.Log(target + ", door is open:" + opened);
            while (Mathf.Abs(doorModel.position.magnitude - target.magnitude) > 2f)
            {
                doorModel.position = Vector3.Lerp(doorModel.position, target, Time.deltaTime * rateOfOpening);
                yield return null;
            }
            doorModel.position = target;
            opened = !opened;
            isCoroutineOver = true;
            yield return null;

        }
        

    }
}
