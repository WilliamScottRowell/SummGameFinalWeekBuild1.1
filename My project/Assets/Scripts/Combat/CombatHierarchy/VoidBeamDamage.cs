using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBeamDamage : MonoBehaviour
{
    public int Damage = 12;
    GameObject player, talentUI;
    PlayerStatsManager statsHandler;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        statsHandler = player.GetComponent<CombatController>().GetStatSystem();
        Damage += statsHandler.magicDamage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TakeDamage>())
        {
            other.GetComponent<TakeDamage>().LowerHealth(Damage);
        }
    }
}
