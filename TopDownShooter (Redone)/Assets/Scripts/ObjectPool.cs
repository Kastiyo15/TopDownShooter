using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    // Hopefully the parent object where each object pool will be a child of
    [Header("Object Pool Parents")]
    [SerializeField] private GameObject _parentPlayerBullets;
    [SerializeField] private GameObject _parentEnemyAgent;
    [SerializeField] private GameObject _parentEnemyBullets;


    [Header("Player Pools")]
    // Pool Player Bullets
    [SerializeField] private GameObject _playerBullet;
    private List<GameObject> _playerBulletPool = new List<GameObject>();
    [SerializeField] public int _playerBulletPoolSize;


    [Header("Enemy Pools")]
    // Pool Enemy Agents
    [SerializeField] private GameObject _enemyAgent;
    private List<GameObject> _enemyAgentPool;
    [SerializeField] private int _enemyAgentPoolSize;


    // Pool Enemy Bullets
    [SerializeField] private GameObject _enemyBullet;
    private List<GameObject> _enemyBulletPool;
    [SerializeField] private int _enemyBulletPoolSize;


    // BOOLS
    private bool RifleBulletPoolAdded = false;
    private bool ShotgunBulletPoolAdded = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



    public void AddPlayerBulletsToPool(int id, GameObject bulletPrefab, int poolSize)
    {
        // PLAYER BULLETS // 
        // So rifle should be first gun out, so add all these bullets to the pool first
        if (id == 0 && !RifleBulletPoolAdded && !ShotgunBulletPoolAdded)
        {
            // Create gameobjects, but set them inactive, add them to the list
            for (int i = 0; i < poolSize; i++)
            {
                GameObject tmp;
                tmp = Instantiate(bulletPrefab, _parentPlayerBullets.transform);
                tmp.SetActive(false);
                _playerBulletPool.Add(tmp);
            }

            RifleBulletPoolAdded = true;
        }

        // now rifle is loaded in, added the shotgun bullets
        if (id == 1 && RifleBulletPoolAdded && !ShotgunBulletPoolAdded)
        {
            // Create gameobjects, but set them inactive, add them to the list
            for (int i = 0; i < poolSize; i++)
            {
                GameObject tmp;
                tmp = Instantiate(bulletPrefab, _parentPlayerBullets.transform);
                tmp.SetActive(false);
                _playerBulletPool.Add(tmp);
            }

            ShotgunBulletPoolAdded = true;
        }

    }


    // Player Functions
    public GameObject GetPlayerBulletFromPool(int iD, int poolSize)
    {
        if (iD == 0)
        {
            // find an inactive object and return in, by looping through list
            for (int i = 0; i < poolSize; i++)
            {
                if (!_playerBulletPool[i].activeInHierarchy)
                {
                    return _playerBulletPool[i];
                }
            }
        }

        if (iD == 1)
        {
            var secondPoolSize = (_playerBulletPool.Count - 1) - poolSize;

            // find an inactive object and return in, by looping through list
            for (int i = _playerBulletPool.Count - 1; i > secondPoolSize; i--)
            {
                if (!_playerBulletPool[i].activeInHierarchy)
                {
                    return _playerBulletPool[i];
                }
            }
        }

        return null;
    }
}
