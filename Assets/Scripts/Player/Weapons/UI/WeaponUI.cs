using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class WeaponUI : MonoBehaviour
{
    private ActiveWeaponsUI active;
    private OverflowWeaponsUI overflow;
    private DefaultWeaponSlot defaultSlot;

    private void Awake()
    {
        active = GetComponentInChildren<ActiveWeaponsUI>();
        overflow = GetComponentInChildren<OverflowWeaponsUI>();
        defaultSlot = GetComponentInChildren<DefaultWeaponSlot>();

        /*
        var canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 5;
        */
    }

    public void AddActiveWeapon(AbstractWeapon weapon) => active.AddWeapon(weapon);
    public void RemoveActiveWeapon(AbstractWeapon weapon) => active.RemoveWeapon(weapon);
    public void SelectWeapon(AbstractWeapon weapon) => active.SelectWeapon(weapon);
    public void DeSelectWeapon() => active.DeSelectWeapon();
    public void PlayActiveWeaponPlaceVFX(AbstractWeapon weapon, Vector3 from, Vector3 target, float duration) => active.PlayWeaponPlaceVFX(weapon, from, target, duration);
    public void LockWeapons() => active.Lock();
    public void UnlockWeapons() => active.UnLock();

    public void AddOverflowWeapon(AbstractWeapon weapon) => overflow.AddWeapon(weapon);
    public void RemoveOverflowWeapon(AbstractWeapon weapon) => overflow.RemoveWeapon(weapon);

    public void HighlightDirection(Direction dir) => active.HighlightDirection(dir);

    public void SetUpDefaultSlot(AbstractWeapon weapon) => defaultSlot.SetUp(weapon);
    public void SetupActiveSlots(int slots) => active.Setup(slots);

}
