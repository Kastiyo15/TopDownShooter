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
        [Range(1000f, 10000f)] public float AdditionMult;
        [Range(2f, 16f)] public float PowerMult;
        [Range(7f, 28f)] public float DivisionMult;
        public XPBar BarXP;
    }


    [System.Serializable]
    public class XPBar
    {
        public Image Background, SlowBar, Foreground;
        public TMP_Text XPText, LevelText;
    }


    [Header("Player Level Data")]
    public LevelData m_Player = new LevelData();

    [Header("Weapon Level Data")]
    public LevelData m_Rifle = new LevelData();
    public LevelData m_Shotgun = new LevelData();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    private void Start()
    {
        m_Player.RequiredXp = CalculateRequiredXp(m_Player);
        StartCoroutine(UpdateXPBar(m_Player));

        m_Rifle.RequiredXp = CalculateRequiredXp(m_Rifle);
        StartCoroutine(UpdateXPBar(m_Rifle));

        m_Shotgun.RequiredXp = CalculateRequiredXp(m_Shotgun);
        StartCoroutine(UpdateXPBar(m_Shotgun));
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


    // Add experience flat rate
    public void GainExperience(LevelData data, float xpGained)
    {
        data.CurrentXp += xpGained;

        while (data.CurrentXp >= data.RequiredXp)
        {
            LevelUp(data);
        }

        StopCoroutine(UpdateXPBar(data));
        StartCoroutine(UpdateXPBar(data));
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
    public IEnumerator UpdateXPBar(LevelData data)
    {
        data.BarXP.LevelText.SetText($"{data.Level}");

        data.BarXP.Foreground.fillAmount = data.CurrentXp / data.RequiredXp;
        data.BarXP.XPText.SetText($"XP: {data.CurrentXp} / {data.RequiredXp}");

        float duration = 0.25f;
        if (data.BarXP.SlowBar.fillAmount != data.BarXP.Foreground.fillAmount)
        {
            for (float t = 0.0f; t < duration; t += Time.deltaTime)
            {
                data.BarXP.SlowBar.fillAmount = Mathf.Lerp(data.BarXP.SlowBar.fillAmount, data.BarXP.Foreground.fillAmount, t);
                yield return null;
            }
        }
        yield break;
    }


    public void HideWeaponBars(int id)
    {
        switch (id)
        {
            case (0):
                m_Shotgun.BarXP.SlowBar.gameObject.SetActive(false);
                m_Shotgun.BarXP.Foreground.gameObject.SetActive(false);
                m_Rifle.BarXP.SlowBar.gameObject.SetActive(true);
                m_Rifle.BarXP.Foreground.gameObject.SetActive(true);
                StartCoroutine(UpdateXPBar(m_Rifle));
                break;
            case (1):
                m_Rifle.BarXP.SlowBar.gameObject.SetActive(false);
                m_Rifle.BarXP.Foreground.gameObject.SetActive(false);
                m_Shotgun.BarXP.SlowBar.gameObject.SetActive(true);
                m_Shotgun.BarXP.Foreground.gameObject.SetActive(true);
                StartCoroutine(UpdateXPBar(m_Shotgun));
                break;
        }
    }
}
