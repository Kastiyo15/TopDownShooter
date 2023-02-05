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
    public LevelManager.LevelData Z_PlayerLevelData = new LevelManager.LevelData();
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
    public int Z_TotalWavesCompleted; // H/M/S played
    public float Z_TotalPlayTime; // H/M/S played
    public int Z_TotalRuns; // How many times player has restarted

    [Header("Shooting Stats")]
    public int Z_TotalEnemiesKilled;
    public int Z_TotalDeaths;
    public float Z_TotalKDA;
    public int Z_TotalBulletsFired;
    public int Z_TotalBulletsHit;
    public float Z_TotalAccuracy; // bullets hit / fired / 100
    public int Z_TotalDamage;
    #endregion

    // Prefix W_
    #region PLAYER WEAPON STATS
    [Header("Weapon Stats")]
    public int Z_WeaponID;
    public string Z_WeaponName;
    public int Z_WeaponClipSize;
    public float Z_WeaponFireRate;
    public float Z_WeaponSpread;

    [Header("Weapon Base Stat Multipliers")]
    public int Z_WeaponClipSizeMult;
    public float Z_WeaponFireRateMult;
    public float Z_WeaponSpreadMult;
    #endregion

    // Prefix B_
    #region PLAYER BULLET STATS
    [Header("Bullet Stats")]
    public int Z_BulletID;
    public string Z_BulletName;
    public GameObject Z_BulletPrefab;
    public int Z_BulletDamage;
    public float Z_BulletVelocity;
    public int Z_BulletAmount;

    [Header("Bullet Base Stat Multipliers")]
    public int Z_BulletDamageMult;
    public float Z_BulletVelocityMult;
    public float Z_BulletAmountMult;
    #endregion

    // All Lists
    #region ALL PLAYER, WEAPON, BULLET, SCORE LISTS
    [Header("Player General and Score Lists")]
    public List<int> Z_TalentPointsSpentGeneral = new List<int>();
    public List<int> Z_TalentPointsSpentHealth = new List<int>();
    public List<int> Z_TalentPointsSpentDefence = new List<int>();
    public List<int> Z_TalentPointsSpentAgility = new List<int>();
    public List<int> Z_TalentPointsSpentScore = new List<int>();

    [Header("Weapon and Bullet Lists")]
    public List<int> Z_TalentPointsSpentRifleWeapon = new List<int>();
    public List<int> Z_TalentPointsSpentShotgunWeapon = new List<int>();
    public List<int> Z_TalentPointsSpentRifleBullet = new List<int>();
    public List<int> Z_TalentPointsSpentShotgunBullet = new List<int>();
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
