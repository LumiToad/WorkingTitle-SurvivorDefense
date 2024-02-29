using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverflowWeaponsUI : MonoBehaviour
{
    [SerializeField, AssetsOnly, FoldoutGroup("References")]
    private WeaponSlotUI overFlowSlotTemplate;

    [SerializeField, ChildGameObjectsOnly, FoldoutGroup("References")]
    private Transform Slotholder;

    public void AddWeapon(AbstractWeapon toShow)
    {
        var slot = Instantiate(overFlowSlotTemplate);
        slot.transform.SetParent(Slotholder, false);
        slot.TryShow(toShow);
    }

    public void RemoveWeapon(AbstractWeapon toRemove)
    {
        foreach (var slot in Slotholder.transform.GetComponentsInChildren<WeaponSlotUI>())
        {
            if (slot.IsShowing(toRemove))
            {
                slot.RemoveWeapon();
            }
        }
    }
}
