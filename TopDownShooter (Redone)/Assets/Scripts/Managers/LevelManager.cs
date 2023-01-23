using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        m_Rifle.RequiredXp = CalculateRequiredXp(m_Rifle);
        m_Shotgun.RequiredXp = CalculateRequiredXp(m_Shotgun);
    }


    // Update is called once per frame
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GainExperience(m_Player, Random.Range(30, 100));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GainExperience(m_Rifle, Random.Range(30, 100));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GainExperience(m_Shotgun, Random.Range(30, 100));
        }
    }


    // Add experience flat rate
    public void GainExperience(LevelData data, float xpGained)
    {
        Debug.Log("Exp Gained");
        data.CurrentXp += xpGained;

        while (data.CurrentXp >= data.RequiredXp)
        {
            LevelUp(data);
        }
    }


    private void LevelUp(LevelData data)
    {
        data.Level++;
        data.TalentPoints++;
        data.CurrentXp -= data.RequiredXp;
        data.RequiredXp = CalculateRequiredXp(data);
    }
}
