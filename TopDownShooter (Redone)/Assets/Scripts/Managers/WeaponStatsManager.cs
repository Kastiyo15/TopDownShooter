using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStatsManager : MonoBehaviour/* , ISaveable */
{

    public static WeaponStatsManager Instance { get; private set; }

    #region PLAYER WEAPON STATS
    [Header("Weapon Stats")]
    public int W_WeaponID;
    public string W_WeaponName;
    public int W_WeaponClipSize;
    public float W_WeaponFireRate;
    public float W_WeaponSpread;
    public float W_WeaponOverheatCooldown;

    [Header("Weapon Base Stat Multipliers")]
    public int W_WeaponClipSizeMult;
    public float W_WeaponFireRateMult;
    public float W_WeaponSpreadMult;
    public float W_WeaponOverheatCooldownMult;
    #endregion

    // Not related to load or save functions
    [Header("Current Clip Counters")]
    public int W_CurrentRifleClip;
    public int W_CurrentShotgunClip;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

}

