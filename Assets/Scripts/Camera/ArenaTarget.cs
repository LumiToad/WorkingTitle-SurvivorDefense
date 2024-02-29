using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class ArenaScrollingTargetNew : BaseScrollingTarget
{
    [SerializeField, Required]
    public float blendTime;

    protected override void Awake()
    {
        base.Awake();
        CalculatedBlendTime = blendTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(transform.position, 1.0f);
    }
}