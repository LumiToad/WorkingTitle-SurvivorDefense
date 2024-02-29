using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerMovement
{
    public Action<bool> PlayerDashed;

    private const float rotationSpeed = 25;

    public delegate Coroutine startCorutine(IEnumerator routine);

    private PlayerUI ui;

    [SerializeField]
    private FloatStat speed;

    [SerializeField, FoldoutGroup("References")]
    private SFXPlayer[] sfxPlayers;
    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem[] dashVfxs;
    [SerializeField, FoldoutGroup("References")]
    private DashIndicator dashIndicator;

    [SerializeField, FoldoutGroup("DashSetting")]
    private float dashSpeed = 25.0f;

    [SerializeField, SuffixLabel("Seconds", true), FoldoutGroup("DashSetting")]
    private float dashDuration = 0.1f;
    private float dashDurationTimer;

    [SerializeField, SuffixLabel("Seconds", true), FoldoutGroup("DashSetting")]
    private float dashCooldown = 2.0f;
    private float dashCooldownTimer;

    private Vector2 inputDirection = new Vector2();

    private bool isDashing = false;

    public void SetUp(PlayerUI ui)
    {
        this.ui = ui;
        dashCooldownTimer = dashCooldown;
    }

    public void MovementUpdate(PlayerInputActions input, Transform transform, Rigidbody rb, Animator playerAnim, startCorutine start)
    {
        inputDirection = input.Player.Move.ReadValue<Vector2>();

        playerAnim.SetBool("walking", inputDirection != Vector2.zero);

        UpdateDashTimer(rb);
        UpdateDashIndicator();
        TryDash(input, transform, rb, start);
    }

    public void FixedMovementUpdate(Transform transform, Rigidbody rb)
    {
        Move(transform, rb);

        if (inputDirection == new Vector2()) return;
        LookIntoDirection(new Vector3(inputDirection.x, 0, inputDirection.y), transform);
    }

    private void Move(Transform transform, Rigidbody rb)
    {
        if (isDashing) return;

        inputDirection.Normalize();
        rb.velocity = new Vector3(inputDirection.x, 0, inputDirection.y) * speed;
    }

    public void LookIntoDirection(Vector3 dir, Transform transform)
    {
        if (isDashing) return;

        if (dir == new Vector3()) return;
        var rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }

    private bool TryDash(PlayerInputActions input, Transform transform, Rigidbody rb, startCorutine start)
    {
        if (!input.Player.Dash.WasPressedThisFrame()) return false;
        if (dashCooldownTimer < dashCooldown) return false;

        start(Dash(rb, transform));
        
        foreach (var sfxPlayer in sfxPlayers)
        {
            sfxPlayer.PlayOnTriggerKey("DASH");
        }

        foreach (var dashVfx in dashVfxs) 
        {
            dashVfx.Stop();
            var main = dashVfx.main;
            main.duration = dashDuration / 2;
            main.startLifetime = dashDuration / 2;
            dashVfx.Play();
        }

        dashIndicator.Dash(transform, dashCooldown, dashDuration, dashSpeed + speed);
        ui.SetDashCD(dashCooldown);

        return true;
    }

    private void UpdateDashTimer(Rigidbody rb)
    {
        if (dashCooldownTimer < dashCooldown)
        {
            dashCooldownTimer += Time.deltaTime;
        }
    }

    private void UpdateDashIndicator()
    {
        if (dashCooldownTimer >= dashCooldown && 
            dashIndicator.gameObject.activeSelf)
        {
            dashIndicator.MoveToPlayer();
        }
    }

    private IEnumerator Dash(Rigidbody rb, Transform transform)
    {
        PlayerDashed?.Invoke(true);
        dashCooldownTimer = 0;

        isDashing = true;
        rb.velocity = transform.forward * (dashSpeed + speed);
        rb.excludeLayers = LayerMask.GetMask("Enemy");

        yield return new WaitForSeconds(dashDuration);

        dashDurationTimer = 0.0f;
        isDashing = false;
        rb.excludeLayers = 0;
        PlayerDashed?.Invoke(false);
    }
}
