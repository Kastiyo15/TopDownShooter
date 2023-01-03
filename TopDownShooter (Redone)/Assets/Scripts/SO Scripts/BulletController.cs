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
            LoadBulletData(BulletData);
        }
    }


    public void LoadBulletData(BulletDataSO data)
    {
        BulletStatsManager.Instance.B_BulletID = BulletData.BulletID;
        BulletStatsManager.Instance.B_BulletName = BulletData.BulletName;
        BulletStatsManager.Instance.B_BulletPrefab = BulletData.BulletPrefab;
        BulletStatsManager.Instance.B_BulletDamage = BulletData.BulletDamage;
        BulletStatsManager.Instance.B_BulletVelocity = BulletData.BulletVelocity;
        BulletStatsManager.Instance.B_BulletAmount = BulletData.BulletAmount;

        BulletStatsManager.Instance.B_BulletDamageMult = BulletData.BulletDamageMult;
        BulletStatsManager.Instance.B_BulletVelocityMult = BulletData.BulletVelocityMult;
        BulletStatsManager.Instance.B_BulletAmountMult = BulletData.BulletAmountMult;
    }
}
