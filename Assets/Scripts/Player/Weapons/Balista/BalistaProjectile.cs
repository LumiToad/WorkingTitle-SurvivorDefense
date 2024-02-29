using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalistaProjectile : AbstractWeapon
{
    protected override void Fire(AbstractEnemy target)
    {
        var knive = Pool.Get<Balista>();
        base.SetUpProjectile(knive);
        knive.Shoot(target.transform.position - transform.position);
    }
}
