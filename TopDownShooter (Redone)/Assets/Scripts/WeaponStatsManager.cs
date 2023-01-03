using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStatsManager : MonoBehaviour, ISaveable
{

    public static WeaponStatsManager Instance { get; private set; }

    #region PLAYER WEAPON STATS
    [Header("Weapon Stats")]
    public int W_WeaponID;
    public string W_WeaponName;
    public int W_WeaponClipSize;
    public float W_WeaponFireRate;
    public float W_WeaponSpread;

    [Header("Weapon Base Stat Multipliers")]
    public int W_WeaponClipSizeMult;
    public float W_WeaponFireRateMult;
    public float W_WeaponSpreadMult;
    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



    ////////////////////////
    // SAVING AND LOADING //
    ////////////////////////
    public void PopulateSaveData(SaveData a_SaveData)
    {
        // Weapon stats
        a_SaveData.Z_WeaponID = W_WeaponID;
        a_SaveData.Z_WeaponName = W_WeaponName;
        a_SaveData.Z_WeaponClipSize = W_WeaponClipSize;
        a_SaveData.Z_WeaponFireRate = W_WeaponFireRate;
        a_SaveData.Z_WeaponSpread = W_WeaponSpread;

        // Weapon Multiplier
        a_SaveData.Z_WeaponClipSizeMult = W_WeaponClipSizeMult;
        a_SaveData.Z_WeaponFireRateMult = W_WeaponFireRateMult;
        a_SaveData.Z_WeaponSpreadMult = W_WeaponSpreadMult;
    }


    public void LoadFromSaveData(SaveData a_SaveData)
    {
        // Weapon stats
        W_WeaponID = a_SaveData.Z_WeaponID;
        W_WeaponName = a_SaveData.Z_WeaponName;
        W_WeaponClipSize = a_SaveData.Z_WeaponClipSize;
        W_WeaponFireRate = a_SaveData.Z_WeaponFireRate;
        W_WeaponSpread = a_SaveData.Z_WeaponSpread;

        // Weapon Multiplier
        W_WeaponClipSizeMult = a_SaveData.Z_WeaponClipSizeMult;
        W_WeaponFireRateMult = a_SaveData.Z_WeaponFireRateMult;
        W_WeaponSpreadMult = a_SaveData.Z_WeaponSpreadMult;
    }
    ////////////////////////
    // SAVING AND LOADING //
    ////////////////////////
}

