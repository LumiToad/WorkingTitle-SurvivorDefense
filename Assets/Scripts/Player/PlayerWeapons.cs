using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerWeapons : MonoBehaviour, IWeaponHolder
{
    [SerializeField]
    private int abilitySlots = 4;
    [SerializeField]
    private int overflowSlots = 1;

    [SerializeField, SuffixLabel ("in seconds", true)]
    private float placeTime = 0.5f;

    [FoldoutGroup("References"), SerializeField]
    private Tower towerTemplate;
    [FoldoutGroup("References"), SerializeField]
    private Transform weaponHolder;

    private AbstractWeapon defaultWeapon = null;
    private List<AbstractWeapon> activeWeapons;
    private List<AbstractWeapon> overflowWeapons;

    private WeaponUI ui;
    private WeaponUnlockCanvas unlockUi;
    private AbstractWeapon selectedWeapon = null;

    private List<AbstractWeapon> AddQueue = new List<AbstractWeapon>();

    public void Awake()
    {
        activeWeapons = new List<AbstractWeapon>();
        overflowWeapons = new List<AbstractWeapon>();
        ui = GetComponentInChildren<WeaponUI>();
        unlockUi = GetComponentInChildren<WeaponUnlockCanvas>();
    }

    private void Start()
    {
        StartCoroutine(AddWeaponLoop());
        ui.SetupActiveSlots(abilitySlots);
    }

    public void WeaponUpdate(PlayerInputActions input, IntStat towerHP)
    {
        TryPlaceTower(input, towerHP);
        TryInputWeaponSelect(input);
        TryManuelFire(input);
    }

    private bool TryPlaceTower(PlayerInputActions input, IntStat towerHP)
    {
        if (!input.Player.BuildTower.WasPerformedThisFrame()) return false;

        if (Time.timeScale == 0) return false;
        if (selectedWeapon == null) return false;
        if (activeWeapons.Count < 1) return false;

        var ray = Camera.main.ScreenPointToRay(input.Player.MousePosition.ReadValue<Vector2>());
        var hit = new RaycastHit();
        int layer = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("NoPlacement");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                return false;
            }

            var noPlacement = hit.transform.GetComponent<Blinker>();
            if(noPlacement != null)
            {
                noPlacement.Blink();
                return false;
            }

            var tower = GameObject.Instantiate(towerTemplate);
            tower.transform.position = hit.point;
            tower.SetUp(placeTime, towerHP);

            ui.PlayActiveWeaponPlaceVFX(selectedWeapon, transform.position, hit.point, placeTime);

            WeaponManager.TransferWeapon(selectedWeapon, this, tower);

            if(activeWeapons.Count > 0)
            {
                TrySelect(activeWeapons.Last());
            }

            if (overflowWeapons.Count > 0)
            {
                StartCoroutine(AddActiveWeapon(overflowWeapons.First()));
                ui.RemoveOverflowWeapon(overflowWeapons.First());
                overflowWeapons.Remove(overflowWeapons.First());
            }

            return true;
        }

        return false;
    }

    private bool TryInputWeaponSelect(PlayerInputActions input)
    {
        if (activeWeapons.Count < 1) return false;

        var index = 0;
        if(selectedWeapon != null)
        {
            index = activeWeapons.IndexOf(selectedWeapon);
        }

        var direction = input.Player.ScrollTowerSelection.ReadValue<Vector2>().y;

        if (input.Player.SelectLeftTower.WasPerformedThisFrame()) direction--;
        if (input.Player.SelectRightTower.WasPerformedThisFrame()) direction++;

        if (direction == 0) return false;
        if (direction > 0)
        {
            ui.HighlightDirection(Sirenix.Utilities.Direction.Right);
            index++;
        }
        if (direction < 0)
        {
            ui.HighlightDirection(Sirenix.Utilities.Direction.Left);
            index--;
        }

        if (index < 0) index = activeWeapons.Count - 1;
        if (index > activeWeapons.Count - 1) index = 0;

        return TrySelect(activeWeapons[index]);
    }

    private bool TryManuelFire(PlayerInputActions input)
    {
        if (defaultWeapon == null) return false;
        if (!input.Player.Fire.IsPressed()) return false;

        var ray = Camera.main.ScreenPointToRay(input.Player.MousePosition.ReadValue<Vector2>());
        var hit = new RaycastHit();
        int layer = 1 << LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            var dir = (ray.origin - defaultWeapon.transform.position).normalized;
            var dist = defaultWeapon.transform.position.y - hit.point.y;

            Debug.DrawLine(ray.origin, hit.point + dir * dist, Color.white, 1000);

            hit.point += dir * dist;

            return defaultWeapon.TryManuelFire(hit.point);
        }
        return false;
    }

    public void DeActivate()
    {
        selectedWeapon = null;
        ui.DeSelectWeapon();
        ui.LockWeapons();
    }

    public void Activate()
    {
        ui.UnlockWeapons();
    }

    [Button]
    private void DebugGetWeapon()
    {
        if (Application.isPlaying)
        {
            WeaponManager.GetRandomWeapon(this);
        }
    }
    private bool TrySelect(AbstractWeapon weapon)
    {
        if (!activeWeapons.Contains(weapon) || weapon == selectedWeapon) return false;

        selectedWeapon = weapon;
        ui.SelectWeapon(weapon);

        return true;
    }

    private IEnumerator AddWeaponLoop()
    {
        while (true)
        {
            while(AddQueue.Count > 0)
            {
                var weapon = AddQueue.First();
                var overflow = activeWeapons.Count >= abilitySlots;

                unlockUi.Show(weapon, overflow);
                yield return new WaitForSeconds(1.25f);

                if (!overflow)
                {
                    yield return StartCoroutine(AddActiveWeapon(weapon));
                }
                else
                {
                    yield return StartCoroutine(AddOverFlowWeapon(weapon));
                }

                AddQueue.Remove(weapon);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator AddActiveWeapon(AbstractWeapon weapon)
    {
        if(activeWeapons.Count < abilitySlots)
        {
            activeWeapons.Add(weapon);
        }

        weapon.gameObject.SetActive(true);
        ui.AddActiveWeapon(weapon);

        yield return new WaitForSeconds(0.75f);
    }

    private IEnumerator AddOverFlowWeapon(AbstractWeapon weapon)
    {
        if (overflowWeapons.Count < overflowSlots)
        {
            ui.AddOverflowWeapon(weapon);
            overflowWeapons.Add(weapon);
        }

        yield return new WaitForSeconds(0.75f);

        QuickHints.ShowOnce("OverflowHint", abilitySlots.ToString(), overflowSlots.ToString());
    }

    #region Ability Holder
    public void RemoveWeapon(AbstractWeapon weapon)
    {
        activeWeapons.Remove(weapon);
        ui.RemoveActiveWeapon(weapon);

        if(selectedWeapon == weapon)
        {
            selectedWeapon = null;
        }
    }

    public bool TryAddWeapon(AbstractWeapon weapon)
    {
        weapon.transform.SetParent(weaponHolder);
        weapon.transform.position = transform.position;

        if (defaultWeapon == null)
        {
            defaultWeapon = weapon;
            weapon.SetManualFire(true);
            ui.SetUpDefaultSlot(defaultWeapon);
            return true;
        }

        weapon.gameObject.SetActive(false);
        AddQueue.Add(weapon);
        return true;
    }

    public AbstractWeapon GetLatest() => activeWeapons.Last();
    #endregion
}
