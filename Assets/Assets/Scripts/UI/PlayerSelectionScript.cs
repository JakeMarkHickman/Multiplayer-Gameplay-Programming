using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class PlayerSelectionScript : NetworkBehaviour
{
    [SerializeField] UIDocument document;

    ulong[] m_RougeQueue = new ulong[2];
    ulong[] m_CrafterQueue = new ulong[2];
    ulong[] m_RandomQueue = new ulong[2];


    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("button", SelectedButton);

        if(IsOwner)
        {
            document.enabled = true;

            Button rouge = document.rootVisualElement.Q<Button>("rouge_BTN");
            rouge.RegisterCallback<ClickEvent>(rougeButtonHandler);

            Button crafter = document.rootVisualElement.Q<Button>("crafter_BTN");
            crafter.RegisterCallback<ClickEvent>(crafterButtonHandler);
        }
    }

    void rougeButtonHandler(ClickEvent data)
    {
        using FastBufferWriter writer = new FastBufferWriter(256, Unity.Collections.Allocator.Temp);
        writer.WriteValueSafe("Rouge");
        NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("button", NetworkManager.ServerClientId, writer, NetworkDelivery.Reliable);
    }

    void crafterButtonHandler(ClickEvent data)
    {
        using FastBufferWriter writer = new FastBufferWriter(256, Unity.Collections.Allocator.Temp);
        writer.WriteValueSafe("Crafter");
        NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("button", NetworkManager.ServerClientId, writer, NetworkDelivery.Reliable);
    }

    void SelectedButton(ulong senderClientId, FastBufferReader reader)
    {
        string data = "";

        reader.ReadValueSafe(out data);

        if(data == "Rouge")
        {
            for(int i = 0; i < m_RougeQueue.Length; i++)
            {
                if (m_RougeQueue[i] == 0)
                {
                    m_RougeQueue[i] = senderClientId;
                }
            }
        }

        if (data == "Crafter")
        {
            for (int i = 0; i < m_CrafterQueue.Length; i++)
            {
                if (m_CrafterQueue[i] == 0)
                {
                    m_CrafterQueue[i] = senderClientId;
                }
            }
        }

        if (data == "Random")
        {
            for (int i = 0; i < m_RandomQueue.Length; i++)
            {
                if (m_RandomQueue[i] == 0)
                {
                    m_RandomQueue[i] = senderClientId;
                }
            }
        }

        for (int i = 0; i < m_RougeQueue.Length; i++)
        {
            
        }
    }
}
