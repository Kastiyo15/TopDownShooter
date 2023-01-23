using UnityEngine;

public interface IHittable
{
    public void OnHit(int amount);

    public void BulletType(int id);
}
