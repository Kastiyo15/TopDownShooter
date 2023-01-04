using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{

    private int _damageValue;

    // Once enabled, disable after 3 seconds
    private void OnEnable()
    {
        _damageValue = BulletStatsManager.Instance.B_BulletDamage;
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
        CancelInvoke();
    }

}
