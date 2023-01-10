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
    private List<GameObject> _playerBulletPool = new List<GameObject>();


    [Header("Enemy Pools")]
    // Pool Enemy Agents
    private List<GameObject> _enemyAgentPool = new List<GameObject>();


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



    public void AddEnemyAgentsToPool(int poolSize, int enemyTypes, EnemySO[] enemyArray)
    {
        // run a for loop, add 1 agent of each type to the pool at a time
        // decreasing a variable after each time someone is added
        // Stop when variable = poolsize

        int enemyCounter = poolSize;

        for (int i = 0; _enemyAgentPool.Count < poolSize; i++)
        {
            for (int j = 0; j < enemyTypes && enemyCounter > 0; j++)
            {
                GameObject tmp;
                tmp = Instantiate(enemyArray[j].EnemyPrefab, _parentEnemyAgent.transform);
                tmp.SetActive(false);
                _enemyAgentPool.Add(tmp);
                enemyCounter--;
            }
        }
    }


    public GameObject GetEnemyAgentsFromPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            // check if enemy agent matching the id is active in hierarchy
            if (!_enemyAgentPool[i].activeInHierarchy)
            {
                return _enemyAgentPool[i];
            }
        }

        return null;
    }


    public int GetActiveEnemyAgents()
    {
        var activeAgents = 0;

        foreach (GameObject enemy in _enemyAgentPool)
        {
            if (enemy.activeInHierarchy)
            {
                activeAgents++;
            }
        }

        return activeAgents;
    }
}
