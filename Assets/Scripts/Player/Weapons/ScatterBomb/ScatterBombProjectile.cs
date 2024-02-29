using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBombProjectile : AbstractProjectile
{
    public float explosionRange {private get; set; }
    public float scatterCount { private get; set; }

    public override void Destroy()
    {
        StartCoroutine(delayedDestroy());
    }

    private IEnumerator delayedDestroy()
    {
        for (int i = 0; i < scatterCount; i++)
        {
            Vector2 raw = Random.insideUnitCircle * explosionRange;
            Vector3 target = transform.position + new Vector3(raw.x, -1, raw.y);

            var scatter = Pool.Get<ScatterBombScatterProjectile>();
            scatter.transform.position = transform.position;
            scatter.SetUp(base.damage, base.source);
            scatter.Shoot(target - transform.position);

            if(i % 3 == 0)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        Pool.Return<ScatterBombProjectile>(this.gameObject);
    }

    protected override void Hit(AbstractEnemy enemy)
    {
       
    }

    protected override bool LaunchUpdate(float percent)
    {
        if(percent > 0.5f)
        {
            Destroy();
            return false;
        }

        return base.LaunchUpdate(percent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
