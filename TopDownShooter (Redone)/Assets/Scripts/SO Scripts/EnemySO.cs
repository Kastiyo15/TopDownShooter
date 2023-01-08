using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "My Game/Enemy SO")]
public class EnemySO : ScriptableObject
{
    [Header("Enemy References")]
    public int EnemyID;
    public string EnemyName;
    public GameObject EnemyPrefab;

    [Header("Enemy Base Stats")]
    public int EnemyHealth; // Can replace these with HEA, DEF, AGI later
    public float EnemyMoveSpeed;
    public int EnemyAttackPower; // how much damage it does
    public int EnemyScoreValue; // how many points for killing it
}
