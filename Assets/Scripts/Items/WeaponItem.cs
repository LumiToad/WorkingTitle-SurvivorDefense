using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : AbstractItem
{
    [Title("WeaponItem")]

    [SerializeField]
    private AbstractWeapon WeaponToEarn;

    [SerializeField, FoldoutGroup("References")]
    private Image Icon;

    private void Awake()
    {
        Icon.sprite = WeaponToEarn.sprite;
    }

    protected override void PickedUp(Player player)
    {
        WeaponManager.GetWeapon(WeaponToEarn.gameObject.name, player);
        Destroy(this.gameObject);
    }
}
