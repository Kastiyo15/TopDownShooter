using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "My Game/Bullet Data SO")]
public class BulletDataSO : ScriptableObject
{
    [Header("Bullet Base Stat Multipliers")]
    public int BulletID;
    public string BulletName;
    public GameObject BulletPrefab;
    public int BulletDamage;
    public float BulletVelocity;
    public int BulletAmount;
    public float BulletKnockbackForce;

    [Header("Bullet Base Stat Multipliers")]
    public int BulletDamageMult;
    public float BulletVelocityMult;
    public int BulletAmountMult;
    public float BulletKnockbackForceMult;
}
