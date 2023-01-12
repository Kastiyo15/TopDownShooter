using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxArea : MonoBehaviour
{
    [SerializeField] private Vector2 _size;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _size);
    }
#endif


    public Vector2 GetRandomPoint(System.Random r = null)
    {
        var rand = r != null ? (float)r.NextDouble() : Random.value;
        rand *= (2f * _size.x + 2 * _size.y);
        if (rand < _size.x)
            return new Vector2(rand, 0) - (_size / 2f);
        rand -= _size.x;
        if (rand < _size.y)
            return new Vector2(_size.x, rand) - (_size / 2f);
        rand -= _size.y;
        if (rand < _size.x)
            return new Vector2(rand, _size.y) - (_size / 2f);
        else
            return new Vector2(0, rand - _size.x) - (_size / 2f);
    }
}
