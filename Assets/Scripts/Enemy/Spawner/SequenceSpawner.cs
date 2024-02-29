using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class SequenceSpawner : Spawner
{
    [Title("Sequence")]

    [SerializeField, ProgressBar(0, "sequenceDuration"), HideLabel]
    private float progress = 0;

    [SerializeField, SuffixLabel("In Seconds", true), OnValueChanged("OnDurationChanged")]
    private float sequenceDuration;
    [SerializeField]
    private bool cycling;

    [SerializeField, ListDrawerSettings(CustomAddFunction = "AddStep")]
    private List<SequenceStep> steps;

    [OnInspectorInit]
    private void init()
    {
        base.hideSpawnRateInInspector = true;
        base.hideEnemyTemplate = true;
        base.hideProgress = true;
    }

    protected override void TickDown(List<AbstractEnemy> localSpawns)
    {
        if(progress > sequenceDuration)
        {
            if (!cycling) return;
            progress = 0;
        }

        progress += Time.fixedDeltaTime;

        foreach (var sequence in steps)
        {
            if (sequence.TickDownToSpawn(progress, localSpawns))
            {
                base.Spawn(sequence.enemyTemplate);
            }
        }
    }


    //called by Odin
    private void OnDurationChanged()
    {
        foreach(var sequence in steps)
        {
            sequence.duration = sequenceDuration;
        }
    }

    //called by Odin
    private SequenceStep AddStep()
    {
        var sequence = new SequenceStep();
        sequence.duration = sequenceDuration;

        return sequence;
    }
}

[System.Serializable]
public class SequenceStep
{
    [SerializeField, ProgressBar(0f, "GetCooldown"), HideLabel]
    private float timer = 0;

    [SerializeField, AssetsOnly, InlineEditor]
    public AbstractEnemy enemyTemplate;
    [SerializeField, MinMaxSlider(0, "duration", true)]
    private Vector2 activeDuration;
    [SerializeField]
    private SpawnCooldown spawnRate;
    [HideInInspector]
    public float duration = 0;

    //saved for Odin
    private List<AbstractEnemy> odinLastLocalSpawns;

    public bool TickDownToSpawn(float progress, List<AbstractEnemy> localSpawns)
    {
        if (progress < activeDuration.x || progress > activeDuration.y) return false;

        timer += Time.fixedDeltaTime;

        odinLastLocalSpawns = localSpawns;

        if (timer > spawnRate.GetUpdatedCooldown(localSpawns))
        {
            timer = 0;
            spawnRate.Refresh();

            return true;
        }

        return false;
    }

    //Used by Odin
    private float GetCooldown() => spawnRate.GetUpdatedCooldown(odinLastLocalSpawns);
}
