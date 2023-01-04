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


    // Start is called before the first frame update
    private void Start()
    {
        // Set the stats of the weapon to the scriptable object selected with the 
        GetWeaponAndBulletStats();


        // Loop through equipped weapons and add their bullets to the object pool
        for (int i = 0; i < _weaponController.Count; i++)
        {
            ObjectPool.Instance.AddPlayerBulletsToPool(_bulletController[i].BulletData.BulletPrefab, _weaponController[i].WeaponData.WeaponClipSize);
        }
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
        }
        #endregion
    }


    private void ChangeWeapon(int i)
    {
        // Select Weapon ID
        id += i;
        id = Mathf.Clamp(id, 0, 1);
        Debug.Log(id);

        GetWeaponAndBulletStats();
    }


    private void GetWeaponAndBulletStats()
    {
        _weaponController[id].LoadWeaponData(_weaponController[id].WeaponData);
        _bulletController[id].LoadBulletData(_bulletController[id].BulletData);

        _scriptHUD.WeaponSelectedHUD(id);
    }


    private IEnumerator Shoot()
    {
        IsShootable = false;
        Debug.Log("Shooting");

        #region BULLET SPREAD SCRIPT

        // if Rifle, then give messy spray when changing weapon spread
        if (id == 0)
        {
            // If you have 1 bullet and no weapon spread, fire accurate
            if (_bulletController[id].BulletData.BulletAmount == 1 && _weaponController[id].WeaponData.WeaponSpread == 0)
            {
                _startAngle = -(_firePoint.eulerAngles.z) + _weaponController[id].WeaponData.WeaponSpread;
                _endAngle = -(_firePoint.eulerAngles.z) - _weaponController[id].WeaponData.WeaponSpread;
            }
            // If you have 1 or more and bullet spread is not equal to 0, fire randomly
            else if (_bulletController[id].BulletData.BulletAmount >= 1 && _weaponController[id].WeaponData.WeaponSpread != 0)
            {
                _startAngle = -(_firePoint.eulerAngles.z) + Random.Range(-_weaponController[id].WeaponData.WeaponSpread, _weaponController[id].WeaponData.WeaponSpread);
                _endAngle = -(_firePoint.eulerAngles.z) - Random.Range(-_weaponController[id].WeaponData.WeaponSpread, _weaponController[id].WeaponData.WeaponSpread);
            }
        }

        // If shotgun, then give accurate spray, no randomness
        if (id == 1)
        {
            if (_bulletController[id].BulletData.BulletAmount > 0) // if you have more than 1 bullet, then do a shotgun arc
            {
                _startAngle = -(_firePoint.eulerAngles.z) + _weaponController[id].WeaponData.WeaponSpread;
                _endAngle = -(_firePoint.eulerAngles.z) - _weaponController[id].WeaponData.WeaponSpread;
            }
        }

        // _angleStep = (_endAngle - _startAngle) / _bulletController[id].BulletData.BulletAmount;
        _angleStep = -((2 * _weaponController[id].WeaponData.WeaponSpread) / (_bulletController[id].BulletData.BulletAmount - 1));
        _angle = _startAngle;


        for (int i = 0; i < _bulletController[id].BulletData.BulletAmount; i++)
        {
            float bulDirx = _firePoint.position.x + Mathf.Sin((_angle * Mathf.PI) / 180f);
            float bulDiry = _firePoint.position.y + Mathf.Cos((_angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirx, bulDiry, 0f);
            Vector2 bulDir = (bulMoveVector - _firePoint.position).normalized;

            // Initiate a bullet from the object pool
            GameObject bul = ObjectPool.Instance.GetPlayerBulletFromPool(id, _weaponController[id].WeaponData.WeaponClipSize);

            // Check if we have bullets in the clip
            // If we do then return, TODO: Reload timer!
            if (bul == null)
            {
                yield return new WaitForSeconds(WeaponStatsManager.Instance.W_WeaponFireRate);

                Debug.Log("Shooting Stopped");
                IsShootable = true;
            }

            // Set position and rotation of the returned bullet and set it active
            bul.transform.position = _firePoint.position;
            bul.transform.rotation = _firePoint.rotation;
            bul.SetActive(true);


            // Access the rigidbody, in order to move the bullet
            Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
            rb.velocity = (bulDir * _bulletController[id].BulletData.BulletVelocity);


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
