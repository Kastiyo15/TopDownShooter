using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IKnockable
{
    [SerializeField] private Vector2 _target;
    [SerializeField] private Vector2 _moveVelocity;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _knockbackRange;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private int _scoreValue; // how much score is earned when dead


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
            _rb.position = Vector2.MoveTowards(_rb.position, _target, _moveSpeed * Time.deltaTime);
        }
    }


    // Rotate enemy towards target
    private void RotateEnemy()
    {
        // Rotate player part 2
        Vector2 lookDir = (_target - _rb.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        _rb.rotation = Mathf.Lerp(_rb.rotation, angle, _moveSpeed);
    }


    // Knock back the Enemy when hit (Uses Interface)
    public void KnockedBack(Vector2 direction)
    {
        var knockForce = Random.Range(Mathf.Round(BulletStatsManager.Instance.B_BulletKnockbackForce / 5f), BulletStatsManager.Instance.B_BulletKnockbackForce);
        _rb.AddForce(direction * knockForce, ForceMode2D.Impulse);
    }


    // Code to run when this gameobject dies
    public void OnEnemyDeath()
    {
        ScoreStatsManager.Instance.AddScorePerKill(_scoreValue);
    }

}
