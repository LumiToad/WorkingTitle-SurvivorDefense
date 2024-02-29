using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : AbstractWeapon
{
    [SerializeField, MinValue(0), TabGroup("Specifics")]
    private int bounces;

    protected override void Fire(AbstractEnemy target)
    {
        var bullet = Pool.Get<Spark>();
        bullet.bounces = new IntStat(bounces);

        SetUpProjectile(bullet);

        bullet.Shoot(target.transform.position - transform.position);
    }
}
