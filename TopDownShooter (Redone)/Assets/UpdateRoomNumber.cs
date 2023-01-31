using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UpdateRoomNumber : MonoBehaviour
{
    private EnemyWaveSpawner _scriptWaveSpawner;
    [SerializeField] private TMP_Text _waveText;


    // Start is called before the first frame update
    private void Start()
    {
        _scriptWaveSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemyWaveSpawner>();

        StartCoroutine("SetWaveText");
    }


    private IEnumerator SetWaveText()
    {
        while (!_scriptWaveSpawner.WaveStarted)
        {
            _waveText.gameObject.SetActive(false);
            yield return null;
        }

        _waveText.gameObject.SetActive(true);
        _waveText.SetText($"{_scriptWaveSpawner.WaveNumber}");
    }
}
