using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPuddle : DamageZone, IPoolable
{
    public void OnReturnedToPool()
    {
    }

    public void OnTakenFromPool()
    {
    }

    public void SetUp(float puddleDuration, IDamageSource source)
    {
        base.overrideSource = source;
        StartCoroutine(delay(puddleDuration));
    }

    private IEnumerator delay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Pool.Return<PoisonPuddle>(this.gameObject);
    }
}
