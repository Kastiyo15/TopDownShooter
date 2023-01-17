using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class Enemy : MonoBehaviour, IKnockable, IHittable
{
    [Header("Components")]
    private Vector2 _target;
    private Vector2 _moveVelocity;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Material _originalMaterial;

    [Header("Variables")]
    [SerializeField] private int _iD;
    [SerializeField] private float _knockbackRange;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private int _scoreValue; // how much score is earned when dead
    private Vector2 _lookDir;

    [Header("References")]
    [SerializeField] private Health _scriptHealth;
    private EnemyWaveSpawner _scriptEnemyWaveSpawner;
    [SerializeField] private DamageNumber _damagePrefab;
    [SerializeField] private Animator _animatorEnemy;


    // Once enabled, disable after 3 seconds
    private void OnEnable()
    {
        _animatorEnemy.SetInteger("ID", _iD);
        _sr.material = _originalMaterial;
        _scriptHealth.Adjust(_scriptHealth.MaxHp);
        _scriptEnemyWaveSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemyWaveSpawner>();
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
        _target = FindObjectOfType<Player>().gameObject.transform.position;
    }


    // Move towards target Vector
    private void FollowTarget()
    {
        if (Vector2.Distance(_rb.position, _target) > _stoppingDistance)
        {
           // _rb.position = Vector2.MoveTowards(_rb.position, _target, _moveSpeed * Time.deltaTime);
            _rb.velocity += _lookDir * _moveSpeed * Time.fixedDeltaTime;
        }
    }


    // Rotate enemy towards target
    private void RotateEnemy()
    {
        // Rotate player part 2
        _lookDir = (_target - _rb.position);
        float angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg - 90f;
        _rb.rotation = Mathf.Lerp(_rb.rotation, angle, _moveSpeed);
    }


    // Knock back the Enemy when hit (Uses Interface)
    public void KnockedBack(Vector2 direction, float knockForce)
    {
        _rb.AddForce(direction * knockForce, ForceMode2D.Impulse);
    }


    // Code to run when this gameobject dies
    public void OnHit(int damageValue)
    {
        _damagePrefab.Spawn(transform.position + Vector3.up, damageValue);

        if (_scriptHealth.Hp > 0)
        {
            _animatorEnemy.SetTrigger("OnHit");
            _animatorEnemy.SetInteger("ID", _iD);
        }
    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }


    // once disabled, stop invoking
    private void OnDisable()
    {
        ScoreStatsManager.Instance.AddScorePerKill(_scoreValue);
        _scriptEnemyWaveSpawner.DecreaseEnemiesRemaining();
        CancelInvoke();
    }
}
