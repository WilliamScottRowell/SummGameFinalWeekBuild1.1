using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    // Core stat tracking
    public int maxHealth;
    public int health;
    public int lives;

    // Collectable stat tracking
    public int currencyTotal;

    // Experience stat tracking
    public int playerLevel;

    // Talent stat tracking
    public int currentTalentPointsHeld;

    public PlayerStats()
    {
        maxHealth = 10;
        health = maxHealth;
        lives = 5;
        currencyTotal = 0;
        playerLevel = 1;
        currentTalentPointsHeld = 0;
    }
}
