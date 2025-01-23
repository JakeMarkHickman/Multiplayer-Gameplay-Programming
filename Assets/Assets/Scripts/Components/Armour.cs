using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Armour : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<float> m_Armour = new NetworkVariable<float>(
            1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public float GetArmour()
    {
        return m_Armour.Value;
    }

    public void SetArmour(float value)
    {
        if (!IsServer)
            return;

        m_Armour.Value = value;
    }
}
