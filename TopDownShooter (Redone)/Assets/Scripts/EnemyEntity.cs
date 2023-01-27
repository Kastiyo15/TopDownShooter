using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;


public class EnemyEntity : MonoBehaviour, IKnockable, IHittable
{

    public int Level;

    [System.Serializable]
    public class Attributes
    {
        public int Health;
        public int Defence;
        public int Agility;
    }

    [System.Serializable]
    public class Stats
    {
        public int HealthRegenValue; // how much health is regenerated
        public float HealthRegenRate; // how quickly health regenerates

        public float BlockChance; // percentage, take no damage if blocked
        public int DefenceMultiplier; // increase this to increase how many points the base defence stat blocks

        public float MoveSpeed;
        public float KnockbackRange;
        public float StoppingDistance;

        public int ScoreValue; // how much score is earned when dead
    }

    [System.Serializable]
    public class References
    {
        public int ID;
        [HideInInspector] public Vector2 Target;
        [HideInInspector] public Vector2 MoveVelocity;
        [HideInInspector] public Vector2 LookDir;
        public Health ScriptHealth;
        [HideInInspector] public EnemyWaveSpawner ScriptEnemyWaveSpawner;
        public DamageNumber DamagePrefab;
        public Animator AnimatorEnemy;
    }

    public Attributes m_Attributes = new Attributes();
    public Stats m_Stats = new Stats();
    public References m_References = new References();

    [Header("Components")]
    [SerializeField] protected Rigidbody2D _rb;
    public SpriteRenderer _sr;
    public Material _originalMaterial;

    public int HitByBulletID;



    // Knock back the Enemy when hit (Uses Interface)
    public void KnockedBack(Vector2 direction, float knockForce)
    {
        _rb.AddForce(direction * knockForce, ForceMode2D.Impulse);
    }


    // Code to run when this gameobject dies
    public void OnHit(int amount)
    {
        m_References.DamagePrefab.Spawn(transform.position + Vector3.up, amount);

        if (m_References.ScriptHealth.Hp > 0)
        {
            m_References.AnimatorEnemy.SetTrigger("OnHit");
            m_References.AnimatorEnemy.SetInteger("ID", m_References.ID);
        }
    }


    public void BulletType(int id)
    {
        HitByBulletID = id;
    }
}
