using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultWeaponSlot : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private Image icon;
    [SerializeField, FoldoutGroup("References")]
    private Image fill;

    public void SetUp(AbstractWeapon toDisplay)
    {
        icon.sprite = toDisplay.sprite;

        toDisplay.cooldownUpdated += UpdateCooldownDisplay;
    }

    private void UpdateCooldownDisplay(float percent)
    {
        fill.fillAmount = percent;
    }
}
