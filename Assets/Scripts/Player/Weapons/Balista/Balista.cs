using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balista : AbstractProjectile, IPoolable
{
    [Title("Knive Projectile")]

    [SerializeField]
    private int piercings = 0;

    private int startingPiercings = -1;

    protected override void Hit(AbstractEnemy enemy)
    {
        enemy.TakeDamage(damage, base.source);

        piercings--;

        if (piercings < 0)
        {
            Destroy();
        }
    }

    public override void Destroy()
    {
        Pool.Return<BalistaProjectile>(this.gameObject);
    }

    protected override void InternalOnReturnedToPool()
    {
        if (startingPiercings < 0) return;
        piercings = startingPiercings;
    }

    protected override void InternalOnTakenFromPool()
    {
        if (startingPiercings > 0) return;
        startingPiercings = piercings;
    }
}
