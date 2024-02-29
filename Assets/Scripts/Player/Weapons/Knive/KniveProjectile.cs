using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class KniveProjectile : AbstractProjectile
{
    [Title("Knive Projectile")]

    [SerializeField]
    private int piercings = 0;

    private int startingPiercings = -1;

    private void Start()
    {
        
    }

    protected override void Hit(AbstractEnemy enemy)
    {
        enemy.TakeDamage(damage, base.source);

        piercings--;

        if(piercings < 0)
        {
            Destroy();
        }
    }

    public override void Destroy()
    {
        Pool.Return<KniveProjectile>(this.gameObject);
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
