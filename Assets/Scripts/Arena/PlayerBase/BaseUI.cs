using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private Healthbar healthbar;

    public void SetHealthPercent(float percent) => healthbar.SetTo(percent);

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
