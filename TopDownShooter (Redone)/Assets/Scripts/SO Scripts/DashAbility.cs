using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DashAbility", menuName = "My Game/DashAbility SO")]
public class DashAbility : Ability
{

    public float DashVelocity;

    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.IsDashing = true;
        player.MovementSpeed = DashVelocity;
    }


    public override void BeginCooldown(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.IsDashing = false;
        player.MovementSpeed = player.NormalMovementSpeed;
    }
}
