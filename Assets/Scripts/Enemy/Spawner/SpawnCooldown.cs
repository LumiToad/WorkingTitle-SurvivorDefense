using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnCooldown
{
    enum smartMode
    {
        local,
        global
    }

    [SerializeField]
    private bool range;

    [SerializeField]
    private bool smart;

    [HideIf("range")]
    [SerializeField, SuffixLabel("Enemies per second", true)]
    private float spawnRate;

    [ShowIf("range")]
    [InfoBox("Error: min value must be smaller than max value.", InfoMessageType = InfoMessageType.Error, VisibleIf = "WithinLegalRange")]
    [SerializeField, SuffixLabel("Enemies per second", true)]
    private float minSpawnRate;

    [ShowIf("range")]
    [SerializeField, SuffixLabel("Enemies per second", true)]
    private float maxSpawnRate;

    [Header("Smart")]
    [ShowIf("smart")]
    [SerializeField]
    [EnumToggleButtons]
    private smartMode mode = smartMode.local;

    [ShowIf("smart")]
    [SerializeField]
    private float targetEnemies;

    [ShowIf("smart")]
    [MinValue(0)]
    [SerializeField, SuffixLabel("bonus speed per missing % to target enemies", true)]
    private float smartBoost = 1;

    [ShowIf("smart")]
    [DisableInPlayMode]
    [DisableInEditorMode]
    [SerializeField]
    private float boostPercent;

    private float cooldown = 0;

    public float GetUpdatedCooldown(List<AbstractEnemy> spawnedEnemies)
    {
        var result = cooldown;

        if (smart && Application.isPlaying)
        {
            boostPercent = 1 - (GetSmartEnemyCount(spawnedEnemies) / (float)targetEnemies);
            result -= boostPercent * smartBoost;
        }

        return result;
    }

    public float Refresh() => cooldown = cooldown = range ? 1 / (Random.Range(minSpawnRate, maxSpawnRate)) : 1 / spawnRate;

    private int GetSmartEnemyCount(List<AbstractEnemy> spawnedEnemies)
    {
        switch (mode)
        {
            case smartMode.local:
                if(spawnedEnemies == null)
                {
                    return 0;
                }
                return spawnedEnemies.Count;
            case smartMode.global:
                return Pool.GetOutOfPoolCount<AbstractEnemy>();
        }
        return 0;
    }

    //used by Odin
    private bool WithinLegalRange() => minSpawnRate > maxSpawnRate;
}
