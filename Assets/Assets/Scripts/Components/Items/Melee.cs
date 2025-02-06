using Unity.Netcode;
using UnityEngine;

public class Melee : Item
{

    public override void Use(NetworkObject user)
    {
        base.Use(user);

        Attack();
    }

    public void Attack()
    {

    }
}
