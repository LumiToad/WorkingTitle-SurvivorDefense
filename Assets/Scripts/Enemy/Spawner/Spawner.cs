using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Title("Spawner")]

    [SerializeField]
    private bool debugAutoEnable = false;

    [HideIf("hideProgress")]
    [SerializeField,ProgressBar(0, "GetCooldown"), HideLabel]
    protected float timer = 0;

    [HideIf("hideEnemyTemplate")]
    [SerializeField, Required, AssetsOnly, PropertyOrder(0)]
    protected AbstractEnemy EnemyTemplate;

    [HideIf("hideSpawnSize")]
    [SerializeField, PropertyOrder(0)]
    private Vector2 spawnArea = new Vector2(5,5);

    [SerializeField, FoldoutGroup("bonus power options")]
    private int bonusHealth = 0;
    [SerializeField, FoldoutGroup("bonus power options")]
    private int bonusSpeed = 0;
    [SerializeField, FoldoutGroup("bonus power options")]
    private int bonusDamage = 0;

    [HideIf("hideSpawnRateInInspector")]
    [SerializeField, PropertyOrder(0)]
    protected SpawnCooldown spawnRate;

    protected bool hideSpawnRateInInspector = false;
    protected bool hideEnemyTemplate = false;
    protected bool hideSpawnSize = false;
    protected bool hideProgress = false;

    private Color gizmosColor = Color.red;

    private List<AbstractEnemy> localSpawns = new List<AbstractEnemy>();

    private void Awake()
    {
        DeActivate();
        if (debugAutoEnable)
        {
            Activate();
        }
    }

    private void FixedUpdate()
    {
        TickDown(localSpawns);
    }

    protected virtual void Spawn(AbstractEnemy toSpawn)
    {
        if (toSpawn == null) return;

        var enemy = Pool.Get<AbstractEnemy>(toSpawn.gameObject.name);
        if (enemy == null) return;

        enemy.transform.position = GetRandomSpawnPoint();
        enemy.transform.SetParent(transform);

        enemy.GainBonusDamage(bonusDamage);
        enemy.GainBonusHealth(bonusHealth);
        enemy.GainBonusSpeed(bonusSpeed);

        enemy.SetUp();

        timer = 0;
        spawnRate.Refresh();

        localSpawns.Add(enemy);
        enemy.died += (enemy_) => { localSpawns.Remove(enemy_); };
    }

    protected virtual void TickDown(List<AbstractEnemy> localSpawns)
    {
        timer += Time.fixedDeltaTime;
        if (timer >= spawnRate.GetUpdatedCooldown(localSpawns))
        {
            Spawn(EnemyTemplate);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        var result = new Vector3();

        result += transform.position;
        result.x += Random.Range(spawnArea.x / 2 * -1, spawnArea.x / 2);
        result.z += Random.Range(spawnArea.y / 2 * -1, spawnArea.y / 2);

        return result;
    }

    public virtual void Activate()
    {
        this.enabled = true;
        gizmosColor = Color.green;
    }

    public void DeActivate()
    {
        this.enabled = false;
        gizmosColor = Color.red;
    }

    //Used by Odin
    private float GetCooldown() => spawnRate.GetUpdatedCooldown(localSpawns);

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0, spawnArea.y));
    }
}
