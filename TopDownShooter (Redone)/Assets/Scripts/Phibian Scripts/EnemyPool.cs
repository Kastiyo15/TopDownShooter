using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new_enemy_pool", menuName = "My Game/Enemy Pool")]
public class EnemyPool : ScriptableObject
{
    //public Dictionary<int, EnemyPoolSpawn> spawnPoolEntries = new Dictionary<int, EnemyPoolSpawn>();
    [SerializeField] public List<EnemyPoolSpawn> spawnPoolEntries = new List<EnemyPoolSpawn>();

    public List<Enemy> GetEntriesFor(int entryCount, int _waveNum)
    {
        List<Enemy> entries = new List<Enemy>();

        int totalWeight = 0;
        for (int i = 0; i <= _waveNum; i++)
        {
            if (spawnPoolEntries[i].EnemyPrefab[i] != null && spawnPoolEntries[i].Weight[i] != 0)
            {
                totalWeight += spawnPoolEntries[i].Weight[i];
            }
        }

        for (int j = 0; j < entryCount; j++)
        {
            var randWeight = Random.Range(0, totalWeight);
            int weightRun = 0;
            for (int i = 0; i < entryCount; i++)
            {
                if (spawnPoolEntries[i].EnemyPrefab != null && spawnPoolEntries[i].Weight[i] != 0)
                {
                    weightRun += spawnPoolEntries[i].Weight[i];
                    if (weightRun > randWeight)
                    {
                        entries.Add(spawnPoolEntries[i].EnemyPrefab[i]);
                        break;
                    }
                }
            }
        }

        return entries;
    }

    [System.Serializable]
    public class EnemyPoolSpawn
    {
        [SerializeField] public List<Enemy> EnemyPrefab = new List<Enemy>();
        [SerializeField] public List<int> Weight = new List<int>();
    }
}