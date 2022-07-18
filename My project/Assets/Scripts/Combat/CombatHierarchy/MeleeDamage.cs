using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public Melee meleeController;
    Type type = Type.MELEE;
    public int Damage;
    GameObject player, talentUI;
    PlayerStatsManager statsHandler;
    void Start() {
        Invoke("FindAfterStart", 0.1f);
        //FindAfterStart();
    }
    public void FindAfterStart() {
        meleeController = GetComponentInParent<Melee>();
        player = GameObject.FindWithTag("Player");
        statsHandler = player.GetComponent<CombatController>().GetStatSystem();
        Damage += statsHandler.physicalDamage;
    }
    void OnCollisionEnter(Collision c) {
        if(meleeController.isSlashing) {
            if(c.gameObject.GetComponent<TakeDamage>()) {
            c.gameObject.GetComponent<TakeDamage>().LowerHealth(Damage);
            }
        }
    }
    void OnCollisionExit(Collision c) {
        if(meleeController.isSlashing && c.gameObject.tag != "WindBlade") {
            if(c.gameObject.GetComponent<TakeDamage>()) {
            c.gameObject.GetComponent<TakeDamage>().LowerHealth(Damage);
            }
        }
    }
}
