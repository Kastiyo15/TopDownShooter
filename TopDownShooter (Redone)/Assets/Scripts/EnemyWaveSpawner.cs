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
    public Vector2 RoomCentre;

    [Header("Debug Purposes")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private int waveSpawnCount;
    [SerializeField] private int _arrayLength;

    [Header("Keep Track Of")]
    public bool WaveResting; // TRUE when player is wandering around room, before Start Wave
    public bool WaveStarted; // TRUE when player walks in room trigger
    public bool WaveComplete; // TRUE when player kills all enemies in the wave
    [SerializeField] private float _waveDelay;
    public int EnemiesRemaining = 0;
    public int WaveNumber;


    // Start is called before the first frame update
    private void Start()
    {
        WaveResting = true;
        WaveStarted = false;
        WaveComplete = true;
        
        RoomCentre = new Vector2(0f, 0f); // initialise this variable
        _arrayLength = _enemyScriptableObject.Length; // initialise length of array variable
    }


    private void Update()
    {
        if (WaveStarted && EnemiesRemaining == 0)
        {
            EndWave();
        }
    }


    public void DecreaseEnemiesRemaining()
    {
        EnemiesRemaining--;
        _scriptHUD.UpdateKillsRemaining(EnemiesRemaining);
        _scriptHUD.UpdateKillsThisRun();
    }


    // Called from RoomTrigger
    public void GetRoomCentre(Vector2 roomPosition)
    {
        RoomCentre = roomPosition;
        _boxArea.gameObject.transform.position = roomPosition;
    }


    // Called from RoomTrigger
    public void StartWave()
    {
        GameManager.EnteredFirstRoom = true;

        EnemiesRemaining = -1; // Stop the code firing when it = 0
        WaveResting = false;
        WaveStarted = true;
        WaveComplete = false;

        StartCoroutine(WaveSpawning());


        _scriptHUD.ShowHUD();
    }


    private void EndWave()
    {
        print("EndWave");
        WaveResting = true;
        WaveStarted = false;
        WaveComplete = true;

        ScoreStatsManager.Instance.AddWaveScore(WaveNumber * 10);
    }


    private void GetEnemyCount()
    {
        EnemiesRemaining = waveSpawnCount;
        _scriptHUD.UpdateKillsRemaining(EnemiesRemaining);
    }


    private IEnumerator WaveSpawning()
    {
        if (WaveNumber <= 100)
        {
            spawnDelay = GetSpawnDelay();
            waveSpawnCount = GetSpawnCount();

            yield return new WaitForSeconds(spawnDelay);

            // Add enemy SO into the object pool, removing the ones from previous wave
            ObjectPool.Instance.AddEnemyAgentsToPool(waveSpawnCount, _arrayLength, _enemyScriptableObject);

            GetEnemyCount();

            for (int i = 0; i < waveSpawnCount; i++)
            {
                var nextSpawnPos = _boxArea.GetRandomPoint();

                //yield return new WaitForSeconds(_waveDelay / waveSpawnCount);
                yield return new WaitForSeconds(spawnDelay);

                var newSpawn = ObjectPool.Instance.GetEnemyAgentsFromPool(waveSpawnCount);
                newSpawn.SetActive(true);

                newSpawn.transform.position = (RoomCentre + nextSpawnPos);
            }
            WaveNumber++;
        }
    }


    private float GetSpawnDelay()
    {
        var startingDelay = 2f;
        var finalDelay = 0.5f;
        var maxWave = 100;
        return (WaveNumber >= 0 && WaveNumber <= maxWave) ? (1f / (Mathf.Pow(maxWave, 2)) * (startingDelay - finalDelay) * Mathf.Pow(WaveNumber - maxWave, 2)) : finalDelay;
    }


    private int GetSpawnCount()
    {
        var startingCount = 1;
        var finalCount = 60;
        var maxWave = 100;
        return (WaveNumber >= 0 && WaveNumber <= maxWave) ? Mathf.RoundToInt(1f / (Mathf.Pow(maxWave, 2)) * (finalCount - startingCount) * Mathf.Pow(WaveNumber, 2)) + startingCount : finalCount;
    }
}
