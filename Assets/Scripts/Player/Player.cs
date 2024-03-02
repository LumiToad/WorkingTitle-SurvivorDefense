using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour, IWeaponHolder, IDamageAble, IDamageSource
{
    [Title("Player")]
    [SerializeField]
    private AbstractWeapon startWeapon;
    [SerializeField]
    private PlayerMovement movement;
    [SerializeField]
    private IntStat hp;

    [field: SerializeField, MinValue(1)]
    public float XPUntilLevelUp;
    [SerializeField]
    public int LevelUpHP;
    private float currentXP = 0;
    private int currentLevel = 0;

    [SerializeField, FoldoutGroup("References")]
    private Renderer[] blinkingMeshes;

    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem damageParticles;
    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem healParticles;
    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem levelUpParticles;

    [SerializeField, FoldoutGroup("References")]
    private Animator anim;


    private PlayerWeapons weapons;
    private PlayerInteraction interaction;
    private Healthbar healthbar;
    private EnemyMagnet magnet;
    private PlayerMesh mesh;
    private PlayerVignette vignette;
    private PlayerUI ui;
    private Rigidbody rb;
    private PlayerInputActions input;

    private bool canBeHurt = true;
    private bool isInBlinkingState = false;

    public string glueSourceName => "GluePlayer";

    private void Awake()
    {
        Time.timeScale = 1;

        input = new PlayerInputActions();
        ui = GetComponentInChildren<PlayerUI>();
        weapons = GetComponentInChildren<PlayerWeapons>();
        interaction = GetComponentInChildren<PlayerInteraction>();
        healthbar = GetComponentInChildren<Healthbar>();
        magnet = GetComponentInChildren<EnemyMagnet>(true);
        mesh = GetComponentInChildren<PlayerMesh>();
        vignette = GetComponentInChildren<PlayerVignette>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movement.PlayerDashed += OnPlayerDashed;
        movement.SetUp(ui);

        if(startWeapon != null)
        {
            WeaponManager.GetWeapon(startWeapon.gameObject.name, this);
        }
        else
        {
            WeaponManager.GetRandomWeapon(this);
        }

        StartCoroutine(DamageOnOutsideOfScreen());
        StartCoroutine(TeleportToCenter());

        ActivateTurrets();
        SetMagnet(true);
    }

    private void Update()
    {
        if (Time.timeScale == 0.0f) return;

        movement.MovementUpdate(input, transform, rb, anim, StartCoroutine);
        weapons.WeaponUpdate(input, new IntStat(this.hp.totalValue / 2));
        
        var interactTarget = interaction.TryGetInteractTarget(input, this);
        if(interactTarget != null)
        {
            StartCoroutine(Interact(interactTarget));
        }
    }

    private void FixedUpdate()
    {
        movement.FixedMovementUpdate(transform, rb);
    }

    private IEnumerator TeleportToCenter()
    {
        yield return new WaitForEndOfFrame();

        LayerMask excludeLayers = LayerMask.GetMask("Player", "NoPlacement");
        float screenWidthMid = Screen.currentResolution.width / 2;
        float screenHeightMid = Screen.currentResolution.height / 2;
        float angleRad = 10.0f * Mathf.PI / 180.0f;
        Vector3 saveSpot = GetSaveTeleportSpot(0.0f, angleRad, screenWidthMid, screenHeightMid, excludeLayers);
        saveSpot.y = transform.position.y;
        transform.position = saveSpot;
    }

    private Vector3 GetSaveTeleportSpot(float radius, float angleRad, float screenWidthMid, float screenHeightMid, LayerMask excludeLayers)
    {
        float x = screenWidthMid + (radius * Mathf.Cos(angleRad));
        float y = screenHeightMid + (radius * Mathf.Sin(angleRad));

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, Camera.main.nearClipPlane));
        RaycastHit[] hits;
        Vector3 spot = Vector3.zero;
        hits = Physics.RaycastAll(ray.origin, ray.direction, 100.0f, ~excludeLayers);
        foreach (RaycastHit hit in hits)
        {
            int layer = hit.collider.gameObject.layer;
            if (
                layer == LayerMask.NameToLayer("Obstacle") ||
                layer == LayerMask.NameToLayer("PlayerOnlyWall")
                )
            {
                spot = Vector3.zero;
                break;
            }

            spot = hit.point;
        }

        if (spot == Vector3.zero)
        {
            return GetSaveTeleportSpot(radius + 10f, angleRad + 10.0f, screenWidthMid, screenHeightMid, excludeLayers);
        }
        return spot;
    }

    public void EarnXP(float amount)
    {
        currentXP += amount;
        ui.SetXPBar(currentXP, XPUntilLevelUp);

        while (currentXP >= XPUntilLevelUp)
        {
            LevelUp();
            currentXP -= XPUntilLevelUp;

            ui.SetXPBar(currentXP, XPUntilLevelUp);
        }
    }

    private void LevelUp()
    {
        WeaponManager.GetRandomWeapon(this);
        RestoreHealth(LevelUpHP);
        levelUpParticles.Play();

        currentLevel++;
        ui.SetLevel(currentLevel);
    }

    public void TakeDamage(int damage, IDamageSource source)
    {
        if (!canBeHurt) return;

        hp -= damage;

        healthbar.SetTo(hp.totalValue / (float)hp.value);
        mesh.Blink();
        vignette.Blink();
        damageParticles.Play();

        if (hp <= 0)
        {
            Die("You just took too much damage.");
        }
    }

    public void TakeDamagePercent(int percent, IDamageSource source)
    {
        if (!canBeHurt) return;

        int damage = Mathf.FloorToInt((float)(hp.value / 100.0f) * percent);
        TakeDamage(damage, source);
    }

    private void Die(string message)
    {
        GameUI.ShowGameOverScreen(message);
    }

    public void RestoreHealth(int healing)
    {
        hp += healing;
        healthbar.SetTo(hp.totalValue / (float)hp.value);
        healParticles.Play();
    }

    public void ActivateTurrets()
    {
        weapons.Activate();

        input.Player.BuildTower.Enable();
        input.Player.SelectLeftTower.Enable();
        input.Player.SelectRightTower.Enable();
        input.Player.ScrollTowerSelection.Enable();
    }

    public void DeActivateTurret()
    {
        weapons.DeActivate();

        input.Player.BuildTower.Disable();
        input.Player.SelectLeftTower.Disable();
        input.Player.SelectRightTower.Disable();
        input.Player.ScrollTowerSelection.Disable();
    }

    public void SetMagnet(bool state) => magnet.gameObject.SetActive(state);

    private void OnPlayerDashed(bool value)
    {
        canBeHurt = !value;
    }

    private void OnEnable() => SetInput(true);

    private void OnDisable() => SetInput(false);

    private void SetInput(bool state)
    {
        if(state == true)
        {
            input.Player.Enable();
            input.Player.BuildTower.Disable();
            input.Player.SelectLeftTower.Enable();
            input.Player.SelectRightTower.Enable();
            input.Player.ScrollTowerSelection.Enable();
        }
        else
        {
            input.Player.Disable();
        }
    }

    private IEnumerator StartIFrameState(float duration)
    {
        isInBlinkingState = true;
        canBeHurt = false;
        StartCoroutine(BlinkingState(0.01f));
        yield return new WaitForSeconds(duration);
        isInBlinkingState = false;
        canBeHurt = true;
    }

    private IEnumerator BlinkingState(float speed)
    {
        foreach (var renderer in blinkingMeshes)
        {
            renderer.enabled = !renderer.enabled;
        }

        yield return new WaitForSeconds(speed);
        
        if (isInBlinkingState) 
        {
            StartCoroutine(BlinkingState(speed));
        }
        else
        {
            foreach (var renderer in blinkingMeshes)
            {
                renderer.enabled = true;
            }
        }
    }

    private IEnumerator DamageOnOutsideOfScreen()
    {
        yield return new WaitForSeconds(2.0f);

        if (!gameObject.IsOnScreen(60.0f))
        {
            TakeDamagePercent(30,this);
            StartCoroutine(StartIFrameState(0.5f));
            StartCoroutine(TeleportToCenter());
        }

        StartCoroutine(DamageOnOutsideOfScreen());
    }

    private IEnumerator Interact(IInteractable toInteract)
    {
        if (!input.Player.enabled) yield break;

        var punchDuration = 0.8f;
        var fullAnimDuration = 1.15f;
        var returnDuration = 0.2f;

        var originalDirection = transform.forward;

        input.Player.Move.Disable();
        anim.SetTrigger("ActivateGenerator");

        float timer = 0;
        while(timer < punchDuration)
        {
            timer += Time.deltaTime;
            movement.LookIntoDirection(toInteract.gameObject.transform.position - transform.position, transform);
            yield return new WaitForEndOfFrame();
        }

        toInteract.Interact(this);

        while(timer < fullAnimDuration)
        {
            timer += Time.deltaTime;
            movement.LookIntoDirection(toInteract.gameObject.transform.position - transform.position, transform);
            yield return new WaitForEndOfFrame();
        }

        timer = 0;

        while(timer < returnDuration)
        {
            movement.LookIntoDirection(originalDirection, transform);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        input.Player.Move.Enable();
    }

    #region Forward IAbilityHolder functions to abilities
    public void RemoveWeapon(AbstractWeapon weapon) => weapons.RemoveWeapon(weapon);

    public bool TryAddWeapon(AbstractWeapon weapon) => weapons.TryAddWeapon(weapon);
    #endregion
}
