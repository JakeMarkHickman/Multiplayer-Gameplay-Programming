using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    [SerializeField] UIDocument m_Hud;

    private Health m_HealthComp;

    private void OnEnable()
    {
        m_HealthComp = gameObject.GetComponent<Health>();

        m_HealthComp.healthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        m_HealthComp.healthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(HealthChangeStruct data)
    {
        //rootVi healthBar = m_Hud.rootVisualElement.Q<ProgressBar>("Bar_Health");

        //Debug.Log(healthBar);
    }
}
