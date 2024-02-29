using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;


public class TutorialArena : LevelSection
{
    [SerializeField, FoldoutGroup("References"), Required]
    private ArenaScrollingTargetNew target;

    [SerializeField]
    private TutorialCombatEncounter firstCombat;
    [SerializeField]
    private TutorialCombatEncounter intermediateCombat;
    [SerializeField]
    private TutorialCombatEncounter towerCombat;
    [SerializeField]
    private TutorialCombatEncounter realCombat;

    private ArenaCanvas ui;
    private TutorialUI tutorial;
    private CamScrollingNew camScrolling;
    private PlayerBase generator;

    private Player player;

    public override void StartSection(int index)
    {
        camScrolling.GoToTarget(target);
        StartCoroutine(Tutorial());
        base.StartSection(index);
    }

    private void Awake()
    {
        ui = GetComponentInChildren<ArenaCanvas>();
        camScrolling = Camera.main.GetComponent<CamScrollingNew>();
        generator = GetComponentInChildren<PlayerBase>();
        tutorial = GetComponentInChildren<TutorialUI>();

        generator.activated += GeneratorActivated;

        ui.Hide();
    }

    private void GeneratorActivated(Player by)
    {
        GetComponentInChildren<LocationIcon>(true).gameObject.SetActive(false);
        player = by;
        by.SetMagnet(false);
    }

    private IEnumerator Tutorial()
    {
        yield return new WaitForEndOfFrame();
        WeaponManager.SetTutorialMode(true);

        FindObjectOfType<Player>().DeActivateTurret();

        yield return StartCoroutine(Greetings());
        yield return StartCoroutine(GeneratorActivation());

        var defaultXPUntilLevelUp = player.XPUntilLevelUp;
        player.XPUntilLevelUp = 20;

        yield return StartCoroutine(Weapons());
        yield return StartCoroutine(Combat(firstCombat));
        yield return StartCoroutine(Experience());
        yield return StartCoroutine(ExperienceTask());

        player.XPUntilLevelUp = 60;

        yield return StartCoroutine(Combat(intermediateCombat));
        yield return StartCoroutine(Tower());

        StartCoroutine(CountDownArenaTimer());
        yield return StartCoroutine(Combat(realCombat));
        yield return StartCoroutine(Finished());

        WeaponManager.SetTutorialMode(false);

        AchivementManager.ProgressAchivement(1, new[] { this });
        player.XPUntilLevelUp = defaultXPUntilLevelUp;
        player.SetMagnet(true);
        ui.Hide();
        generator.HideUI();
        base.EndSection();
    }

    #region Steps
    private IEnumerator Greetings()
    {
        tutorial.Show(TutorialDisplays.welcome);

        yield return StartCoroutine(WaitUntilConfirmed(TutorialDisplays.welcome));

        tutorial.Hide(TutorialDisplays.welcome);
    }


    private IEnumerator GeneratorActivation()
    {
        tutorial.Show(TutorialDisplays.activateGenerator);
        tutorial.UpdateProgress(TutorialDisplays.activateGenerator, 1);

        bool activated = false;
        generator.activated += (Player p) => { activated = true; };
        while (!activated)
        {
            yield return new WaitForEndOfFrame();
        }

        tutorial.UpdateProgress(TutorialDisplays.activateGenerator, 0);

        yield return new WaitForSeconds(1);

        tutorial.Hide(TutorialDisplays.activateGenerator);
    }

    private IEnumerator Weapons()
    {
        tutorial.Show(TutorialDisplays.weapons);

        yield return StartCoroutine(WaitUntilConfirmed(TutorialDisplays.weapons));

        tutorial.Hide(TutorialDisplays.weapons);
    }

    private IEnumerator Combat(TutorialCombatEncounter encounter)
    {
        tutorial.Show(TutorialDisplays.destroyEnemies);
        tutorial.UpdateProgress(TutorialDisplays.destroyEnemies, 20 - Pool.GetOutOfPoolCount<AbstractEnemy>());
        encounter.StartEncounter();

        float timer = 0;

        while (timer < 1.25f)
        {
            var enemies = Pool.GetOutOfPoolCount<AbstractEnemy>();
            tutorial.UpdateProgress(TutorialDisplays.destroyEnemies, enemies);

            if(enemies == 0)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }

            yield return new WaitForEndOfFrame();
        }

        encounter.EndEncounter();
        tutorial.Hide(TutorialDisplays.destroyEnemies);
    }

    private IEnumerator Experience()
    {
        tutorial.Show(TutorialDisplays.XPItems);

        yield return StartCoroutine(WaitUntilConfirmed(TutorialDisplays.XPItems));

        tutorial.Hide(TutorialDisplays.XPItems);
    }

    private IEnumerator ExperienceTask()
    {
        tutorial.Show(TutorialDisplays.collectXPItems);

        while (Pool.GetOutOfPoolCount<XPItem>() > 0)
        {
            yield return new WaitForEndOfFrame();
            tutorial.UpdateProgress(TutorialDisplays.collectXPItems, Pool.GetOutOfPoolCount<XPItem>());
        }


        tutorial.Hide(TutorialDisplays.collectXPItems);
    }

    private IEnumerator Tower()
    {
        towerCombat.StartEncounter();

        tutorial.Show(TutorialDisplays.towers);

        yield return StartCoroutine(WaitUntilConfirmed(TutorialDisplays.towers));

        tutorial.Hide(TutorialDisplays.towers);

        player.ActivateTurrets();

        tutorial.Show(TutorialDisplays.destroyEnemies);
        tutorial.UpdateProgress(TutorialDisplays.destroyEnemies, 20 - Pool.GetOutOfPoolCount<AbstractEnemy>());

        float timer = 0;

        while (timer < 3)
        {
            var enemies = Pool.GetOutOfPoolCount<AbstractEnemy>();
            tutorial.UpdateProgress(TutorialDisplays.destroyEnemies, enemies);

            if (enemies == 0)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }

            yield return new WaitForEndOfFrame();
        }

        tutorial.Hide(TutorialDisplays.destroyEnemies);
    }

    private IEnumerator CountDownArenaTimer()
    {
        ui.gameObject.SetActive(true);

        var timer = 0f;
        while(timer < 60)
        {
            tutorial.Hide(TutorialDisplays.destroyEnemies);
            ui.ShowTime(60 - timer);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ui.ShowTime(0);
    }

    private IEnumerator Finished()
    {
        tutorial.Show(TutorialDisplays.congratulations);

        yield return StartCoroutine(WaitUntilConfirmed(TutorialDisplays.congratulations));

        tutorial.Hide(TutorialDisplays.congratulations);
    }

    #endregion

    private IEnumerator WaitUntilConfirmed(TutorialDisplays step)
    {
        Time.timeScale = 0;
        tutorial.confirmed += confirmed;
        bool flag = false;

        while (!flag)
        {
            yield return new WaitForEndOfFrame();
        }

        tutorial.confirmed -= confirmed;
        Time.timeScale = 1;

        void confirmed(TutorialDisplays display)
        {
            if(display == step)
            {
                flag = true;
            }
        }
    }

}
