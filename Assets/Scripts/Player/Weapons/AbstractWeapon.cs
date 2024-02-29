using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public abstract class AbstractWeapon : MonoBehaviour, IDamageSource
{
    public enum WeaponTargets
    {
        closest,
        furthest
    }

    public event Action fired;
    public event Action<float> cooldownUpdated;

    [SerializeField, EnumToggleButtons, TabGroup("General")]
    private WeaponTargets targetMode;
    [SerializeField, TabGroup("General")]
    protected int damage = 1;
    [SerializeField, SuffixLabel("shots per Second", true), TabGroup("General")]
    protected float fireRate = 0.25f;
    [SerializeField, TabGroup("General")]
    protected float range = 10;
    [SerializeField, TabGroup("General"), LabelText("Line Of Sight")]
    private bool requiresLineOfSight = false;
    [SerializeField, TabGroup("General")]
    private Transform weaponRange;

    [field:SerializeField, Required, PreviewField(ObjectFieldAlignment.Left), TabGroup("VFX & SFX"), PropertyOrder(1)]
    public Sprite sprite { get; private set; }
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1)]
    private AudioSource fireSFX;
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1)]
    private ParticleSystem particle;
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1)]
    private string weaponName;
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1), FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string localizationPath;

    private bool manuelFire = false;

    public string Name { get { return CSVLanguageFileParser.GetLangDictionary(localizationPath, SelectedLanguage.value)[$"{weaponName}_Name"]; ; } }
    public string Description { get { return CSVLanguageFileParser.GetLangDictionary(localizationPath, SelectedLanguage.value)[$"{weaponName}_Description"]; ; } }

    public string glueSourceName => weaponName;

    protected float cooldown;
    protected float activeCooldown;

    private void Awake()
    {
        cooldown = 1 / fireRate;
        activeCooldown = cooldown;

        InternalAwake();
        ShowWeaponRange(false);
    }

    protected abstract void Fire(AbstractEnemy target);

    private void Update()
    {
        var target = GetTarget();
        if (target == null && !manuelFire) return;

        activeCooldown -= Time.deltaTime;
        cooldownUpdated?.Invoke(activeCooldown / cooldown);

        if(activeCooldown <= 0 && !manuelFire)
        {
            activeCooldown = cooldown;

            #region Reset
            foreach (var anim in GetComponentsInChildren<Animator>())
            {
                anim.Rebind();
                anim.Update(0f);
            }
            if (fireSFX != null)
            {
                fireSFX.Stop();
                fireSFX.Play();
            }
            if (particle != null)
            {
                particle.Stop();
                particle.Play();
            }
            #endregion

            var pos = target.transform.position;
            pos.y = transform.position.y;
            transform.LookAt(pos);

            Fire(target);
            fired?.Invoke();
        }

        InternalUpdate();
    }

    protected void SetUpProjectile(AbstractProjectile projectile)
    {
        projectile.transform.position = transform.position;
        projectile.SetUp(damage, this);
    }

    public bool TryManuelFire(Vector3 mousePosition)
    {
        if(manuelFire && activeCooldown <= 0)
        {
            ManuelFire(mousePosition);
            activeCooldown = cooldown;
            return true;
        }

        return false;
    }
    public virtual void ManuelFire(Vector3 mousePosition) { }
    public void SetManualFire(bool state) => manuelFire = state;

    #region Internal Voids
    protected virtual void InternalUpdate() { }

    protected virtual void InternalAwake() { }

    protected virtual void InternalDrawGizmosSelected() { }
    #endregion

    #region target Selection
    protected AbstractEnemy GetTarget()
    {
        switch (targetMode)
        {
            case WeaponTargets.closest:
                return GetClosestTarget();
            case WeaponTargets.furthest:
                return GetFurthestTarget();
        }

        return null;
    }

    private AbstractEnemy GetClosestTarget()
    {
        AbstractEnemy closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in Pool.GetOutOfPools<AbstractEnemy>())
        {
            if (!enemy.enabled) continue;

            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < range && distance < closestDistance)
            {
                if (requiresLineOfSight && !LineOfSightTo(enemy)) continue;

                closest = enemy;
                closestDistance = distance;
            }
        }

        return closest;
    }

    private bool LineOfSightTo(AbstractEnemy target)
    {
        var ray = new Ray(transform.position, (target.transform.position - transform.position));
        var hit = new RaycastHit();
        int layer = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("PlayerOnlyWall");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            if (Vector3.Distance(transform.position, hit.point) > Vector3.Distance(transform.position, target.transform.position)) return true;

            return false;
        }

        return true;
    }

    private AbstractEnemy GetFurthestTarget()
    {
        AbstractEnemy closest = null;
        float highestDistance = 0;

        foreach (var enemy in Pool.GetOutOfPools<AbstractEnemy>())
        {
            if (!enemy.enabled) continue;

            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < range && distance > highestDistance)
            {
                closest = enemy;
                highestDistance = distance;
            }
        }

        return closest;
    }

    public void ShowWeaponRange(bool state)
    {
        weaponRange.gameObject.SetActive(state);
        weaponRange.transform.localScale = new Vector3(range * 2,0.5f,range * 2);
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        InternalDrawGizmosSelected();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
