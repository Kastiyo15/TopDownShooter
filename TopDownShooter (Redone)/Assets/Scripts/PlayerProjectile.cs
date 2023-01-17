using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{
    private int _damageValue;
    public Vector2 ProjectileDirection;


    // Once enabled, disable after 3 seconds
    private void OnEnable()
    {
        // when enabled, set the damage of the bullet to the current equipped weapon
        _damageValue = BulletStatsManager.Instance.B_BulletDamage;
        
        // decrease the respective bullet counters
        // rifle
        if (CompareTag("RifleBullet"))
        {
            WeaponStatsManager.Instance.W_CurrentRifleClip++;
        }
        // shotgun
        if (CompareTag("ShotgunBullet"))
        {
            WeaponStatsManager.Instance.W_CurrentShotgunClip++;
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
                health.Damage(_damageValue);


                // Interface: Will knock back object
                var hittable = hitInfo.gameObject.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittable.OnHit(_damageValue);
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
        if (CompareTag("RifleBullet"))
        {
            WeaponStatsManager.Instance.W_CurrentRifleClip--;
        }
        // shotgun
        if (CompareTag("ShotgunBullet"))
        {
            WeaponStatsManager.Instance.W_CurrentShotgunClip--;
        }

        CancelInvoke();
    }

}
