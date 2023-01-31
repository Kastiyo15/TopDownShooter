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
            StopCoroutine("Shoot");
            StartCoroutine("Shoot");
        }

        #region 'CHANGE WEAPON' Keybinds
        // 'Change Weapon' Keybinds
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeapon(-1); // minus 1 from weapon selector value
            LevelManager.Instance.HideWeaponBars(WeaponStatsManager.Instance.W_WeaponID);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeWeapon(1); // add 1 from weapon selector value
            LevelManager.Instance.HideWeaponBars(WeaponStatsManager.Instance.W_WeaponID);

            // Used to add bullets to item pool only once
            if (once)
            {
                changedWeapon = true;
                once = false;
            }
        }
        #endregion

        // Set Ammo counter HUD
        if (!_weaponController[id].WeaponData.WeaponIsOverheating)
        {
            _scriptHUD.AmmoCounterHUD(0);
        }
        else if (_weaponController[id].WeaponData.WeaponIsOverheating)
        {
            _scriptHUD.AmmoCounterHUD(1);
        }
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
        _scriptHUD.DisplayAmmoBars(id);
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
            // Shake the camera when shooting
            CinemachineShake.Instance.ShakeCamera((Mathf.Clamp(BulletStatsManager.Instance.B_BulletKnockbackForce / 15f, 0f, 10f)), 0.25f);

            IsShootable = false;
            //Debug.Log("Shooting");

            #region BULLET SPREAD SCRIPT

            // if Rifle, then give messy spray when changing weapon spread
            if (id == 0)
            {
                // If you have 1 bullet 
                if (BulletStatsManager.Instance.B_BulletAmount == 1)
                {
                    // fire accurate
                    if (WeaponStatsManager.Instance.W_WeaponSpread == 0)
                    {
                        _startAngle = (_firePoint.eulerAngles.z);
                        _endAngle = (_firePoint.eulerAngles.z);
                        _angleStep = 0f;
                        _angle = -_startAngle;
                    }
                    // Fire randomly within range of spread
                    else if (WeaponStatsManager.Instance.W_WeaponSpread != 0)
                    {
                        // divide by 2 to get the range equal to the weapon spread value, otherwise the range is double the spread value
                        _startAngle = _firePoint.eulerAngles.z - (WeaponStatsManager.Instance.W_WeaponSpread * 0.5f);
                        _endAngle = _firePoint.eulerAngles.z + (WeaponStatsManager.Instance.W_WeaponSpread * 0.5f);
                        _angleStep = 0f;

                        var spread = Random.Range(_startAngle, _endAngle);
                        _angle = -spread;
                    }
                }
                // MAKE SURE TO LOCK BULLET AMOUNT WHILST WEAPON SPREAD IS 0 (have to increase weapon spread before unlocking new bullet)
                // If you have more than 1 bullet
                else if (BulletStatsManager.Instance.B_BulletAmount > 1)
                {
                    // Fire in range / bullet amount (accurate)
                    if (WeaponStatsManager.Instance.W_WeaponSpread != 0)
                    {
                        var spread = Random.Range(0f, WeaponStatsManager.Instance.W_WeaponSpread);

                        _startAngle = _firePoint.eulerAngles.z - (spread * 0.5f);
                        _endAngle = _firePoint.eulerAngles.z + (spread * 0.5f);

                        _angleStep = -WeaponStatsManager.Instance.W_WeaponSpread / BulletStatsManager.Instance.B_BulletAmount;
                        _angle = -_startAngle;
                    }
                }
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
                    //Debug.Log("Shooting Stopped");
                    IsShootable = true;
                    yield break;
                    //yield return new WaitForSeconds(WeaponStatsManager.Instance.W_WeaponFireRate);
                }


                // Set position and rotation of the returned bullet and set it active
                bul.transform.position = _firePoint.position;
                bul.transform.rotation = _firePoint.rotation;

                bul.SetActive(true);


                // Access the rigidbody, in order to move the bullet, and add it ot player velocity
                Vector2 playerVelocity = gameObject.GetComponent<Player>().Rb.velocity;
                Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
                rb.velocity = (bulDir * (BulletStatsManager.Instance.B_BulletVelocity + (playerVelocity.magnitude * 0.5f)));


                // Clear the render trail, so another can be made
                bul.GetComponent<TrailRenderer>().Clear();

                // Send the bullet direction to the projectile
                bul.GetComponent<PlayerProjectile>().GetDirection(bulDir);

                // Increase angle before firing another bullet (if necessary)
                _angle += _angleStep;

                // Interface: Will knock back player
                var knockable = gameObject.GetComponent<IKnockable>();
                var knockForceMultiplier = 3f;
                if (knockable != null)
                {
                    knockable.KnockedBack(bulDir, Random.Range(BulletStatsManager.Instance.B_BulletKnockbackForce * knockForceMultiplier, BulletStatsManager.Instance.B_BulletKnockbackForce * knockForceMultiplier * 2f));
                }
            }
            #endregion

            // Wait 'weapon fire rate', before allowing code to be run again
            yield return new WaitForSeconds(WeaponStatsManager.Instance.W_WeaponFireRate);

            //Debug.Log("Shooting Stopped");
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
        // set timer to the current weapons cooldown timer
        _overheatTimer = WeaponStatsManager.Instance.W_WeaponOverheatCooldown;

        // trip the current weapons cooldown bool
        _weaponController[g].WeaponData.WeaponIsOverheating = true;
        Debug.Log("Weapon Overheated!");

        // Activate overheat bar
        _scriptHUD.StartCoroutine(_scriptHUD.ActivateOverheatBar(g, _overheatTimer));

        // wait for the cooldown timer
        yield return new WaitForSeconds(_overheatTimer);

        // set the current weapons bool false
        _weaponController[g].WeaponData.WeaponIsOverheating = false;
        Debug.Log("Weapon Cooled!");
    }
}
