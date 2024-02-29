using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : AbstractItem
{
    [Title("HealthItem")]
    [SerializeField]
    private int value;

    protected override void PickedUp(Player player)
    {
        player.RestoreHealth(value);
        Destroy(this.gameObject);
    }
}
