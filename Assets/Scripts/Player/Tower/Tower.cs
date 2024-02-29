using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour, IWeaponHolder//IDamageAble
{
    private AbstractWeapon weapon;
    [SerializeField, FoldoutGroup("References")]
    private Transform weaponHolder;
    [SerializeField, FoldoutGroup("References")]
    private Dissolve dissolve;

    //private Healthbar healthbar;

    //private IntStat health;

    private void Awake()
    {
       // healthbar = GetComponentInChildren<Healthbar>();
    }

    public void SetUp(float delay, IntStat towerHealth) 
    {
        StartCoroutine(SetUpInternal(delay));
        //this.health = new IntStat(towerHealth);
    }
    
    private IEnumerator SetUpInternal(float delay)
    {
        yield return new WaitForSeconds(delay);
        dissolve.manifested += EnableWeapon;
        dissolve.Begin(dissolveMode.manifest);
    }

    public void RemoveWeapon(AbstractWeapon weapon) => weapon = null;

    public bool TryAddWeapon(AbstractWeapon weapon)
    {
        if (this.weapon != null) return false;
        this.weapon = weapon;

        weapon.transform.SetParent(weaponHolder);
        weapon.transform.position = weaponHolder.position;
        weapon.gameObject.SetActive(false);

        return true;
    }

    private void EnableWeapon()
    {
        weapon?.gameObject.SetActive(true);
        GetComponentInChildren<TowerUI>().Show(weapon);
    }

    /*
    public void TakeDamage(int damage)
    {
        health -= damage;

        healthbar.SetTo(health.totalValue / (float)health.value);

        if (health <= 0)
        {
            TowerDeath();
        }
    }
    */

    private void TowerDeath()
    {
        dissolve.dissolved += () => { Destroy(this.gameObject); };

        dissolve.Reset();
        dissolve.Begin(dissolveMode.dissolve);
        RemoveWeapon(weapon);
    }
}
