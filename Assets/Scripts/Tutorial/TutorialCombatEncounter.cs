using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCombatEncounter : MonoBehaviour
{
    private List<Spawner> spawners;
    private List<EnemyPath> paths;

    private void Awake()
    {
        spawners = new List<Spawner>();
        foreach(var spawner in GetComponentsInChildren<Spawner>())
        {
            spawners.Add(spawner);
        }

        paths = new List<EnemyPath>();
        foreach(var path in GetComponentsInChildren<EnemyPath>())
        {
            paths.Add(path);
        }
    }

    public void StartEncounter()
    {
        foreach (var spawner in spawners)
        {
            spawner.Activate();
        }

        foreach (var path in paths)
        {
            path.Activate();
        }
    }

    public void EndEncounter()
    {
        foreach (var spawner in spawners)
        {
            spawner.DeActivate();
        }

        foreach (var path in paths)
        {
            path.DeActivate();
        }
    }
}
