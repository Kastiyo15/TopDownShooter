using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DeathScreen : MonoBehaviour
{
    // Not all going to be used on page 1
    [System.Serializable]
    public class TextList
    {
        public TMP_Text Score;
        public TMP_Text WaveReached;
        public TMP_Text PlayTime;
        public TMP_Text Runs;
        public TMP_Text EnemiesKilled;
        public TMP_Text Deaths;
        public TMP_Text KDA;
        public TMP_Text BulletsFired;
        public TMP_Text BulletsHit;
        public TMP_Text Accuracy;
        public TMP_Text Damage;
    }

    [Header("Page 1")]
    public TextList Page1List = new TextList();
    [Header("Page 2")]
    public TextList Page2List = new TextList();

    [Header("Lerp Timer")]
    [SerializeField] private float _timer;

    [Header("References")]
    [SerializeField] private EnemyWaveSpawner _scriptWaveSpawner;
    [SerializeField] private PageType _type;


    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.IsDead)
        {
            UpdateStats();
        }
    }


    private void UpdateStats()
    {
        switch (_type)
        {
            case (PageType.RunStats):
                if (_timer < 15f) // stop the timer after 15 seconds
                {
                    _timer += Time.deltaTime;
                }

                // Lerp stats after one second
                if (_timer > 1f)
                {
                    LerpRunStats(Page1List.WaveReached, _scriptWaveSpawner.WaveNumber, 2f);
                    LerpRunStats(Page1List.Score, ScoreStatsManager.Instance.t_runScore, 2.5f);
                    LerpRunTimer(Page1List.PlayTime, CareerStatsManager.Instance.t_CurrentTime, 2.5f);
                    LerpRunStats(Page1List.EnemiesKilled, CareerStatsManager.Instance.t_EnemiesKilled, 3f);
                    LerpRunStats(Page1List.BulletsFired, CareerStatsManager.Instance.t_BulletsFired, 3.5f);
                    LerpRunStats(Page1List.Accuracy, CareerStatsManager.Instance.t_BulletAccuracy, 3.5f);
                    LerpRunStats(Page1List.Damage, CareerStatsManager.Instance.t_TotalDamage, 3.5f);
                }
                break;

            case (PageType.CareerStats):
                if (_timer < 15f) // stop the timer after 15 seconds
                {
                    _timer += Time.deltaTime;
                }

                // Lerp stats after one second
                if (_timer > 1f)
                {
                    LerpRunStats(Page2List.Score, PlayerStatsManager.Instance.S_TotalScore, 2f);
                    LerpRunStats(Page2List.WaveReached, PlayerStatsManager.Instance.C_TotalWavesCompleted, 2.5f);
                    LerpRunTimer(Page2List.PlayTime, PlayerStatsManager.Instance.C_TotalPlayTime, 2.5f);
                    LerpRunStats(Page2List.Runs, PlayerStatsManager.Instance.C_TotalRuns, 2.5f);
                    LerpRunStats(Page2List.EnemiesKilled, PlayerStatsManager.Instance.C_TotalEnemiesKilled, 3f);
                    LerpRunStats(Page2List.Deaths, PlayerStatsManager.Instance.C_TotalDeaths, 3f);
                    LerpRunStats(Page2List.KDA, PlayerStatsManager.Instance.C_TotalKDA, 3f);
                    LerpRunStats(Page2List.BulletsFired, PlayerStatsManager.Instance.C_TotalBulletsFired, 3.5f);
                    LerpRunStats(Page2List.BulletsHit, PlayerStatsManager.Instance.C_TotalBulletsHit, 3.5f);
                    LerpRunStats(Page2List.Accuracy, PlayerStatsManager.Instance.C_TotalAccuracy, 3.5f);
                    LerpRunStats(Page2List.Damage, PlayerStatsManager.Instance.C_TotalDamage, 3.5f);
                }
                break;
        }
    }

    // Send the text, then lerp the appropriate stat from 0
    private void LerpRunStats(TMP_Text txt, float stat, float addedTime)
    {
        float tempStat = 0f;
        tempStat = Mathf.Lerp(tempStat, stat, _timer / addedTime);
        txt.SetText($"{tempStat:#0}");

        // Add a percent sign
        if (stat == CareerStatsManager.Instance.t_BulletAccuracy || stat == PlayerStatsManager.Instance.C_TotalAccuracy)
        {
            txt.SetText($"{tempStat:#0.00}%");
        }

        // Add a percent sign
        if (stat == PlayerStatsManager.Instance.C_TotalKDA)
        {
            txt.SetText($"{tempStat:#0.00}");
        }
    }


    private void LerpRunTimer(TMP_Text txt, float stat, float addedTime)
    {
        float tmpTimer = 0f;
        if (tmpTimer < stat)
        {
            tmpTimer = Mathf.Lerp(tmpTimer, stat, _timer / addedTime);
        }
        TimeSpan runTime = TimeSpan.FromSeconds(tmpTimer);
        txt.text = runTime.ToString(@"mm\:ss\:ff");
    }


    public enum PageType
    {
        RunStats,
        CareerStats,
    }
}
