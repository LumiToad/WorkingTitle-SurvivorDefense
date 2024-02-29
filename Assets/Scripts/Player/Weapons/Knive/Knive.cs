using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knive : AbstractWeapon
{
    protected override void Fire(AbstractEnemy target)
    {
        var knive = Pool.Get<KniveProjectile>();
        base.SetUpProjectile(knive);
        knive.Shoot(target.transform.position - transform.position);
    }

    public override void ManuelFire(Vector3 mousePosition)
    {
        var knive = Pool.Get<KniveProjectile>();
        base.SetUpProjectile(knive);
        knive.Shoot(mousePosition - transform.position);
    }
}
