using Unity.VisualScripting;
using UnityEngine;

public class DDoL : MonoBehaviour
{
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
}
