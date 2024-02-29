using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : AbstractWeapon
{
    [SerializeField, TabGroup("Specifics")]
    private float puddleSize;
    [SerializeField,TabGroup("Specifics"), SuffixLabel("damages per second", true)]
    private float poisonDamageRate;
    [SerializeField, TabGroup("Specifics"), SuffixLabel("seconds", true)]
    private float puddleDuration;
    

    protected override void Fire(AbstractEnemy target)
    {
        var projectile = Pool.Get<PoisonProjectile>();
        projectile.puddleDamageRate = poisonDamageRate;
        projectile.puddleScale = new Vector3(puddleSize * 2, 0.1f, puddleSize * 2);
        projectile.puddleDuration = this.puddleDuration;

        base.SetUpProjectile(projectile);

        projectile.Launch(target.transform.position);
    }
}
