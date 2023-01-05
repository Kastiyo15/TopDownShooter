using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{

    private int _damageValue;


    // Once enabled, disable after 3 seconds
    private void OnEnable()
    {
        // when enabled, set the damage of the bullet to the current equipped weapon
        _damageValue = BulletStatsManager.Instance.B_BulletDamage;


        // decrease the respective bullet counters
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


        Invoke("Disable", 2f);
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // If we hit something with a different tag, see if we can cause damage
        if (hitInfo.tag != tag)
        {
            // Damage using the new Health Script
            if (hitInfo.gameObject.TryGetComponent<Health>(out var health))
            {
                health.Damage(_damageValue);
            }

            Disable();
        }
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
            WeaponStatsManager.Instance.W_CurrentRifleClip++;
        }
        // shotgun
        if (CompareTag("ShotgunBullet"))
        {
            WeaponStatsManager.Instance.W_CurrentShotgunClip++;
        }

        CancelInvoke();
    }

}
