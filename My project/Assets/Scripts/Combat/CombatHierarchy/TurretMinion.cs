using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMinion : MonoBehaviour
{

    public Transform turretModel;
    public float rotateSpeed = 10;
    public float attackInterval = 1;
    public float detectionRange = 40;
    public float speed = 1f;
    public GameObject Player, Origin;
    public SphereCollider sc;
    CombatController c;
    private float nextAttackTime;
    bool can = true;
    public float projectileSpeed = 200;
    List<GameObject> enemies = new List<GameObject>();
    void Start()
    {
        turretModel = this.transform.GetChild(0);
        nextAttackTime = Time.time;
        Player = GameObject.Find("Player");
        c = Player.GetComponent<CombatController>();
        sc = this.GetComponent<SphereCollider>();
        sc.radius = 10f;
        Destroy(gameObject, 8f);
    }
    void Update()
    {
        Attack();
        FollowPlayer();
    } 
    void FollowPlayer() {
        Vector3 distance = (Origin.transform.position - turretModel.position);
        if(distance.magnitude > 1f) {
            this.transform.Translate(distance * Time.deltaTime);
        }
    }
    void Attack() {
        if(enemies.Count > 0) {
            while(enemies.Count > 0 && enemies[0] == null) {
                enemies.RemoveAt(0);
            }
            if(enemies.Count == 0) {
                return;
            }
            turretModel.rotation = Quaternion.Slerp(turretModel.rotation, Quaternion.LookRotation(enemies[0].transform.position - turretModel.position), Time.deltaTime * rotateSpeed);
            if (Time.time >= nextAttackTime)
            {    
                Vector3 direction = (enemies[0].transform.position - turretModel.position).normalized * projectileSpeed;
                c.CreateMinionBullets(turretModel.position, direction);
                nextAttackTime = Time.time + attackInterval;
            }
        }
    }
    void OnTriggerEnter(Collider c) {
        if(c.gameObject.GetComponent<TakeDamage>() && enemies.IndexOf(c.gameObject) < 0) {
            enemies.Insert(enemies.Count, c.gameObject);
        }
    }
    void OnTriggerExit(Collider c) {
        if(c.gameObject.GetComponent<TakeDamage>() && enemies.IndexOf(c.gameObject) >= 0) {
            enemies.Remove(c.gameObject);
        }
    }
}