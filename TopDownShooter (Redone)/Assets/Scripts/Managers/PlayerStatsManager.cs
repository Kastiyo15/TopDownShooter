// Will need to create a WeaponStatsManager, because I will have two weapons
// Rifle and shotgun
// Weapon skill upgrades will change depending on each weapon
// Also have things like 'enable homing missiles'

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerStatsManager : MonoBehaviour, ISaveable
{

    public static PlayerStatsManager Instance { get; private set; }

    // This is where we will store and load all the the players stats
    // Prefix G_
    #region PLAYER HEALTH, DEFENCE, AGILITY STATS
    [Header("General Stats")]
    public int G_PlayerHealth; // affects number of hitpoints
    public int G_PlayerDefence; // reduces damage taken, damage - defence
    public float G_PlayerAgility; // affects how fast player moves
    #endregion

    // Prefix H_
    #region PLAYER HEALTH STATS
    [Header("Health Stats")]
    public int H_PlayerRegenerationValue; // how much health is regenerated
    public float H_PlayerRegenerationRate; // how quickly health regenerates
    #endregion

    // Prefix D_
    #region PLAYER DEFENCE STATS
    [Header("Defence Stats")]
    public float D_PlayerBlockChance; // percentage, take no damage if blocked
    public int D_PlayerDefenceMultiplier; // increase this to increase how many points the base defence stat blocks
    #endregion

    // Prefix A_
    #region PLAYER AGILITY STATS
    [Header("Agility Stats")]
    public float A_PlayerDashDistance; // how far player dashes when key is pressed
    public float A_PlayerDashTimer; // how often the player can dash
    #endregion

    // Prefix L_  May add system where enemies are given a level and their base stats are randomly assigned
    #region PLAYER LEVEL STATS
    [Header("Level Stats")]
    public int L_Level; // player level
    public float L_CurrentXp; // player current xp
    public float L_RequiredXp; // player required xp to level up
    public int L_TalentPoints; // how many total talent points player has
    public int L_TalentPointsSpent; // total talent points spent
    public int L_TalentPointsAvailable; // total remaining talent points
    #endregion

    // Prefix S_
    #region PLAYER SCORE STATS
    [Header("Score Stats")]
    public int S_TotalScore; // total score collected 
    public int S_ScorePerKill; // score gained from killing any enemy
    public int S_TimerScore; // Score gained every time the timer goes down
    public float S_Timer; // When reaches zero, add TimerScore
    public int S_MaximumMultiplier; // highest score multiplier player can reach
    #endregion

    // Prefix C_
    #region PLAYER CAREER STATS
    [Header("Career Stats")]
    public float C_TotalPlayTime; // H/M/S played
    public int C_TotalRuns; // How many times player has restarted

    [Header("Shooting Stats")]
    public int C_TotalEnemiesKilled;
    public int C_TotalDeaths;
    public float C_TotalKDA;
    public int C_TotalBulletsFired;
    public int C_TotalBulletsHit;
    public float C_TotalAccuracy; // bullets hit / fired / 100
    #endregion

    #region PLAYER TALENT POINTS LISTS
    [Header("Player Points Spent Lists")]
    public List<int> L_TalentPointsSpentGeneral = new List<int>();
    public List<int> L_TalentPointsSpentHealth = new List<int>();
    public List<int> L_TalentPointsSpentDefence = new List<int>();
    public List<int> L_TalentPointsSpentAgility = new List<int>();
    public List<int> L_TalentPointsSpentScore = new List<int>();
    #endregion

    #region PLAYER WEAPON POINTS LIST
    [Header("Weapon Points Lists")]
    public List<int> L_TalentPointsSpentRifleWeapon = new List<int>();
    public List<int> L_TalentPointsSpentShotgunWeapon = new List<int>();
    #endregion

    #region PLAYER BULLET POINTS LIST
    [Header("Bullet Points Lists")]
    public List<int> L_TalentPointsSpentRifleBullet = new List<int>();
    public List<int> L_TalentPointsSpentShotgunBullet = new List<int>();
    #endregion


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    ////////////////////////
    // SAVING AND LOADING //
    ////////////////////////
    public void PopulateSaveData(SaveData a_SaveData)
    {
        // General stats
        a_SaveData.Z_PlayerHealth = G_PlayerHealth;
        a_SaveData.Z_PlayerDefence = G_PlayerDefence;
        a_SaveData.Z_PlayerAgility = G_PlayerAgility;

        // Health
        a_SaveData.Z_PlayerRegenerationValue = H_PlayerRegenerationValue;
        a_SaveData.Z_PlayerRegenerationRate = H_PlayerRegenerationRate;

        // Defence
        a_SaveData.Z_PlayerBlockChance = D_PlayerBlockChance;
        a_SaveData.Z_PlayerDefenceMultiplier = D_PlayerDefenceMultiplier;

        // Agility
        a_SaveData.Z_PlayerDashDistance = A_PlayerDashDistance;
        a_SaveData.Z_PlayerDashTimer = A_PlayerDashTimer;

        // Level
        a_SaveData.Z_Level = L_Level;
        a_SaveData.Z_CurrentXp = L_CurrentXp;
        a_SaveData.Z_RequiredXp = L_RequiredXp;
        a_SaveData.Z_TalentPoints = L_TalentPoints;
        a_SaveData.Z_TalentPointsSpent = L_TalentPointsSpent;
        a_SaveData.Z_TalentPointsAvailable = L_TalentPointsAvailable;

        // Score
        a_SaveData.Z_TotalScore = S_TotalScore;
        a_SaveData.Z_ScorePerKill = S_ScorePerKill;
        a_SaveData.Z_TimerScore = S_TimerScore;
        a_SaveData.Z_Timer = S_Timer;
        a_SaveData.Z_MaximumMultiplier = S_MaximumMultiplier;

        // Career
        a_SaveData.Z_TotalPlayTime = C_TotalPlayTime;
        a_SaveData.Z_TotalRuns = C_TotalRuns;
        a_SaveData.Z_TotalEnemiesKilled = C_TotalEnemiesKilled;
        a_SaveData.Z_TotalDeaths = C_TotalDeaths;
        a_SaveData.Z_TotalKDA = C_TotalKDA;
        a_SaveData.Z_TotalBulletsFired = C_TotalBulletsFired;
        a_SaveData.Z_TotalBulletsHit = C_TotalBulletsHit;
        a_SaveData.Z_TotalAccuracy = C_TotalAccuracy;

        // Player Lists
        a_SaveData.Z_TalentPointsSpentGeneral = L_TalentPointsSpentGeneral;
        a_SaveData.Z_TalentPointsSpentHealth = L_TalentPointsSpentHealth;
        a_SaveData.Z_TalentPointsSpentDefence = L_TalentPointsSpentDefence;
        a_SaveData.Z_TalentPointsSpentAgility = L_TalentPointsSpentAgility;
        a_SaveData.Z_TalentPointsSpentScore = L_TalentPointsSpentScore;

        // Weapon Lists
        a_SaveData.Z_TalentPointsSpentRifleWeapon = L_TalentPointsSpentRifleWeapon;
        a_SaveData.Z_TalentPointsSpentShotgunWeapon = L_TalentPointsSpentShotgunWeapon;
        a_SaveData.Z_TalentPointsSpentRifleBullet = L_TalentPointsSpentRifleBullet;
        a_SaveData.Z_TalentPointsSpentShotgunBullet = L_TalentPointsSpentShotgunBullet;
    }


    public void LoadFromSaveData(SaveData a_SaveData)
    {
        // General stats
        G_PlayerHealth = a_SaveData.Z_PlayerHealth;
        G_PlayerDefence = a_SaveData.Z_PlayerDefence;
        G_PlayerAgility = a_SaveData.Z_PlayerAgility;

        // Health
        H_PlayerRegenerationValue = a_SaveData.Z_PlayerRegenerationValue;
        H_PlayerRegenerationRate = a_SaveData.Z_PlayerRegenerationRate;

        // Defence
        D_PlayerBlockChance = a_SaveData.Z_PlayerBlockChance;
        D_PlayerDefenceMultiplier = a_SaveData.Z_PlayerDefenceMultiplier;

        // Agility
        A_PlayerDashDistance = a_SaveData.Z_PlayerDashDistance;
        A_PlayerDashTimer = a_SaveData.Z_PlayerDashTimer;

        // Level
        L_Level = a_SaveData.Z_Level;
        L_CurrentXp = a_SaveData.Z_CurrentXp;
        L_RequiredXp = a_SaveData.Z_RequiredXp;
        L_TalentPoints = a_SaveData.Z_TalentPoints;
        L_TalentPointsSpent = a_SaveData.Z_TalentPointsSpent;
        L_TalentPointsAvailable = a_SaveData.Z_TalentPointsAvailable;

        // Score
        S_TotalScore = a_SaveData.Z_TotalScore;
        S_ScorePerKill = a_SaveData.Z_ScorePerKill;
        S_TimerScore = a_SaveData.Z_TimerScore;
        S_Timer = a_SaveData.Z_Timer;
        S_MaximumMultiplier = a_SaveData.Z_MaximumMultiplier;

        // Career
        C_TotalPlayTime = a_SaveData.Z_TotalPlayTime;
        C_TotalRuns = a_SaveData.Z_TotalRuns;
        C_TotalEnemiesKilled = a_SaveData.Z_TotalEnemiesKilled;
        C_TotalDeaths = a_SaveData.Z_TotalDeaths;
        C_TotalKDA = a_SaveData.Z_TotalKDA;
        C_TotalBulletsFired = a_SaveData.Z_TotalBulletsFired;
        C_TotalBulletsHit = a_SaveData.Z_TotalBulletsHit;
        C_TotalAccuracy = a_SaveData.Z_TotalAccuracy;

        // Player Lists
        L_TalentPointsSpentGeneral = a_SaveData.Z_TalentPointsSpentGeneral;
        L_TalentPointsSpentHealth = a_SaveData.Z_TalentPointsSpentHealth;
        L_TalentPointsSpentDefence = a_SaveData.Z_TalentPointsSpentDefence;
        L_TalentPointsSpentAgility = a_SaveData.Z_TalentPointsSpentAgility;
        L_TalentPointsSpentScore = a_SaveData.Z_TalentPointsSpentScore;

        // Weapon Lists
        L_TalentPointsSpentRifleWeapon = a_SaveData.Z_TalentPointsSpentRifleWeapon;
        L_TalentPointsSpentShotgunWeapon = a_SaveData.Z_TalentPointsSpentShotgunWeapon;
        L_TalentPointsSpentRifleBullet = a_SaveData.Z_TalentPointsSpentRifleBullet;
        L_TalentPointsSpentShotgunBullet = a_SaveData.Z_TalentPointsSpentShotgunBullet;
    }
    ////////////////////////
    // SAVING AND LOADING //
    ////////////////////////
}
