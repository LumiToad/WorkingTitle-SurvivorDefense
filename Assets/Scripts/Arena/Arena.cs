using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : LevelSection
{
    [SerializeField, SuffixLabel("Seconds", true)]
    private float duration = 10;
    [SerializeField, FoldoutGroup("References"), Required]
    private ArenaScrollingTargetNew target;

    private float timer = 0;
    private bool active = false;

    private ArenaCanvas ui;
    private List<Spawner> spawners;
    private PlayerBase generator;
    private LocationIcon locationIcon;

    private Player player;

    private List<EnemyPath> arenaPaths;

    private CamScrollingNew camScrolling;

    private void Awake()
    {
        timer = duration;

        spawners = new List<Spawner>();
        ui = GetComponentInChildren<ArenaCanvas>();
        generator = GetComponentInChildren<PlayerBase>();
        locationIcon = GetComponentInChildren<LocationIcon>(true);
        camScrolling = Camera.main.GetComponent<CamScrollingNew>();

        foreach (var s in GetComponentsInChildren<Spawner>(true))
        {
            spawners.Add(s);
            s.enabled = false;
        }

        arenaPaths = new List<EnemyPath>();
        foreach(var path in GetComponentsInChildren<EnemyPath>(true))
        {
            arenaPaths.Add(path);
            path.DeActivate();
        }

        if(generator != null)
        {
            generator.activated += Activate;
        }
        else
        {
            Debug.LogError("Error: Arena contains no energy generator");
        }

        ui.Hide();
    }

    public override void StartSection(int index)
    {
        camScrolling.GoToTarget(target);
    }

    public override void LevelSectionUpdate(int distanceToThisSection)
    {
        if(distanceToThisSection == 1)
        {
            locationIcon.Show();
        }
        else
        {
            locationIcon.Hide();
        }

        base.LevelSectionUpdate(distanceToThisSection);
    }

    public void Activate(Player p)
    {
        this.player = p;
        active = true;

        p.SetMagnet(false);
        p.ActivateTurrets();
        ui.Show();

        foreach (var s in spawners)
        {
            s.Activate();
        }

        foreach(var path in arenaPaths)
        {
            path.Activate();
        }

        Pool.ReturnAll<XPItem>();
    }

    public void Complete()
    {
        active = false;
        ui.Hide();

        foreach (var s in spawners)
        {
            s.DeActivate();
        }

        foreach(var path in arenaPaths)
        {
            path.DeActivate();   
        }


        StartCoroutine(WaitForAllEnemyDeaths());
    }

    private IEnumerator WaitForAllEnemyDeaths()
    {
        while(Pool.GetOutOfPoolCount<AbstractEnemy>() > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        player.SetMagnet(true);
        generator.HideUI();

        Pool.ReturnAll<XPItem>();

        EndSection();

        if (target == null) yield break;
        target.EnableVirtualCam(false);
    }

    private void FixedUpdate()
    {
        if (!active) return;
        timer -= Time.deltaTime;

        ui.ShowTime(timer);

        if(timer < 0)
        {
            Complete();
        }
    }
}
