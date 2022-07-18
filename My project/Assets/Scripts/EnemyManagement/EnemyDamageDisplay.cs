using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CMF {
    public class EnemyDamageDisplay : MonoBehaviour
    {

        private JumpEnemies je;

        [SerializeField]
        private Transform cameraPosition;

        [SerializeField]
        private Image healthbar;


        // Start is called before the first frame update
        void Start()
        {
            je = GetComponentInParent<JumpEnemies>();
            healthbar = GetComponentInChildren<Image>();

        }

        // Update is called once per frame
        void Update()
        {

            transform.LookAt(cameraPosition);
            healthbar.fillAmount = je.health / je.maxHealth;

        }
    }
}
