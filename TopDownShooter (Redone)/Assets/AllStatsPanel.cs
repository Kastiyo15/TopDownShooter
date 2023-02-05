using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AllStatsPanel : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _healthRegenValue;
    [SerializeField] private TMP_Text _healthRegenRate;
    [SerializeField] private TMP_Text _defence;
    [SerializeField] private TMP_Text _blockChance;
    [SerializeField] private TMP_Text _defenceMult;
    [SerializeField] private TMP_Text _agility;
    [SerializeField] private TMP_Text _dashDistance;
    [SerializeField] private TMP_Text _dashTimer;
    [SerializeField] private TMP_Text _scorePerKill;
    [SerializeField] private TMP_Text _aliveScore;
    [SerializeField] private TMP_Text _scoreMaxMult;


    private void Start()
    {
        UpdateAllStats();
    }


    private void UpdateAllStats()
    {
        UpdateStatValue(_health, PlayerStatsManager.Instance.G_PlayerHealth);
        UpdateStatValue(_healthRegenValue, PlayerStatsManager.Instance.H_PlayerRegenerationValue);
        UpdateStatValue(_healthRegenRate, PlayerStatsManager.Instance.H_PlayerRegenerationRate);
        UpdateStatValue(_defence, PlayerStatsManager.Instance.G_PlayerDefence);
        UpdateStatValue(_blockChance, PlayerStatsManager.Instance.D_PlayerBlockChance);
        UpdateStatValue(_defenceMult, PlayerStatsManager.Instance.D_PlayerDefenceMultiplier);
        UpdateStatValue(_agility, PlayerStatsManager.Instance.G_PlayerAgility);
        UpdateStatValue(_dashDistance, PlayerStatsManager.Instance.A_PlayerDashDistance);
        UpdateStatValue(_dashTimer, PlayerStatsManager.Instance.A_PlayerDashTimer);
        UpdateStatValue(_scorePerKill, PlayerStatsManager.Instance.S_ScorePerKill);
        UpdateStatValue(_aliveScore, PlayerStatsManager.Instance.S_TimerScore);
        UpdateStatValue(_scoreMaxMult, PlayerStatsManager.Instance.S_MaximumMultiplier);
    }


    private void UpdateStatValue(TMP_Text text, float value)
    {
        if (value != 0)
        {
            text.SetText($"{value:#0}");
        }
        else
        {
            text.SetText($"-");
        }
    }
}
