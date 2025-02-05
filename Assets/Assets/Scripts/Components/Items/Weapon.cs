using Unity.Netcode;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    public void Attack(NetworkObject User, Vector3 AttackDirection) { }

    protected virtual void ApplyDamage() { }
}
