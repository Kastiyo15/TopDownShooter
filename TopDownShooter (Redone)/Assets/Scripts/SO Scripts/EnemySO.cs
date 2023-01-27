using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "My Game/Enemy SO")]
public class EnemySO : ScriptableObject
{
    [Header("Enemy References")]
    public int EnemyID;
    public string EnemyName;
    public GameObject EnemyPrefab;
}
