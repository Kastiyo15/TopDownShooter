using System.Collections;
using UnityEngine;


public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HUDScript _scriptHUD;
    [SerializeField] private Camera _cam;
    [SerializeField] private BoxArea _boxArea;
    [SerializeField] private EnemySO _enemyScriptableObject;

    [SerializeField] private float _waveDelay;
    public int WaveNumber;


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

        if (WaveNumber <= 100)
        {
            _scriptHUD.UpdateWaveHUD(WaveNumber);

            yield return new WaitForSeconds(spawnDelay);

            for (int i = 0; i <= waveSpawnCount; i++)
            {
                var nextSpawnPos = _boxArea.GetRandomPoint();

                yield return new WaitForSeconds(_waveDelay / waveSpawnCount);

                var newSpawner = Instantiate(_enemyScriptableObject.EnemyPrefab, ((Vector2)_cam.transform.position + nextSpawnPos), Quaternion.identity);
            }
            yield return new WaitForSeconds(_waveDelay);
            WaveNumber++;
            StartCoroutine(WaveSpawning());
        }
    }


    private float GetSpawnDelay()
    {
        var startingDelay = 2f;
        var finalDelay = 0.25f;
        var maxWave = 100;
        return (WaveNumber >= 0 && WaveNumber <= maxWave) ? (1f / (Mathf.Pow(maxWave, 2)) * (startingDelay - finalDelay) * Mathf.Pow(WaveNumber - maxWave, 2)) : finalDelay;
    }


    private int GetSpawnCount()
    {
        var startingCount = 4;
        var finalCount = 32;
        var maxWave = 75;
        return (WaveNumber >= 0 && WaveNumber <= maxWave) ? Mathf.RoundToInt(1f / (Mathf.Pow(maxWave, 2)) * (finalCount - startingCount) * Mathf.Pow(WaveNumber, 2)) + startingCount : finalCount;
    }
}
