using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeaponsUI : MonoBehaviour
{
    [SerializeField, AssetsOnly, FoldoutGroup("References")]
    private WeaponSlotUI slotTemplate;

    [SerializeField, ChildGameObjectsOnly, FoldoutGroup("References")]
    private Transform weaponSlotHolder;

    [SerializeField, ChildGameObjectsOnly, FoldoutGroup("References")]
    private UIReminder leftInfo;

    [SerializeField, ChildGameObjectsOnly, FoldoutGroup("References")]
    private UIReminder rightInfo;

    [SerializeField, AssetsOnly, FoldoutGroup("References")]
    private PlaceVFX placeVFXTemplate;

    private List<AbstractWeapon> addQueue = new List<AbstractWeapon>();

    private void Awake()
    {
        foreach(var weapon in GetComponentsInChildren<WeaponSlotUI>())
        {
            Destroy(weapon.gameObject);
        }

        StartCoroutine(AddWeaponLoop());
    }

    public void Setup(int slots)
    {
        for (int i = 0; i < slots; i++)
        {
            AddWeaponSlot();
        }
    }

    private void AddWeaponSlot()
    {
        var slot = Instantiate(slotTemplate);
        slot.transform.SetParent(weaponSlotHolder, false);

        leftInfo.transform.SetAsFirstSibling();
        rightInfo.transform.SetAsLastSibling();
    }

    public void AddWeapon(AbstractWeapon toShow)
    {
        addQueue.Add(toShow);
    }

    public void RemoveWeapon(AbstractWeapon toRemove)
    {
        foreach (var slot in weaponSlotHolder.transform.GetComponentsInChildren<WeaponSlotUI>())
        {
            if (slot.IsShowing(toRemove))
            {
                slot.RemoveWeapon();
            }
        }
    }

    public void PlayWeaponPlaceVFX(AbstractWeapon toPlace, Vector3 from, Vector3 target, float duration)
    {
        foreach (var slot in weaponSlotHolder.transform.GetComponentsInChildren<WeaponSlotUI>())
        {
            if (slot.IsShowing(toPlace))
            {
                RemoveWeapon(toPlace);

                var vfx = Instantiate(placeVFXTemplate);
                vfx.transform.position = from;
                vfx.Place(duration, target);
            }
        }
    }

    public void SelectWeapon(AbstractWeapon toSelect)
    {
        foreach (var weapon in weaponSlotHolder.transform.GetComponentsInChildren<WeaponSlotUI>())
        {
            if (weapon.IsShowing(toSelect))
            {
                weapon.Select();
            }
            else
            {
                weapon.DeSelect();
            }
        }
    }

    public void HighlightDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                leftInfo.Highlight();
                break;
            case Direction.Right:
                rightInfo.Highlight();
                break;
        }

        leftInfo.StopBlinking();
        rightInfo.StopBlinking();
    }

    public void DeSelectWeapon()
    {
        foreach (var weapon in weaponSlotHolder.transform.GetComponentsInChildren<WeaponSlotUI>())
        {
            weapon.DeSelect();
        }
    }

    public void Lock()
    {
        leftInfo.Lock();
        rightInfo.Lock();
    }

    public void UnLock()
    {
        leftInfo.UnLock();
        rightInfo.UnLock();
    }

    private IEnumerator AddWeaponLoop()
    {
        while (true)
        {
            for(int i = 0;i < addQueue.Count;i++)
            {
                var toShow = addQueue[i];
                foreach (var slot in weaponSlotHolder.GetComponentsInChildren<WeaponSlotUI>())
                {
                    if (slot.TryShow(toShow))
                    {
                        addQueue.Remove(toShow);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
