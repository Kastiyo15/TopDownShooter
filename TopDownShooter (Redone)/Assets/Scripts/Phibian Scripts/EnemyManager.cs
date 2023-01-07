using System.Collections;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private BoxArea _boxArea;
    [SerializeField] private Spawner _spawnerPrefab;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private float _waveDelay;
    private int _waveNum;


    // Start is called before the first frame update
    void Start()
    {
        _boxArea.GetRandomPoint();
        StartCoroutine(WaveSpawning());
    }


    private IEnumerator WaveSpawning()
    {
        var spawnDelay = GetSpawnDelay();
        int waveSpawnCount = GetSpawnCount();
        var nextSpawnPopulation = _enemyPool.GetEntriesFor(waveSpawnCount, _waveNum);

        foreach (var spawn in nextSpawnPopulation)
        {
            yield return new WaitForSeconds(spawnDelay);
            var nextSpawnPos = _boxArea.GetRandomPoint();
            var newSpawner = Instantiate(_spawnerPrefab, nextSpawnPos, Quaternion.identity);
            newSpawner.Parent = transform;
            newSpawner.SpawnPrefab = spawn;
        }
        yield return new WaitForSeconds(_waveDelay);
        _waveNum++;
        StartCoroutine(WaveSpawning());
    }


    private float GetSpawnDelay()
    {
        var startingDelay = 2f;
        var finalDelay = 0.25f;
        var maxWave = 100;
        return (_waveNum >= 0 && _waveNum <= maxWave) ? (1f / (Mathf.Pow(maxWave, 2)) * (startingDelay - finalDelay) * Mathf.Pow(_waveNum - maxWave, 2)) : finalDelay;
    }


    private int GetSpawnCount()
    {
        var startingCount = 4;
        var finalCount = 32;
        var maxWave = 75;
        return (_waveNum >= 0 && _waveNum <= maxWave) ? Mathf.RoundToInt(1f / (Mathf.Pow(maxWave, 2)) * (finalCount - startingCount) * Mathf.Pow(_waveNum, 2)) + startingCount : finalCount;
    }
}
