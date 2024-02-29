using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : AbstractWeapon
{
    [Title("Shotgun")]

    [SerializeField,FoldoutGroup("References")]
    private List<Transform> barrels;

    protected override void Fire(AbstractEnemy target)
    {
        foreach(var barrel in barrels)
        {
            var bullet = Pool.Get<ShotGunBullet>();
            base.SetUpProjectile(bullet);

            bullet.Shoot(barrel.transform.position - transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var barrel in barrels)
        {
            Gizmos.DrawLine(transform.position, barrel.transform.position);
        }
    }
}
