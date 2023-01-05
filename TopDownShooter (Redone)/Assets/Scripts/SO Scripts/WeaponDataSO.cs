using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "My Game/Weapon Data SO")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Weapon Base Stats")]
    public int WeaponID;
    public string WeaponName;
    public int WeaponClipSize;
    public float WeaponFireRate;
    public float WeaponSpread;
    public float WeaponOverheatCooldown;

    [Header("Weapon Base Stat Multipliers")]
    public int WeaponClipSizeMult;
    public float WeaponFireRateMult;
    public float WeaponSpreadMult;
    public float WeaponOverheatCooldownMult;

    [Header("Weapon Bools")]
    public bool WeaponIsOverheating;
}
