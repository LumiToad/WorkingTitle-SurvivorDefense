using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [SerializeField, Required, ShowInInspector]
    private Image fill;

    private float dashCooldown = 0;

    public void DashCooldown(float dashCooldown)
    {
        this.dashCooldown = dashCooldown;
        fill.fillAmount = 1;
    }

    private void Update()
    {
        if (fill.fillAmount > 0.0f)
        {
            fill.fillAmount -= (1.0f / dashCooldown) * Time.deltaTime;
        }
    }
}
