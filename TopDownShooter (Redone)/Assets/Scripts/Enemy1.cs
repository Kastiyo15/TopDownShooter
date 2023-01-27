using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyEntity
{
    // Once enabled, disable after 3 seconds
    private void OnEnable()
    {
        m_References.AnimatorEnemy.SetInteger("ID", m_References.ID);
        _sr.material = _originalMaterial;
        m_References.ScriptHealth.Adjust(m_References.ScriptHealth.MaxHp);
        m_References.ScriptEnemyWaveSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemyWaveSpawner>();


        Level = 1;
    }


    // Just used to get information
    private void Update()
    {
        // Get the vector of player(target)
        GetTargetPosition();
    }


    // Used for moving phyics objects, Rigidbody
    private void FixedUpdate()
    {
        // Move towards target
        FollowTarget();

        // rotate enemy
        RotateEnemy();
    }


    // Get the vector of player(target)
    private void GetTargetPosition()
    {
        m_References.Target = FindObjectOfType<Player>().gameObject.transform.position;
    }


    // Move towards target Vector
    private void FollowTarget()
    {
        if (Vector2.Distance(_rb.position, m_References.Target) > m_Stats.StoppingDistance)
        {
            // _rb.position = Vector2.MoveTowards(_rb.position, _target, _moveSpeed * Time.deltaTime);
            _rb.velocity += m_References.LookDir * m_Stats.MoveSpeed * Time.fixedDeltaTime;
        }
    }


    // Rotate enemy towards target
    private void RotateEnemy()
    {
        // Rotate player part 2
        m_References.LookDir = (m_References.Target - _rb.position);
        float angle = Mathf.Atan2(m_References.LookDir.y, m_References.LookDir.x) * Mathf.Rad2Deg - 90f;
        _rb.rotation = Mathf.Lerp(_rb.rotation, angle, m_Stats.MoveSpeed);
    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }


    // once disabled, stop invoking
    private void OnDisable()
    {
        if (m_References.ScriptEnemyWaveSpawner.WaveStarted)
        {
            // Add to killed counter
            CareerStatsManager.Instance.UpdateCareerStatsVariable(CareerStatsManager.VariableType.EnemiesKilled);

            // Add to score
            ScoreStatsManager.Instance.AddScorePerKill(m_Stats.ScoreValue);

            // Decrease enemies alive counter in the wave spawner
            m_References.ScriptEnemyWaveSpawner.DecreaseEnemiesRemaining();

            // Add xp to weapon and player
            LevelManager.Instance.GainExperience(LevelManager.Instance.m_Player, Random.Range(45, 55));

            if (HitByBulletID == 0)
            {
                LevelManager.Instance.GainExperience(LevelManager.Instance.m_Rifle, Random.Range(45, 55));
            }
            else if (HitByBulletID == 1)
            {
                LevelManager.Instance.GainExperience(LevelManager.Instance.m_Shotgun, Random.Range(45, 55));
            }
        }

        CancelInvoke();
    }
}
