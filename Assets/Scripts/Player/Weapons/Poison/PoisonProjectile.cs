using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PoisonProjectile : AbstractProjectile
{
    [HideInInspector]
    public Vector3 puddleScale;
    [HideInInspector]
    public float puddleDamageRate;
    [HideInInspector]
    public float puddleDuration;

    public override void Destroy()
    {
        Pool.Return<PoisonProjectile>(this.gameObject);
    }

    protected override void Hit(AbstractEnemy enemy)
    {
    }

    protected override void LaunchEnd()
    {
        var puddle = Pool.Get<PoisonPuddle>();
        puddle.damage = base.damage;
        puddle.cooldown = 1 / puddleDamageRate;
        puddle.transform.localScale = puddleScale;

        var ray = new Ray(transform.position, Vector3.down * 10);
        var hit = new RaycastHit();
        int layer = 1 << LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            puddle.transform.position = hit.point + new Vector3(0, puddleScale.y, 0);
        }
        else
        {
            puddle.transform.position = transform.position;
        }

        puddle.SetUp(puddleDuration, base.source);

        base.LaunchEnd();

        Destroy();
    }
}
