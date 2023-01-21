using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CareerStatsManager : MonoBehaviour
{
    public static CareerStatsManager Instance { get; private set; }

    [Header("Career Stats")]
    public float t_PlayTime = 0;
    public int t_Runs = 0;

    [Header("Shooting Stats")]
    public int t_EnemiesKilled = 0;
    public int t_Deaths = 0;
    public float t_KDA = 0;
    public int t_BulletsFired = 0;
    public int t_BulletsHit = 0;
    public float t_Accuracy = 0; // bullets hit / fired / 100


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void UpdateEnemiesKilled()
    {
        t_EnemiesKilled++;
    }


    private void UpdateAllCareerStats()
    {
        // Career Stats
        PlayerStatsManager.Instance.C_TotalPlayTime += t_PlayTime;
        PlayerStatsManager.Instance.C_TotalRuns += t_Runs;

        // Shooting Stats
        PlayerStatsManager.Instance.C_TotalEnemiesKilled += t_EnemiesKilled;
        PlayerStatsManager.Instance.C_TotalDeaths += t_Deaths;
        PlayerStatsManager.Instance.C_TotalKDA += t_KDA;
        PlayerStatsManager.Instance.C_TotalBulletsFired += t_BulletsFired;
        PlayerStatsManager.Instance.C_TotalBulletsHit += t_BulletsHit;
        PlayerStatsManager.Instance.C_TotalAccuracy += t_Accuracy;
    }
}
