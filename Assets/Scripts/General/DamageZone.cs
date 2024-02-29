using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamageZone : MonoBehaviour, IDamageSource
{
    [Title("DamageZone")]

    [SerializeField]
    public int damage = 1;

    [SerializeField]
    private bool collisionEnter;
    [SerializeField]
    private bool collisionExit;

    [SerializeField]
    private bool stay = true;
    [SerializeField, SuffixLabel("seconds", true), ShowIf("stay")]
    public float cooldown = 0.1f;

    private float activeCooldown;

    private List<IDamageAble> inRange = new List<IDamageAble>();
    public IDamageSource overrideSource = null;
    string IDamageSource.glueSourceName => "GlueSourceName";

    private void OnTriggerEnter(Collider other)
    {
        var target = other.transform.GetComponent<IDamageAble>();
        if (target == null || target.gameObject.layer == this.gameObject.layer) return;
        inRange.Add(target);

        if (!collisionEnter) return;
        Hit(target);
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.transform.GetComponent<IDamageAble>();
        if (target == null || target.gameObject.layer == this.gameObject.layer) return;
        inRange.Remove(target);

        if (!collisionExit) return;
        Hit(target);
    }

    protected virtual void Hit(IDamageAble target)
    {
        if (target != null)
        {
            if(overrideSource != null)
            {
                target.TakeDamage(damage, overrideSource);
            }
            else
            {
                target.TakeDamage(damage, this);
            }
        }
    }

    private void HitEverythingInRange()
    {
        while (inRange.Contains(null))
        {
            inRange.Remove(null);
        }

        var toRemove = new List<GameObject>();

        foreach (var target in inRange)
        {
            if (!target.gameObject.activeInHierarchy)
            {
                toRemove.Add(target.gameObject);
                continue;
            }

            Hit(target);
        }

        foreach (var ob in toRemove)
        {
            inRange.Remove(ob.GetComponent<IDamageAble>());
        }
    }

    private void Update()
    {
        if (!stay) return;

        activeCooldown -= Time.deltaTime;

        if (activeCooldown < 0)
        {
            activeCooldown = cooldown;

            HitEverythingInRange();
        }
    }
}
