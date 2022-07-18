using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CMF
{
    public class UIController : MonoBehaviour
    {

        public GameObject menuPanel;

        public Button enableFade;
        public Button disableFade;

        public DamageDisplay dd;

        public bool menuIsPresent = false;
        // Start is called before the first frame update
        void Start()
        {
            enableFade.onClick.AddListener(SetEnableFade);
            disableFade.onClick.AddListener(SetDisableFade);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !menuIsPresent)
            {
                menuPanel.transform.position = new Vector3(Screen.width / 2 / 1.2f, Screen.height / 2, 0);
                menuIsPresent = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && menuIsPresent)
            {
                menuPanel.transform.position = new Vector3(0, 1200, 0);
                menuIsPresent = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void SetEnableFade()
        {

            dd.enableFade = true;

        }
        void SetDisableFade()
        {

            dd.enableFade = false;
        }

    }
}