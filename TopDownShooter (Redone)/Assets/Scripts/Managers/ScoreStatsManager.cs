using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Run at the start of each run until the end
// Count up score and then send the stats to the PlayerStatsManager
public class ScoreStatsManager : MonoBehaviour
{
    public static ScoreStatsManager Instance { get; private set; }

    // t_ stands for total
    [Header("Scores To Keep Note Of")]
    public int t_runScore = 0; // Total score achieved this run
    [SerializeField] private int t_killScore = 0; // Total score gained from kills
    [SerializeField] private int t_timerScore = 0; // Total score gained from the timer
    [SerializeField] private int t_waveScore = 0; // Total Score gained from each wave completed


    [Header("Temporary Scores")]
    [SerializeField] private int _waveScore; // Score gained from each wave completed
    [SerializeField] private int _scorePerKill;
    [SerializeField] private int _timerScore;
    [SerializeField] private float _timer;
    [SerializeField] private int _maximumMultiplier;


    [Header("References")]
    [SerializeField] private HUDScript s_HUDScript;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        GetBaseScoreStats();
    }


    // Set the player statsmanager scores to private variables
    private void GetBaseScoreStats()
    {
        _scorePerKill = PlayerStatsManager.Instance.S_ScorePerKill;
        _timerScore = PlayerStatsManager.Instance.S_TimerScore;
        _timer = PlayerStatsManager.Instance.S_Timer;
        _maximumMultiplier = PlayerStatsManager.Instance.S_MaximumMultiplier;
    }


    // TODO: Add a score multiplier
    // When enemy dies, add to kill score
    public void AddScorePerKill(int enemyValue)
    {
        // Multiply enemy's score value by the players score per kill value
        t_killScore += _scorePerKill * enemyValue;

        // update the total run score
        UpdateTotalRunScore();
    }


    public void AddWaveScore(int waveValue)
    {
        // Add the wave value
        t_waveScore += waveValue;

        // update the total run score
        UpdateTotalRunScore();
    }


    // TODO: Add functions to add all the other scores which add up to t_runScore
    // Adds up all scores to the Total Run Score
    private void UpdateTotalRunScore()
    {
        var currentScore = t_runScore;
        t_runScore = t_killScore + t_timerScore + t_waveScore;
        StartCoroutine(s_HUDScript.UpdateScoreHUD(currentScore));
    }


    // Add the scores to the total score in Player Stats Manager
    private void UpdateTotalScores()
    {
        PlayerStatsManager.Instance.S_TotalScore += t_runScore;
    }
}
