using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HUDScript _scriptHUD;
    [SerializeField] private Camera _cam;
    [SerializeField] private BoxArea _boxArea;
    [SerializeField] private EnemySO[] _enemyScriptableObject;

    [Header("Debug Purposes")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private int waveSpawnCount;
    [SerializeField] private int _arrayLength;

    [Header("Keep Track Of")]
    [SerializeField] private bool _waveComplete = true;
    [SerializeField] private float _waveDelay;
    [SerializeField] public int EnemiesRemaining = 0;
    public int WaveNumber;


    // Start is called before the first frame update
    void Start()
    {
        _boxArea.GetRandomPoint();

        StartCoroutine(WaveSpawning());
        StartCoroutine(GetEnemyCount());

        _arrayLength = _enemyScriptableObject.Length;
    }


    private IEnumerator GetEnemyCount()
    {
        while (true)
        {
            EnemiesRemaining = ObjectPool.Instance.GetActiveEnemyAgents();

            if (EnemiesRemaining == 0)
            {
                _waveComplete = true;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }


    private IEnumerator WaveSpawning()
    {
        while (!_waveComplete)
        {
            yield return null;
        }

        if (WaveNumber <= 100)
        {
            spawnDelay = GetSpawnDelay();
            waveSpawnCount = GetSpawnCount();
            _scriptHUD.UpdateWaveHUD(WaveNumber);

            yield return new WaitForSeconds(spawnDelay);

            // Add enemy SO into the object pool, removing the ones from previous wave
            ObjectPool.Instance.AddEnemyAgentsToPool(waveSpawnCount, _arrayLength, _enemyScriptableObject);


            for (int i = 0; i < waveSpawnCount; i++)
            {
                var nextSpawnPos = _boxArea.GetRandomPoint();

                //yield return new WaitForSeconds(_waveDelay / waveSpawnCount);
                yield return new WaitForSeconds(spawnDelay);

                var newSpawn = ObjectPool.Instance.GetEnemyAgentsFromPool(waveSpawnCount);
                newSpawn.SetActive(true);

                newSpawn.transform.position = ((Vector2)_cam.transform.position + nextSpawnPos);
            }

            _waveComplete = false;

            WaveNumber++;

            StartCoroutine(WaveSpawning());
        }
    }


    private float GetSpawnDelay()
    {
        var startingDelay = 3f;
        var finalDelay = 0.5f;
        var maxWave = 100;
        return (WaveNumber >= 0 && WaveNumber <= maxWave) ? (1f / (Mathf.Pow(maxWave, 2)) * (startingDelay - finalDelay) * Mathf.Pow(WaveNumber - maxWave, 2)) : finalDelay;
    }


    private int GetSpawnCount()
    {
        var startingCount = 10;
        var finalCount = 60;
        var maxWave = 100;
        return (WaveNumber >= 0 && WaveNumber <= maxWave) ? Mathf.RoundToInt(1f / (Mathf.Pow(maxWave, 2)) * (finalCount - startingCount) * Mathf.Pow(WaveNumber, 2)) + startingCount : finalCount;
    }
}
