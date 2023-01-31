using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


// Run at the start of each run until the end
// Count up score and then send the stats to the PlayerStatsManager
public class ScoreStatsManager : MonoBehaviour
{
    public static ScoreStatsManager Instance { get; private set; }

    // t_ stands for total
    [Header("Scores To Keep Note Of")]
    public int t_runScore = 0; // Total score achieved this run
    [SerializeField] private int t_killScore = 0; // Total score gained from kills
    public int t_timerScore = 0; // Total score gained from the timer
    [SerializeField] private int t_waveScore = 0; // Total Score gained from each wave completed


    [Header("Temporary Scores")]
    [SerializeField] private int _waveScore; // Score gained from each wave completed
    [SerializeField] private int _scorePerKill;
    [SerializeField] private int _timerScore;
    [SerializeField] private float _timer;

    [Header("ScoreBar Variables")]
    [SerializeField] private int _maximumMultiplier;
    [SerializeField] public int _currentMultiplier;
    [SerializeField] public int _currentScore;
    [SerializeField] public int _requiredScore;

    [Header("ScoreBar Multipliers")]
    [SerializeField][Range(0f, 1000f)] private float AdditionMult;
    [SerializeField][Range(2f, 16f)] private float PowerMult;
    [SerializeField][Range(7f, 28f)] private float DivisionMult;


    [System.Serializable]
    public class ScoreBar
    {
        public Image Background, SlowBar, Foreground;
        public TMP_Text MultiplierText;
    }


    public ScoreBar m_ScoreBar = new ScoreBar();


    [Header("References")]
    [SerializeField] private HUDScript _scriptHUD;
    [SerializeField] private float _duration; // the timer for the score bar lerp


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

        _currentMultiplier = 1;
        _requiredScore = CalculateRequiredScore();

        // Update the score bar using the hudscript
        _scriptHUD.UpdateScoreBar(m_ScoreBar, _duration);
    }


    // Set the player statsmanager scores to private variables
    private void GetBaseScoreStats()
    {
        _scorePerKill = PlayerStatsManager.Instance.S_ScorePerKill;
        _timerScore = PlayerStatsManager.Instance.S_TimerScore;
        _timer = PlayerStatsManager.Instance.S_Timer;
        _maximumMultiplier = PlayerStatsManager.Instance.S_MaximumMultiplier;
    }


    private int CalculateRequiredScore()
    {
        int solveForRequiredScore = 0;

        // Loop for everytime we have leveled up
        for (int multCycle = 1; multCycle <= _currentMultiplier; multCycle++)
        {
            // Runescape Algorithm
            solveForRequiredScore += (int)Mathf.Floor(multCycle + (AdditionMult * _scorePerKill) * Mathf.Pow(PowerMult, multCycle / DivisionMult));
        }

        return solveForRequiredScore * _currentMultiplier;
    }


    private void IncreaseMultiplier()
    {
        _currentMultiplier++;
        _currentScore -= _requiredScore;
        _requiredScore = CalculateRequiredScore();
    }


    // TODO: Add a score multiplier
    // When enemy dies, add to kill score
    public void AddScorePerKill(int enemyValue)
    {
        // Multiply enemy's score value by the players score per kill value
        var score = _scorePerKill * enemyValue * _currentMultiplier;
        t_killScore += score;
        UpdateTotalRunScore();
    }


    public void AddWaveScore(int waveValue)
    {
        // Add the wave value
        var score = waveValue * _currentMultiplier;
        t_waveScore += score;
        UpdateTotalRunScore();
    }


    // Will add this score to the final score when dead
    public void AddTimerscore()
    {
        t_timerScore = Mathf.RoundToInt(CareerStatsManager.Instance.t_CurrentTime) * _currentMultiplier;
        UpdateTotalRunScore();
    }


    // TODO: Add functions to add all the other scores which add up to t_runScore
    // Adds up all scores to the Total Run Score
    // Add the timer score after death
    public void UpdateTotalRunScore()
    {
        var tmpScore = t_runScore;
        t_runScore = t_killScore + t_waveScore + t_timerScore;

        UpdateTotalScores(t_runScore - tmpScore);
    }


    // Add the scores to the total score in Player Stats Manager
    private void UpdateTotalScores(int value)
    {
        PlayerStatsManager.Instance.S_TotalScore += value;

        // Use this 'value' to increase the score bar values
        _currentScore += value;
        while (_currentScore >= _requiredScore && _currentMultiplier < _maximumMultiplier)
        {
            IncreaseMultiplier();
        }

        // Update the score bar using the hudscript
        _scriptHUD.UpdateScoreBar(m_ScoreBar, _duration);
    }
}
