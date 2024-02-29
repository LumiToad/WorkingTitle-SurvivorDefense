using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSpawner : Spawner
{
    [Header("Instant"), SerializeField]
    private int count;
    [SerializeField, SuffixLabel("seconds")]
    private float delay;

    [OnInspectorInit]
    private void init()
    {
        base.hideSpawnRateInInspector = true;
        base.hideProgress = true;
    }

    public override void Activate()
    {
        StartCoroutine(delayedSpawn());
        base.Activate();
    }

    private IEnumerator delayedSpawn()
    {
        yield return new WaitForSeconds(delay);

        base.spawnRate = new SpawnCooldown();

        for (int i = 0;i < count; i++)
        {
            if(i % 5 == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            base.Spawn(base.EnemyTemplate);
        }

    }
}
