using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CMF {
    public class FirstPersonToggler : MonoBehaviour
    {
        public GameObject player;
        public GameObject capguy;
        public Camera ThirdPersonCam;
        public Camera FirstPersonCam;
        private Vector3 firstPersonCamPosition;
        public bool inFirstPerson = false;
        public Vector3 originalPosition;

        // Start is called before the first frame update
        void Start()
        {

            player = GameObject.Find("Player");
            capguy = GameObject.Find("Capguy");

            firstPersonCamPosition = FirstPersonCam.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {

            if (GetComponentInParent<ModifiedWalkerController>().hasReducedHeight)
            {
                FirstPersonCam.transform.localPosition = new Vector3(firstPersonCamPosition.x,
                    (firstPersonCamPosition / GetComponentInParent<ModifiedWalkerController>().crouchHeightModifier).y,
                    firstPersonCamPosition.z);

            }
            else
            {
                FirstPersonCam.transform.localPosition = firstPersonCamPosition;
            }

            if (Input.GetKeyDown("z"))
            {
                inFirstPerson = !inFirstPerson;

                if (inFirstPerson)
                {
                    ChangeToFirstPerson();
                }
                else
                {
                    ChangeToThirdPerson();
                }
            }
        }

        void ChangeToFirstPerson()
        {
            FirstPersonCam.depth = 2;
            ThirdPersonCam.depth = 1;

            capguy.SetActive(false);
        }

        void ChangeToThirdPerson()
        {
            FirstPersonCam.depth = 1;
            ThirdPersonCam.depth = 2;
            capguy.SetActive(true);
        }
    }
}