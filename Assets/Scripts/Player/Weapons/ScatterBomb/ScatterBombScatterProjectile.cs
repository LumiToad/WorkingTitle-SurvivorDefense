using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBombScatterProjectile : AbstractProjectile
{
    public override void Destroy()
    {
        Pool.Return<ScatterBombScatterProjectile>(this.gameObject);
    }

    protected override void Hit(AbstractEnemy enemy)
    {
        enemy.TakeDamage(base.damage, base.source);
    }
}
