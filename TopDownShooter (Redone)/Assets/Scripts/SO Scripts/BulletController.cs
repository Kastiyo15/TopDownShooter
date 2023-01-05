using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    public BulletDataSO BulletData;


    // Start is called before the first frame update
    void Start()
    {
        if (BulletData != null)
        {
            LoadDefaultBulletData(BulletData);
        }
    }


    // This will load the scriptable objects default values into this Stats manager
    public void LoadDefaultBulletData(BulletDataSO data)
    {
        BulletStatsManager.Instance.B_BulletID = data.BulletID;
        BulletStatsManager.Instance.B_BulletName = data.BulletName;
        BulletStatsManager.Instance.B_BulletPrefab = data.BulletPrefab;
        BulletStatsManager.Instance.B_BulletDamage = data.BulletDamage;
        BulletStatsManager.Instance.B_BulletVelocity = data.BulletVelocity;
        BulletStatsManager.Instance.B_BulletAmount = data.BulletAmount;

        BulletStatsManager.Instance.B_BulletDamageMult = data.BulletDamageMult;
        BulletStatsManager.Instance.B_BulletVelocityMult = data.BulletVelocityMult;
        BulletStatsManager.Instance.B_BulletAmountMult = data.BulletAmountMult;

        LoadUpdatedBulletData(data, data.BulletID);
    }


    // MAKE SURE TO CHANGE THE ARRAY VALUES BELOW IF CHANGE ORDER OF VARIABLES
    // After default values are loaded, multiply the default multipliers by
    // The saved Talent points spent list, then return that data back to
    // The Bullet stats manager script
    public void LoadUpdatedBulletData(BulletDataSO data, int id)
    {
        // Check if rifle
        if (id == 0)
        {
            BulletStatsManager.Instance.B_BulletDamage += PlayerStatsManager.Instance.L_TalentPointsSpentRifleBullet[0] * data.BulletDamageMult;
            BulletStatsManager.Instance.B_BulletVelocity += PlayerStatsManager.Instance.L_TalentPointsSpentRifleBullet[1] * data.BulletVelocityMult;
            BulletStatsManager.Instance.B_BulletAmount += PlayerStatsManager.Instance.L_TalentPointsSpentRifleBullet[2] * data.BulletAmountMult;
        }
        // Check if shotgun
        if (id == 1)
        {
            BulletStatsManager.Instance.B_BulletDamage += PlayerStatsManager.Instance.L_TalentPointsSpentShotgunBullet[0] * data.BulletDamageMult;
            BulletStatsManager.Instance.B_BulletVelocity += PlayerStatsManager.Instance.L_TalentPointsSpentShotgunBullet[1] * data.BulletVelocityMult;
            BulletStatsManager.Instance.B_BulletAmount += PlayerStatsManager.Instance.L_TalentPointsSpentShotgunBullet[2] * data.BulletAmountMult;
        }
    }
}
