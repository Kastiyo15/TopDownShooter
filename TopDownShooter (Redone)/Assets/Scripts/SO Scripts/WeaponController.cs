using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    public WeaponDataSO WeaponData;


    // Start is called before the first frame update
    private void Start()
    {
        if (WeaponData != null)
        {
            LoadWeaponData(WeaponData);
        }
    }


    public void LoadWeaponData(WeaponDataSO data)
    {
        WeaponStatsManager.Instance.W_WeaponID = WeaponData.WeaponID;
        WeaponStatsManager.Instance.W_WeaponName = WeaponData.WeaponName;
        WeaponStatsManager.Instance.W_WeaponClipSize = WeaponData.WeaponClipSize;
        WeaponStatsManager.Instance.W_WeaponFireRate = WeaponData.WeaponFireRate;
        WeaponStatsManager.Instance.W_WeaponSpread = WeaponData.WeaponSpread;

        WeaponStatsManager.Instance.W_WeaponClipSizeMult = WeaponData.WeaponClipSizeMult;
        WeaponStatsManager.Instance.W_WeaponFireRateMult = WeaponData.WeaponFireRateMult;
        WeaponStatsManager.Instance.W_WeaponSpreadMult = WeaponData.WeaponSpreadMult;
    }
}
