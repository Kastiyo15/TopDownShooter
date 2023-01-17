using UnityEngine;


public class Ability : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float activeTime;

    public virtual void Activate(GameObject parent) { } // will be called when we want to activate ability, virtual so we can override it
    public virtual void BeginCooldown(GameObject parent) { }
}
