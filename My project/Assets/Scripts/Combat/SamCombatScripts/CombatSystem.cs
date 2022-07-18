using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CMF
{
 
    // master system. controls DamageDisplay, which is the slave system, during a HPbar pulse.
    public class CombatSystem : MonoBehaviour
    {
        public float attackDamage = 1;
        public float health = 5;
        public float maxHealth = 5;
        public float invulnerabilityTime = 0.2f;
        public float knockbackTime = 5f;
        public int whatIsEnemy = 8;
        public LayerMask EnemyLayerMask;

        public GameObject HPText;

        public DamageDisplay dd;

        [SerializeField]
        private bool canBeAttacked = true;

        private bool waiting = false;

        [SerializeField]
        private ModifiedWalkerController AWC;
        // Start is called before the first frame update
        void Start()
        {
            AWC = gameObject.GetComponent<ModifiedWalkerController>();
        }

        // Update is called once per frame
        void Update()
        {
            HPText.GetComponent<TMPro.TextMeshProUGUI>().text = "HP: " + health;
        }

        public void onHit(float damage)
        {
            if (canBeAttacked && health > 0)
            {
                health -= damage;


                dd.StartPulse();
            }
            if (!waiting)
            {

                StartCoroutine(waitCoroutine());
            }
            if (health<= 0)
            {
                Debug.Log("health is zero.");
            }

        }

        public void onHeal(float healing)
        {
            health += healing;
            if (health > maxHealth)
            {
                health = maxHealth;
            }

            dd.StartPulse();

        }




        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.layer == whatIsEnemy)
            {
                JumpEnemies enemy = collision.gameObject.GetComponent<JumpEnemies>();
                


                //Debug.Log(collision.gameObject);
                Vector3 pointOfCollision = collision.GetContact(0).point;
                RaycastHit hit;
                Vector3 normal = new Vector3();
                //Debug.Log(Physics.Raycast(transform.position, pointOfCollision - transform.position, out hit, 20, EnemyLayerMask));
                //Debug.DrawRay(transform.position, pointOfCollision - transform.position, Color.red);
                if (Physics.Raycast(transform.position, pointOfCollision-transform.position, out hit, 20, EnemyLayerMask))
                {
                    
                    normal = hit.normal;
                    //Debug.Log(normal);
                    //StartCoroutine(knockbackCoroutine(normal));
                    transform.position += Vector3.up * 0.2f;
                    AWC.SetMomentum(normal * 10 + Vector3.up * 5);
                    //GetComponent<Rigidbody>().velocity = normal * 10;
                    if (canBeAttacked && health > 0)
                    {
                        health -= enemy.attackDamage;

                        if (dd.enableFade)
                        {
                            dd.StartPulse();
                            Debug.Log("begin pulse");
                        }

                    }
                    if (!waiting)
                    {
                        StartCoroutine(waitCoroutine());
                    }
                    if (health <= 0)
                    {
                        Debug.Log("Health is zero.");
                    }

                }

            }

        }


        IEnumerator waitCoroutine()
        {
            waiting = true;
            canBeAttacked = false;
            yield return new WaitForSeconds(invulnerabilityTime);
            canBeAttacked = true;
            waiting = false;
        }


    }
}