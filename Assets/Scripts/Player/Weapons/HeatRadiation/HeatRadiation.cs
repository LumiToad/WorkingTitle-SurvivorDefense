using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatRadiation : AbstractWeapon
{
    [FoldoutGroup("References"), SerializeField]
    private DamageZone zone;
    [FoldoutGroup("References"), SerializeField]
    private Blinker blinker;
    [FoldoutGroup("References"), SerializeField]
    private List<Transform> scaleWithRadius;

    protected override void InternalAwake()
    {
        zone.damage = base.damage;
        zone.cooldown = 1 / base.fireRate;

        zone.overrideSource = this;
        UpdateScale();
    }

    protected override void Fire(AbstractEnemy target)
    {
       // blinker.Blink();
    }

    private void UpdateScale()
    {
        foreach(Transform t in scaleWithRadius)
        {
            t.localScale += new Vector3(base.range - 1, base.range - 1, base.range - 1);
        }
    }
}
