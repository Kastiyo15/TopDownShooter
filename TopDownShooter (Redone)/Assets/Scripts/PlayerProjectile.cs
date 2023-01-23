using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{
    private int _damageValue;
    public int DamageMin;
    public int DamageMax;
    public Vector2 ProjectileDirection;
    public enum BulletType
    {
        Rifle,
        Shotgun
    }

    public BulletType type;
    private int _bulletID;

    // Once enabled, disable after 3 seconds
    private void OnEnable()
    {
        // when enabled, set the damage of the bullet to the current equipped weapon
        _damageValue = BulletStatsManager.Instance.B_BulletDamage;
        DamageMin = _damageValue - (Mathf.FloorToInt(_damageValue / 10));
        DamageMax = _damageValue + (Mathf.FloorToInt(_damageValue / 10));


        // decrease the respective bullet counters
        // rifle
        if (type == BulletType.Rifle)
        {
            WeaponStatsManager.Instance.W_CurrentRifleClip++;
            _bulletID = 0;
        }
        // shotgun
        if (type == BulletType.Shotgun)
        {
            WeaponStatsManager.Instance.W_CurrentShotgunClip++;
            _bulletID = 1;
        }

        Invoke("Disable", 2f);
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // If we hit something with a different tag, see if we can cause damage
        if (hitInfo.CompareTag("Enemy") || hitInfo.CompareTag("Obstacle"))
        {
            Disable();

            // Interface: Will minus damage from health of target hit
            if (hitInfo.gameObject.TryGetComponent<Health>(out var health))
            {
                var randDamageNumber = Random.Range(DamageMin, DamageMax);

                health.Damage(randDamageNumber);


                // Interface: Will knock back object
                var hittable = hitInfo.gameObject.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.OnHit(randDamageNumber);
                    hittable.BulletType(_bulletID);
                }
            }

            // Interface: Will knock back object
            var knockable = hitInfo.gameObject.GetComponent<IKnockable>();
            if (knockable != null)
            {
                knockable.KnockedBack(ProjectileDirection, Random.Range(Mathf.Round(BulletStatsManager.Instance.B_BulletKnockbackForce / 5f), BulletStatsManager.Instance.B_BulletKnockbackForce));
            }
        }
    }


    // Used in the player shoot script to get the bullets direction
    public void GetDirection(Vector2 direction)
    {
        ProjectileDirection = direction;
    }


    // when called, set the gameobject to false
    void Disable()
    {
        gameObject.SetActive(false);
    }


    // once disabled, stop invoking
    private void OnDisable()
    {
        // increase the respective bullet counters
        // rifle
        if (type == BulletType.Rifle)
        {
            WeaponStatsManager.Instance.W_CurrentRifleClip--;
        }
        // shotgun
        if (type == BulletType.Shotgun)
        {
            WeaponStatsManager.Instance.W_CurrentShotgunClip--;
        }

        CancelInvoke();
    }

}
