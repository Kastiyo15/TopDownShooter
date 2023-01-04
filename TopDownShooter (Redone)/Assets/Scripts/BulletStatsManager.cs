using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletStatsManager : MonoBehaviour/* , ISaveable */
{

    public static BulletStatsManager Instance { get; private set; }

    #region PLAYER BULLET STATS
    [Header("Bullet Base Stat Multipliers")]
    public int B_BulletID;
    public string B_BulletName;
    public GameObject B_BulletPrefab;
    public int B_BulletDamage;
    public float B_BulletVelocity;
    public int B_BulletAmount;

    [Header("Bullet Base Stat Multipliers")]
    public int B_BulletDamageMult;
    public float B_BulletVelocityMult;
    public float B_BulletAmountMult;
    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



/*     ////////////////////////
    // SAVING AND LOADING //
    ////////////////////////
    public void PopulateSaveData(SaveData a_SaveData)
    {
        // Bullet stats
        a_SaveData.Z_BulletID = B_BulletID;
        a_SaveData.Z_BulletName = B_BulletName;
        a_SaveData.Z_BulletPrefab = B_BulletPrefab;
        a_SaveData.Z_BulletDamage = B_BulletDamage;
        a_SaveData.Z_BulletVelocity = B_BulletVelocity;
        a_SaveData.Z_BulletAmount = B_BulletAmount;

        // Bullet Multiplier
        a_SaveData.Z_BulletDamageMult = B_BulletDamageMult;
        a_SaveData.Z_BulletVelocityMult = B_BulletVelocityMult;
        a_SaveData.Z_BulletAmountMult = B_BulletAmountMult;
    }


    public void LoadFromSaveData(SaveData a_SaveData)
    {
        // Bullet stats
        B_BulletID = a_SaveData.Z_BulletID;
        B_BulletName = a_SaveData.Z_BulletName;
        B_BulletPrefab = a_SaveData.Z_BulletPrefab;
        B_BulletDamage = a_SaveData.Z_BulletDamage;
        B_BulletVelocity = a_SaveData.Z_BulletVelocity;
        B_BulletAmount = a_SaveData.Z_BulletAmount;

        // Bullet Multiplier
        B_BulletDamageMult = a_SaveData.Z_BulletDamageMult;
        B_BulletVelocityMult = a_SaveData.Z_BulletVelocityMult;
        B_BulletAmountMult = a_SaveData.Z_BulletAmountMult;
    }
    ////////////////////////
    // SAVING AND LOADING //
    //////////////////////// */
}

