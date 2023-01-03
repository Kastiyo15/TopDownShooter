using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShoot : MonoBehaviour
{
    [Header("List of Weapons")]
    [SerializeField] private List<WeaponController> _weaponController = new List<WeaponController>(); // List of weapon controller prefabs

    [Header("List of Bullets")]
    [SerializeField] private List<BulletController> _bulletController = new List<BulletController>(); // List of weapon controller prefabs


    // Will get these stats from the Weapon Controller list of scriptable objects
    /*     [Header("Weapon Stats")]
        [SerializeField] private int _weaponID;
        [SerializeField] private string _weaponName;
        [SerializeField] private int _weaponClipSize;
        [SerializeField] private float _weaponFireRate;
        [SerializeField] private float _weaponSpread;

        [Header("Weapon Stat Multipliers")]
        [SerializeField] private int _weaponClipSizeMult;
        [SerializeField] private float _weaponFireRateMult;
        [SerializeField] private float _weaponSpreadMult; */


    private int id = 0;
    private Transform _firePoint; // Where the bullet will fire from



    // Start is called before the first frame update
    private void Start()
    {
        // Set the stats of the weapon to the scriptable object selected with the 
        GetWeaponAndBulletStats();
    }

    // Update is called once per frame
    private void Update()
    {
        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            //Shoot();
        }

        // Change Weapon
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeapon(-1); // minus 1 from weapon selector value
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeWeapon(1); // add 1 from weapon selector value
        }
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
    }


    /*     private void Shoot()
        {
            if (Time.time > _wea)
        } */
}
