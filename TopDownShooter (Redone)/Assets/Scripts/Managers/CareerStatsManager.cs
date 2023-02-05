using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CareerStatsManager : MonoBehaviour
{
    public static CareerStatsManager Instance { get; private set; }

    [Header("Career Stats")]
    public float t_CurrentTime;
    public TimeSpan t_PlayTime;

    [Header("Shooting Stats")]
    public int t_EnemiesKilled = 0;
    public int t_BulletsFired = 0;
    public int t_BulletsHit = 0;
    public float t_BulletAccuracy = 0;
    public int t_TotalDamage = 0;


    [SerializeField] private TMP_Text _txtStopwatch;


    public enum VariableType
    {
        WavesCompleted,
        PlayTime,
        Runs,
        EnemiesKilled,
        PlayerDeaths,
        BulletsFired,
        BulletsHit,
        TotalDamage,
    }


    public VariableType VariableSelector;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void UpdateCareerStatsVariable(VariableType variable)
    {
        switch (variable)
        {
            case (VariableType.WavesCompleted):
                PlayerStatsManager.Instance.C_TotalWavesCompleted++;
                break;

            case (VariableType.PlayTime):
                break;

            case (VariableType.Runs):
                PlayerStatsManager.Instance.C_TotalRuns++;
                break;

            case (VariableType.EnemiesKilled):
                t_EnemiesKilled++;
                PlayerStatsManager.Instance.C_TotalEnemiesKilled++;

                UpdateKDA();
                break;

            case (VariableType.PlayerDeaths):
                PlayerStatsManager.Instance.C_TotalDeaths++;

                UpdateKDA();
                break;

            case (VariableType.BulletsFired):
                t_BulletsFired++;
                PlayerStatsManager.Instance.C_TotalBulletsFired++;

                UpdateBulletAccuracy();
                break;

            case (VariableType.BulletsHit):
                t_BulletsHit++;
                PlayerStatsManager.Instance.C_TotalBulletsHit++;
                break;
        }
    }


    public void UpdateKDA()
    {
        // Update Saved Total Stats
        if (PlayerStatsManager.Instance.C_TotalDeaths > 1)
        {
            PlayerStatsManager.Instance.C_TotalKDA = (float)PlayerStatsManager.Instance.C_TotalEnemiesKilled / (float)PlayerStatsManager.Instance.C_TotalDeaths;
        }
        else
        {
            PlayerStatsManager.Instance.C_TotalKDA = PlayerStatsManager.Instance.C_TotalEnemiesKilled;
        }
    }


    public void UpdateBulletAccuracy()
    {
        // Update This Run Variables
        t_BulletAccuracy = ((float)t_BulletsHit / (float)t_BulletsFired) * 100f;

        // Update Saved Total Stats
        PlayerStatsManager.Instance.C_TotalAccuracy = ((float)PlayerStatsManager.Instance.C_TotalBulletsHit / (float)PlayerStatsManager.Instance.C_TotalBulletsFired) * 100f;
    }


    public void UpdateTotalDamage(int amount)
    {
        // Update This Run Variables
        t_TotalDamage += amount;

        // Update Saved Total Stats
        PlayerStatsManager.Instance.C_TotalDamage += amount;
    }


    public void UpdateStopWatch()
    {
        // Update This Run Variables
        float time = 0f;
        time += Time.deltaTime;
        t_CurrentTime += time;
        t_PlayTime = TimeSpan.FromSeconds(t_CurrentTime);
        _txtStopwatch.text = t_PlayTime.ToString(@"mm\:ss\:ff");

        // Update Saved Total Stats
        PlayerStatsManager.Instance.C_TotalPlayTime += time;
    }
}
