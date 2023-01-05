using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShoot : MonoBehaviour
{
    [Header("Weapon Spread Stats")]
    [SerializeField] private float _startAngle = 0f;
    [SerializeField] private float _endAngle = 0f;
    [SerializeField] private float _angleStep = 0f;
    [SerializeField] private float _angle = 0f;

    [Header("List of Weapons")]
    [SerializeField] private List<WeaponController> _weaponController = new List<WeaponController>(); // List of weapon controller prefabs

    [Header("List of Bullets")]
    [SerializeField] private List<BulletController> _bulletController = new List<BulletController>(); // List of weapon controller prefabs

    [Header("Shooting Bools")]
    [SerializeField] private bool IsShootable = true; // can player shoot?

    [Header("References")]
    [SerializeField] private Transform _firePoint; // Where the bullet will fire from
    [SerializeField] private HUDScript _scriptHUD; // Script for the weapons HUD


    private int id = 0;
    private bool changedWeapon = false;
    private bool once = true;
    private float _overheatTimer = 0f;


    // Start is called before the first frame update
    private void Start()
    {
        // Set the weapon overheat cooldowns to false
        for (int i = 0; i < _weaponController.Count; i++)
        {
            _weaponController[i].WeaponData.WeaponIsOverheating = false;
        }

        // Set the stats of the weapon to the scriptable object selected with the 
        GetWeaponAndBulletStats();

        // Add rifle bullets to pool, until changed to shotgun, then add those to pool
        StartCoroutine("AddPoolBullets");
    }


    // Update is called once per frame
    private void Update()
    {
        // Shoot
        if (Input.GetMouseButton(0) && IsShootable && !GameManager.GameIsPaused)
        {
            StartCoroutine("Shoot");
        }

        #region 'CHANGE WEAPON' Keybinds
        // 'Change Weapon' Keybinds
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeapon(-1); // minus 1 from weapon selector value
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeWeapon(1); // add 1 from weapon selector value

            if (once)
            {
                changedWeapon = true;
                once = false;
            }
        }
        #endregion

        // Set Ammo counter HUD
        _scriptHUD.AmmoCounterHUD();
    }


    // Clamp id between [0, 1] and increase/decrease it when Q or E is pressed 
    // then load the scriptable object stats into the statsmanager
    private void ChangeWeapon(int i)
    {
        // Select Weapon ID
        id += i;
        id = Mathf.Clamp(id, 0, 1);
        Debug.Log(id);

        GetWeaponAndBulletStats();
    }


    // Load the scriptable objects stats into the Corresponding stats manager
    private void GetWeaponAndBulletStats()
    {
        _bulletController[id].LoadDefaultBulletData(_bulletController[id].BulletData);
        _weaponController[id].LoadDefaultWeaponData(_weaponController[id].WeaponData);

        // Change the Q/E sprite on the HUD
        _scriptHUD.WeaponSelectedHUD(id);
    }


    // Coroutine to fire bullets in a random spread
    private IEnumerator Shoot()
    {
        if (!_weaponController[id].WeaponData.WeaponIsOverheating)
        {
            IsShootable = false;
            Debug.Log("Shooting");


            #region BULLET SPREAD SCRIPT

            // if Rifle, then give messy spray when changing weapon spread
            if (id == 0)
            {
                // If you have 1 bullet and no weapon spread, fire accurate
                if (BulletStatsManager.Instance.B_BulletAmount == 1 && WeaponStatsManager.Instance.W_WeaponSpread == 0)
                {
                    _startAngle = -(_firePoint.eulerAngles.z) + WeaponStatsManager.Instance.W_WeaponSpread;
                    _endAngle = -(_firePoint.eulerAngles.z) - WeaponStatsManager.Instance.W_WeaponSpread;
                }
                // If you have 1 or more and bullet spread is not equal to 0, fire randomly
                else if (BulletStatsManager.Instance.B_BulletAmount >= 1 && WeaponStatsManager.Instance.W_WeaponSpread != 0)
                {
                    _startAngle = -(_firePoint.eulerAngles.z) + Random.Range(-WeaponStatsManager.Instance.W_WeaponSpread, WeaponStatsManager.Instance.W_WeaponSpread);
                    _endAngle = -(_firePoint.eulerAngles.z) - Random.Range(-WeaponStatsManager.Instance.W_WeaponSpread, WeaponStatsManager.Instance.W_WeaponSpread);
                }

                // _angleStep = (_endAngle - _startAngle) / _bulletController[id].BulletData.BulletAmount;
                _angleStep = ((2 * WeaponStatsManager.Instance.W_WeaponSpread) / (BulletStatsManager.Instance.B_BulletAmount));
                _angle = _startAngle - WeaponStatsManager.Instance.W_WeaponSpread;
            }

            // If shotgun, then give accurate spray, no randomness
            if (id == 1)
            {
                if (BulletStatsManager.Instance.B_BulletAmount > 0) // if you have more than 1 bullet, then do a shotgun arc
                {
                    _startAngle = -(_firePoint.eulerAngles.z) + WeaponStatsManager.Instance.W_WeaponSpread;
                    _endAngle = -(_firePoint.eulerAngles.z) - WeaponStatsManager.Instance.W_WeaponSpread;
                }

                // _angleStep = (_endAngle - _startAngle) / _bulletController[id].BulletData.BulletAmount;
                _angleStep = -((2 * WeaponStatsManager.Instance.W_WeaponSpread) / (BulletStatsManager.Instance.B_BulletAmount - 1));
                _angle = _startAngle;
            }


            for (int i = 0; i < BulletStatsManager.Instance.B_BulletAmount; i++)
            {
                float bulDirx = _firePoint.position.x + Mathf.Sin((_angle * Mathf.PI) / 180f);
                float bulDiry = _firePoint.position.y + Mathf.Cos((_angle * Mathf.PI) / 180f);

                Vector3 bulMoveVector = new Vector3(bulDirx, bulDiry, 0f);
                Vector2 bulDir = (bulMoveVector - _firePoint.position).normalized;

                // Initiate a bullet from the object pool
                GameObject bul = ObjectPool.Instance.GetPlayerBulletFromPool(id, WeaponStatsManager.Instance.W_WeaponClipSize);

                // Check if we have bullets in the clip
                // If we do then return, TODO: Reload timer!
                if (bul == null)
                {
                    StartCoroutine(ActivateOverheatCooldown(id));
                    Debug.Log("Shooting Stopped");
                    IsShootable = true;
                    yield break;
                    //yield return new WaitForSeconds(WeaponStatsManager.Instance.W_WeaponFireRate);
                }


                // Set position and rotation of the returned bullet and set it active
                bul.transform.position = _firePoint.position;
                bul.transform.rotation = _firePoint.rotation;
                bul.SetActive(true);


                // Access the rigidbody, in order to move the bullet
                Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
                rb.velocity = (bulDir * BulletStatsManager.Instance.B_BulletVelocity);


                // Clear the render trail, so another can be made
                bul.GetComponent<TrailRenderer>().Clear();


                // Increase angle before firing another bullet (if necessary)
                _angle += _angleStep;
            }
            #endregion


            // Wait 'weapon fire rate', before allowing code to be run again
            yield return new WaitForSeconds(WeaponStatsManager.Instance.W_WeaponFireRate);

            Debug.Log("Shooting Stopped");
            IsShootable = true;
        }
    }

    private IEnumerator AddPoolBullets()
    {
        if (id == 0)
        {
            ObjectPool.Instance.AddPlayerBulletsToPool(id, BulletStatsManager.Instance.B_BulletPrefab, WeaponStatsManager.Instance.W_WeaponClipSize);
        }

        while (!changedWeapon) yield return null;

        if (id == 1)
        {
            ObjectPool.Instance.AddPlayerBulletsToPool(id, BulletStatsManager.Instance.B_BulletPrefab, WeaponStatsManager.Instance.W_WeaponClipSize);
        }

        yield return null;
    }

    private IEnumerator ActivateOverheatCooldown(int g)
    {
        // As soon as weapon overheats, activate this coroutine
        // If current weapon equipped is not overheating
        _overheatTimer = WeaponStatsManager.Instance.W_WeaponOverheatCooldown;


        _weaponController[g].WeaponData.WeaponIsOverheating = true;
        Debug.Log("Weapon Overheated!");


        yield return new WaitForSeconds(_overheatTimer);


        _weaponController[g].WeaponData.WeaponIsOverheating = false;
        Debug.Log("Weapon Cooled!");
    }
}
