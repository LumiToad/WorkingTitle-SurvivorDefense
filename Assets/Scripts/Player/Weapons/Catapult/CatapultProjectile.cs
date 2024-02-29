using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultProjectile : AbstractProjectile
{
    [HideInInspector]
    public int bounces = 0;
    [HideInInspector]
    public float bounceDistance = 0;
    private int startingBounces = 0;

    public override void Destroy()
    {
        Pool.Return<CatapultProjectile>(this.gameObject);
    }

    protected override void Hit(AbstractEnemy enemy)
    {
        enemy.TakeDamage(base.damage, base.source);
    }

    protected override void LaunchEnd()
    {
        base.Launch(transform.position + transform.forward * bounceDistance);

        bounces--;
        if(bounces <= 0)
        {
            Destroy();
        }
    }

    protected override void InternalOnReturnedToPool()
    {
        if (startingBounces < 0) return;
        bounces = startingBounces;
    }

    protected override void InternalOnTakenFromPool()
    {
        if (startingBounces > 0) return;
        startingBounces = bounces;
    }
}
