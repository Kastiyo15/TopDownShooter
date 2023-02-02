using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [System.Serializable]
    public class LevelData
    {
        public int Level = 1;
        public float CurrentXp;
        public float RequiredXp;
        public int TalentPoints;
        public int TalentPointsSpent;
        public int TalentPointsAvailable;
        public int ResetPoints;
        [Range(100f, 2000f)] public int AdditionMult;
        [Range(2f, 16f)] public int PowerMult;
        [Range(7f, 28f)] public int DivisionMult;
    }

    [System.Serializable]
    public class XPBar
    {
        public Image Background, SlowBar, Foreground;
        public TMP_Text XPText, LevelText;
    }

    [Header("Player Level Data")]
    public LevelData m_Player = new LevelData();
    public XPBar m_PlayerBar = new XPBar();

    [Header("Weapon Level Data")]
    public LevelData m_Rifle = new LevelData();
    public XPBar m_RifleBar = new XPBar();

    public LevelData m_Shotgun = new LevelData();
    public XPBar m_ShotgunBar = new XPBar();



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
        UpdateXPBar(m_Player, m_PlayerBar);
        UpdateXPBar(m_Rifle, m_RifleBar);
        UpdateXPBar(m_Shotgun, m_ShotgunBar);
    }



    public void StartLevelManager()
    {
        // Set the data sets to the saved data sets
        m_Player = PlayerStatsManager.Instance.L_PlayerLevelData;

        // check if this is a new player
        if (m_Player.RequiredXp == 0)
        {
            m_Player.RequiredXp = CalculateRequiredXp(m_Player);
        }

        // Run this everytime we start a run
        m_Rifle.RequiredXp = CalculateRequiredXp(m_Rifle);
        m_Shotgun.RequiredXp = CalculateRequiredXp(m_Shotgun);


        UpdateXPBar(m_Player, m_PlayerBar);
        UpdateXPBar(m_Rifle, m_RifleBar);
        UpdateXPBar(m_Shotgun, m_ShotgunBar);


        // Update the ui bars and text
        HideWeaponBars(0);
    }


    private int CalculateRequiredXp(LevelData data)
    {
        int solveForRequiredXp = 0;

        // Loop for everytime we have leveled up
        for (int levelCycle = 1; levelCycle <= data.Level; levelCycle++)
        {
            // Runescape Algorithm
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + data.AdditionMult * Mathf.Pow(data.PowerMult, levelCycle / data.DivisionMult));
        }

        return solveForRequiredXp / 4;
    }


    // Add reset points every 5 levels
    public void AddResetPoint(LevelData data)
    {
        data.ResetPoints++;
    }


    // Add experience flat rate
    public void GainExperience(LevelData data, float xpGained)
    {
        data.CurrentXp += xpGained;

        while (data.CurrentXp >= data.RequiredXp)
        {
            LevelUp(data);
        }

    }


    // Increase level and add talent points, and calculate xp for next level
    private void LevelUp(LevelData data)
    {
        data.Level++;
        data.TalentPoints++;
        data.CurrentXp -= data.RequiredXp;
        data.RequiredXp = CalculateRequiredXp(data);
    }


    // Set the fill amount to current xp value
    public void UpdateXPBar(LevelData data, XPBar barXP)
    {
        barXP.LevelText.SetText($"{data.Level}");
        barXP.Foreground.fillAmount = data.CurrentXp / data.RequiredXp;

        float duration = 0.2f;

        if (barXP.SlowBar.fillAmount != barXP.Foreground.fillAmount)
        {
            barXP.SlowBar.fillAmount = Mathf.Lerp(barXP.SlowBar.fillAmount, barXP.Foreground.fillAmount, Mathf.Pow(duration, 2f));
        }
    }

    public void HideWeaponBars(int id)
    {
        switch (id)
        {
            case (0):

                HideXPBar(m_ShotgunBar);
                ShowXPBar(m_RifleBar);

                break;
            case (1):

                HideXPBar(m_RifleBar);
                ShowXPBar(m_ShotgunBar);
                
                break;
        }
    }


    private void HideXPBar(XPBar barXP)
    {
        barXP.SlowBar.gameObject.SetActive(false);
        barXP.Foreground.gameObject.SetActive(false);
        barXP.LevelText.gameObject.SetActive(false);
    }


    private void ShowXPBar(XPBar barXP)
    {
        barXP.SlowBar.gameObject.SetActive(true);
        barXP.Foreground.gameObject.SetActive(true);
        barXP.LevelText.gameObject.SetActive(true);
    }


    // Called in the gameManager, when player dies
    public void UpdateSavedLevelData()
    {
        PlayerStatsManager.Instance.L_PlayerLevelData = m_Player;
    }
}
