using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    // This is where we will store and load all the the players stats
    // Prefix G_
    #region PLAYER HEALTH, DEFENCE, AGILITY STATS
    [Header("General Stats")]
    public int Z_PlayerHealth; // affects number of hitpoints
    public int Z_PlayerDefence; // reduces damage taken, damage - defence
    public float Z_PlayerAgility; // affects how fast player moves
    #endregion

    // Prefix H_
    #region PLAYER HEALTH STATS
    [Header("Health Stats")]
    public int Z_PlayerRegenerationValue; // how much health is regenerated
    public float Z_PlayerRegenerationRate; // how quickly health regenerates
    #endregion

    // Prefix D_
    #region PLAYER DEFENCE STATS
    [Header("Defence Stats")]
    public float Z_PlayerBlockChance; // percentage, take no damage if blocked
    public int Z_PlayerDefenceMultiplier; // increase this to increase how many points the base defence stat blocks
    #endregion

    // Prefix A_
    #region PLAYER AGILITY STATS
    [Header("Agility Stats")]
    public float Z_PlayerDashDistance; // how far player dashes when key is pressed
    public float Z_PlayerDashTimer; // how often the player can dash
    #endregion

    // Prefix L_  May add system where enemies are given a level and their base stats are randomly assigned
    #region PLAYER LEVEL STATS
    [Header("Level Stats")]
    public int Z_Level; // player level
    public float Z_CurrentXp; // player current xp
    public float Z_RequiredXp; // player required xp to level up
    public int Z_TalentPoints; // how many total talent points player has
    public int Z_TalentPointsSpent; // total talent points spent
    public int Z_TalentPointsAvailable; // total remaining talent points
    public List<int> Z_WeaponPointsSpentList = new List<int>(); // list to save where and how many talents points were spent on weapon skills 
    #endregion

    // Prefix S_
    #region PLAYER SCORE STATS
    [Header("Score Stats")]
    public int Z_TotalScore; // total score collected 
    public int Z_ScorePerKill; // score gained from killing any enemy
    public int Z_TimerScore; // Score gained every time the timer goes down
    public float Z_Timer; // When reaches zero, add TimerScore
    public int Z_MaximumMultiplier; // highest score multiplier player can reach
    #endregion

    // Prefix C_
    #region PLAYER CAREER STATS
    [Header("Career Stats")]
    public float Z_TotalPlayTime; // H/M/S played
    public int Z_TotalRuns; // How many times player has restarted

    [Header("Shooting Stats")]
    public int Z_TotalEnemiesKilled;
    public int Z_TotalDeaths;
    public float Z_TotalKDA;
    public int Z_TotalBulletsFired;
    public int Z_TotalBulletsHit;
    public float Z_TotalAccuracy; // bullets hit / fired / 100
    #endregion


    // First method to convert class to Json string
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }


    // Second method to take the Json string and fill this class
    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}


public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}
