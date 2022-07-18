using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CMF {
    public class DamageDisplay : MonoBehaviour
    {

        public bool enableFade;

        CombatSystem cs;

        Color nextColor;



        bool isPulsing = false;


        public float pulseTime = 1f;


        [SerializeField]
        private Image healthbar;
        // Start is called before the first frame update
        void Start()
        {
            cs = transform.parent.parent.parent.GetComponent<CombatSystem>();
            healthbar = GetComponentInChildren<Image>();
            nextColor = healthbar.color;
            enableFade = true;
        }

        // Update is called once per frame
        void Update()
        {
            healthbar.color = nextColor;
            healthbar.fillAmount = cs.health / cs.maxHealth;
            if (enableFade)
            {
                if (!isPulsing)
                {
                    nextColor.a = 0;
                }
            }
            else
            {
                nextColor.a = 1;
            }
        }


        public void StartPulse() {
            //Debug.Log(isPulsing);
            if (!isPulsing)
            {
                StartCoroutine(PulseHPBarOnce());
            }
            else
            {
                StopCoroutine(PulseHPBarOnce());
                StartCoroutine(PulseHPBarOnce());
            }
            
        }


        public float fadeRate = 2;
        IEnumerator PulseHPBarOnce()
        {
            isPulsing = true;
            nextColor.a = 1f;
            while (nextColor.a - 0 >= 0.1f)
            {
                //Debug.Log("Pulsing, " + nextColor.a);
                nextColor.a = Mathf.Lerp(nextColor.a, 0, fadeRate * Time.deltaTime);
                yield return null;
            }

            nextColor.a = 0;
            isPulsing = false;
            //Debug.Log("finished pulsing");
            yield return null;
        }


    }
}