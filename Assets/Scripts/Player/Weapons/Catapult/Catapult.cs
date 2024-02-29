using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : AbstractWeapon
{
    [SerializeField, TabGroup("Specifics")]
    private int bounces = 3;
    [SerializeField, TabGroup("Specifics"), SuffixLabel("per bounce", true)]
    private float bounceDistance = 3;

    protected override void Fire(AbstractEnemy target)
    {
        var projectile = Pool.Get<CatapultProjectile>();
        projectile.bounces = this.bounces + 1;
        projectile.bounceDistance = this.bounceDistance;

        base.SetUpProjectile(projectile);
        projectile.Launch(target.transform);
    }
}
