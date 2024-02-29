using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HallwayNew : LevelSection
{
    private List<Spawner> spawners = new();
    private List<EnemyPath> paths = new();
    private List<ScrollingTargetNew> scrollingTargets = new();

    private CamScrollingNew camScrolling;
    private int currentTargetIndex = 0;
    private bool isLastBlend = false;
    public BaseScrollingTarget CurrentlySelectedTarget { get => scrollingTargets[currentTargetIndex]; }
    public ScrollingTargetNew FirstScrollingTarget { get => scrollingTargets.First(); }
    public ScrollingTargetNew LastScrollingTarget { get => scrollingTargets.Last(); }

    [SerializeField, SuffixLabel("Seconds", true)]
    private float hallwayDuration = 0.0f;

    private void Awake()
    {
        camScrolling = Camera.main.GetComponent<CamScrollingNew>();
        EnableHallway(false);
        if (hallwayDuration == 0.0f) Debug.LogError("LUMI: HALLWAY DURATION IS 0.0 seconds!");
    }

    public override void StartSection(int index)
    {
        Setup(index);
        base.StartSection(index);
    }

    public void Setup(int index)
    {
        foreach (Spawner s in GetComponentsInChildren<Spawner>(true))
        {
            spawners.Add(s);
        }

        foreach (EnemyPath p in GetComponentsInChildren<EnemyPath>(true))
        {
            paths.Add(p);
        }

        foreach (ScrollingTargetNew s in GetComponentsInChildren<ScrollingTargetNew>(true))
        {
            scrollingTargets.Add(s);
        }

        if (spawners.Count == 0) Debug.LogWarning($"LUMI: No spawners in [{gameObject.name}]. Is that correct?");
        if (paths.Count == 0) Debug.LogWarning($"LUMI: No enemy paths in [{gameObject.name}]. Is that correct?");
        if (scrollingTargets.Count == 0) Debug.LogError($"LUMI: NO SCROLLING TARGETS IN [{gameObject.name}]!!!");

        CalculateScrollingSpeed(index);
        EnableHallway(true);
        camScrolling.TargetReached += OnTargetReached;
        CameraBlend();
    }

    private void CalculateScrollingSpeed(int index)
    {
        int startModifyer = (index == 0) ? 1 : 0;
        List<Vector3> allPos = new();
        if (index > 0)
        {
            allPos.Add(camScrolling.transform.position);
        }
        foreach (var target in scrollingTargets) 
        {
            allPos.Add(target.transform.position);
        }

        float[] distances = new float[allPos.Count - 1];
        float fullDistance = 0.0f;

        for (int i = 0; i < distances.Length; i++)
        {
            if (i + 1 > distances.Length) break;
            AddCalculation(allPos[i], allPos[i + 1], ref distances, ref fullDistance, i);
        }

        float velocity = fullDistance / hallwayDuration;

        for (int i = 0; i < distances.Length; i++)
        {
            if (i + 1 > distances.Length) break;
            scrollingTargets[i + startModifyer].CalculatedBlendTime = distances[i] / velocity;
        }
    }

    private void AddCalculation(Vector3 a, Vector3 b, ref float[] distances, ref float fullDistance, int iteration)
    {
        float calcDistance = Vector3.Distance(a, b);
        distances[iteration] = calcDistance;
        fullDistance += calcDistance;
    }

    public void EnableHallway(bool value)
    {
        EnableSpawners(value);
        EnablePaths(value);
    }

    private void OnTargetReached()
    {
        if (isLastBlend) 
        {
            camScrolling.TargetReached -= OnTargetReached;
            EndSection();
            HallwayEnd();
            return;
        }
        isLastBlend = (CurrentlySelectedTarget == LastScrollingTarget);
        CameraBlend();
    }

    private void CameraBlend()
    {
        camScrolling.GoToTarget(CurrentlySelectedTarget);
        IncreaseTargetSelection(1);
    }

    private void EnableSpawners(bool value)
    {
        foreach(Spawner spawner in spawners)
        {
            if (value) 
            {
                spawner.Activate();
            }
            else
            {
                spawner.DeActivate();
            }
        }
    }

    private void EnablePaths(bool value)
    {
        foreach (EnemyPath path in paths)
        {
            if (value)
            {
                path.Activate();
            }
            else
            {
                path.DeActivate();
            }
        }
    }

    public void IncreaseTargetSelection(int value)
    {
        currentTargetIndex += value;
        currentTargetIndex = Mathf.Clamp(currentTargetIndex, 0, scrollingTargets.Count - 1);
    }

    public void DecreaseTargetSelection(int value)
    {
        currentTargetIndex -= value;
        currentTargetIndex = Mathf.Clamp(currentTargetIndex, 0, scrollingTargets.Count - 1);
    }

    public void HallwayEnd()
    {
        EnableHallway(false);
        Pool.ReturnAll<AbstractEnemy>();
        Pool.ReturnAll<XPItem>();
    }

    public void OnDrawGizmos()
    {
        List<ScrollingTargetNew> gizmosTargets = new();
        foreach (ScrollingTargetNew s in GetComponentsInChildren<ScrollingTargetNew>(true))
        {
            gizmosTargets.Add(s);
        }

        Gizmos.color = Color.magenta;

        for (int i = 0; i < gizmosTargets.Count; i++) 
        {
            Vector3 a = gizmosTargets[i].transform.position;
            Gizmos.DrawSphere(a, 1.0f);
            if (i + 1 >= gizmosTargets.Count) break;
            Vector3 b = gizmosTargets[i + 1].transform.position;
            Gizmos.DrawLine(a, b);
            Gizmos.DrawSphere(b, 1.0f);
        }
    }
}
