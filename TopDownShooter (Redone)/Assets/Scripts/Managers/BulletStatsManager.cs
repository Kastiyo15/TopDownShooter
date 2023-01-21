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
    public float B_BulletKnockbackForce;


    [Header("Bullet Base Stat Multipliers")]
    public int B_BulletDamageMult;
    public float B_BulletVelocityMult;
    public float B_BulletAmountMult;
    public float B_BulletKnockbackForceMult;
    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

}