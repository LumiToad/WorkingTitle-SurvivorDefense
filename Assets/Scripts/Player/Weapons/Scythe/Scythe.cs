using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : AbstractWeapon
{
    [SerializeField, TabGroup("Specifics"), SuffixLabel("seconds")]
    private float ScytheSwingDuration = 1;

    [SerializeField, FoldoutGroup("References")]
    private Animator scythe;

    protected override void InternalAwake()
    {
        scythe.transform.localScale = new Vector3();

        foreach(var zone in GetComponentsInChildren<DamageZone>())
        {
            zone.damage = base.damage;
            zone.overrideSource = this;
        }
        base.InternalAwake();
    }

    protected override void Fire(AbstractEnemy target)
    {
        scythe.SetFloat("Speed", 1 / ScytheSwingDuration);
        scythe.Play("ScytheSwing");
    }
}
