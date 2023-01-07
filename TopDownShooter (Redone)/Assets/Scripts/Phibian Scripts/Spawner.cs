using UnityEngine;


public class Spawner : MonoBehaviour
{
    public Enemy SpawnPrefab;
    public Transform Parent;


    public void Spawn()
    {
        Instantiate(SpawnPrefab, transform.position, Quaternion.identity);
    }

    public void Start()
    {
        Spawn();
    }
}
