using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int health, deathExp;
    GameObject gameManager;
    LevelingSystem l;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        l = gameManager.GetComponentInChildren<LevelingSystem>();
    }
    void Update()
    {
        CheckHealth();
    }
    void CheckHealth()
    {
        if (health <= 0)
        {
            l.EarnExperience(deathExp);
            GameObject.Destroy(this.gameObject);
        }
    }
    public void LowerHealth(int damage)
    {
        health -= damage;
    }
}
