using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add System.IO to work with files!

public class GameDataManager : MonoBehaviour
{
    // Reference to live player data
    PlayerStats playerStats = new PlayerStats();
    public PlayerStatsManager playerStatsViewer;

    // Create a field for the save file.
    string path = "";
    string persistentPath = "";

    // Temp player stats
    PlayerStats gameData = new PlayerStats();

    void Awake()
    {
        // Update the path once the persistent path exists.
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        LoadData();
    }

    public void LoadData()
    {
        // Does the file exist?
        if (File.Exists(path))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(path);

            //Debug.Log("Data: " + fileContents);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            gameData = JsonUtility.FromJson<PlayerStats>(fileContents);

            //Debug.Log("Success!");

            //UpdatePlayerStatsFromSave();
        }
        else
        {
            SaveNewPlayerData();
        }
    }

    public void SavaData()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(LoadLivePlayerStats());

        // Write JSON to file.
        File.WriteAllText(path, jsonString);
    }

    public void SaveNewPlayerData()
    {
        playerStats = new PlayerStats();
        UpdateLivePlayerData(playerStats);
    }

    void UpdatePlayerStatsFromSave()
    {
        UpdateLivePlayerData(gameData);
    }

    public void UpdateLivePlayerData(PlayerStats stats)
    {
        //SeeCurrentStats(stats);
        playerStatsViewer.UpdateViewerStats(stats);
    }

    void SeeCurrentStats(PlayerStats stats)
    {
        Debug.Log("hp " + stats.health);
        Debug.Log("maxHP " + stats.maxHealth);
        Debug.Log("lvl " + stats.playerLevel);
        Debug.Log("currency " + stats.currencyTotal);
        Debug.Log("talents " + stats.currentTalentPointsHeld);
    }

    PlayerStats LoadLivePlayerStats()
    {
        PlayerStats stats = new PlayerStats();
        stats.maxHealth = playerStatsViewer.maxHealth;
        stats.health = playerStatsViewer.health;
        stats.playerLevel = playerStatsViewer.playerLevel;
        stats.currencyTotal = playerStatsViewer.currencyTotal;
        stats.currentTalentPointsHeld = playerStatsViewer.currentTalentPointsHeld;
        stats.lives = playerStats.lives;
        return stats;
    }

    // Autosave feature
    public bool autoSave;
    public float autoSaveTimeGap = 5.0f;
    float autoSaveTimePassed;


    private void Update()
    {
        autoSaveTimePassed += Time.deltaTime;
        if (autoSave && autoSaveTimePassed >= autoSaveTimeGap)
        {
            autoSaveTimePassed = 0;
            //Debug.Log("Data automatically saved!");
            SavaData();
        }
    }
}
