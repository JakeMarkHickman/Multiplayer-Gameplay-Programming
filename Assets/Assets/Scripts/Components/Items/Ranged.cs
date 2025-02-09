using Unity.Netcode;
using UnityEngine;

public class Ranged : Item
{
    [SerializeField] NetworkObject ProjectileToSpawn;

    private NetworkVariable<Vector2> FireDir = new NetworkVariable<Vector2>(
            new Vector2(0,0),
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );

    Vector3 playerPos;

    public override void Use(NetworkObject user)
    {
        if (!IsServer)
            return;

        playerPos = user.transform.position;
        FireDir.Value = user.GetComponent<Movement>().m_LastDirection.Value;

        if (FireDir.Value == Vector2.zero)
        {
            FireDir.Value = new Vector2(0, 1);
        }

        FireRPC(user.gameObject.tag);
        
        base.Use(user);
    }

    [Rpc(SendTo.Server)]
    public void FireRPC(string tag)
    {
        Vector3 pos = playerPos + (Vector3)FireDir.Value.normalized;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, FireDir.Value);

        if (!ProjectileToSpawn)
            return;

        NetworkObject Proj = NetworkManager.SpawnManager.InstantiateAndSpawn
            (
                ProjectileToSpawn,
                0,
                false,
                false,
                false,
                pos,
                rot
            );

        Proj.tag = tag;

        if(Proj.GetComponent<Rigidbody2D>())
        {
            Proj.GetComponent<Rigidbody2D>().AddForce(FireDir.Value.normalized * Proj.GetComponent<Speed>().GetSpeed(), ForceMode2D.Impulse);
        }
    }
}
