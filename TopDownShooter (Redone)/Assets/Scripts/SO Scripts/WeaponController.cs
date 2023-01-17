using UnityEngine;


public class WeaponController : MonoBehaviour
{
    public WeaponDataSO WeaponData;


    // Start is called before the first frame update
    private void Start()
    {
        if (WeaponData != null)
        {
            LoadDefaultWeaponData(WeaponData);
        }
    }


    public void LoadDefaultWeaponData(WeaponDataSO data)
    {
        WeaponStatsManager.Instance.W_WeaponID = data.WeaponID;
        WeaponStatsManager.Instance.W_WeaponName = data.WeaponName;
        WeaponStatsManager.Instance.W_WeaponClipSize = data.WeaponClipSize;
        WeaponStatsManager.Instance.W_WeaponFireRate = data.WeaponFireRate;
        WeaponStatsManager.Instance.W_WeaponSpread = data.WeaponSpread;
        WeaponStatsManager.Instance.W_WeaponOverheatCooldown = data.WeaponOverheatCooldown;

        WeaponStatsManager.Instance.W_WeaponClipSizeMult = data.WeaponClipSizeMult;
        WeaponStatsManager.Instance.W_WeaponFireRateMult = data.WeaponFireRateMult;
        WeaponStatsManager.Instance.W_WeaponSpreadMult = data.WeaponSpreadMult;
        WeaponStatsManager.Instance.W_WeaponOverheatCooldownMult = data.WeaponOverheatCooldownMult;


        LoadUpdatedWeaponData(data, data.WeaponID);
    }


    // MAKE SURE TO CHANGE THE ARRAY VALUES BELOW IF CHANGE ORDER OF VARIABLES
    // After default values are loaded, multiply the default multipliers by
    // The saved Talent points spent list, then return that data back to
    // The Bullet stats manager script
    public void LoadUpdatedWeaponData(WeaponDataSO data, int id)
    {
        // Check if rifle
        if (id == 0)
        {
            WeaponStatsManager.Instance.W_WeaponClipSize += PlayerStatsManager.Instance.L_TalentPointsSpentRifleWeapon[0] * data.WeaponClipSizeMult;
            WeaponStatsManager.Instance.W_WeaponFireRate = data.WeaponFireRate * Mathf.Pow(data.WeaponFireRateMult, PlayerStatsManager.Instance.L_TalentPointsSpentRifleWeapon[1]);
            WeaponStatsManager.Instance.W_WeaponSpread += PlayerStatsManager.Instance.L_TalentPointsSpentRifleWeapon[2] * data.WeaponSpreadMult;
            WeaponStatsManager.Instance.W_WeaponOverheatCooldown = data.WeaponOverheatCooldown * Mathf.Pow(data.WeaponOverheatCooldownMult, PlayerStatsManager.Instance.L_TalentPointsSpentRifleWeapon[3]);
        }
        // Check if shotgun
        if (id == 1)
        {
            WeaponStatsManager.Instance.W_WeaponClipSize += PlayerStatsManager.Instance.L_TalentPointsSpentShotgunWeapon[0] * data.WeaponClipSizeMult;
            WeaponStatsManager.Instance.W_WeaponFireRate = data.WeaponFireRate * Mathf.Pow(data.WeaponFireRateMult, PlayerStatsManager.Instance.L_TalentPointsSpentShotgunWeapon[1]);
            WeaponStatsManager.Instance.W_WeaponSpread += PlayerStatsManager.Instance.L_TalentPointsSpentShotgunWeapon[2] * data.WeaponSpreadMult;
            WeaponStatsManager.Instance.W_WeaponOverheatCooldown = data.WeaponOverheatCooldown * Mathf.Pow(data.WeaponOverheatCooldownMult, PlayerStatsManager.Instance.L_TalentPointsSpentRifleWeapon[3]);
        }
    }
}
