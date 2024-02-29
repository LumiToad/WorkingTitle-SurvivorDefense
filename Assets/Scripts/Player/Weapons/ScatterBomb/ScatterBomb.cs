using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBomb : AbstractWeapon
{
    [SerializeField, TabGroup("Specifics")]
    private float explosionRange;
    [SerializeField, TabGroup("Specifics")]
    private float scatterCount;
    [SerializeField, TabGroup("Specifics")]
    private float rangeUntilExplosion;

    protected override void Fire(AbstractEnemy target)
    {
        var projectile = Pool.Get<ScatterBombProjectile>();
        SetUpProjectile(projectile);
        projectile.transform.position = transform.position;

        projectile.explosionRange = explosionRange;
        projectile.scatterCount = scatterCount;


        var direction = target.transform.position - transform.position;
        projectile.Launch(direction.normalized * rangeUntilExplosion + transform.position);
    }
}
