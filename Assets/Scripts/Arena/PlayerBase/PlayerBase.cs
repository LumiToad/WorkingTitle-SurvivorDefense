using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageAble
{
    public Action<Player> activated;

    [SerializeField, FoldoutGroup("References")]
    private Healthbar worldHealthbar;

    [SerializeField]
    private IntStat health;

    private BaseUI ui;
    private BaseInteractionUI interactUI;
    private Camera cam;
    private LocationIcon icon;
    private InteractionMessager interaction;

    private void Awake()
    {
        cam = Camera.main;
        ui = GetComponentInChildren<BaseUI>();
        interactUI = GetComponentInChildren<BaseInteractionUI>(true);
        icon = GetComponentInChildren<LocationIcon>(true);
        interaction = GetComponentInChildren<InteractionMessager>();

        interaction.canInteract += CanInteract;
        interaction.interact += Interact;

        ui.Hide();
    }

    private void Update()
    {
        var screenPos = cam.WorldToScreenPoint(transform.position);
        var screenDistance = Vector3.Distance(new Vector3(Screen.width/ 2, 0, Screen.height / 2), new Vector3(screenPos.x, 0, screenPos.y));
        var displayDistance = Mathf.RoundToInt((screenDistance - Screen.width / 2) / 100);

        icon.UpdateEdgeString(displayDistance.ToString() + "m");
    }

    public void TakeDamage(int damage, IDamageSource source)
    {
        health -= damage;

        var healthPercent = health.totalValue / (float)health.value;
        ui.SetHealthPercent(healthPercent);
        worldHealthbar.SetTo(healthPercent);

        QuickHints.ShowOnce("BaseHint");

        if(health <= 0)
        {
            GameUI.ShowGameOverScreen("Your base took too much damage.");
        }
    }

    public void CanInteract(bool state)
    {
        if (state)
        {
            interactUI.Show();
        }
        else
        {
            interactUI.Hide();
        }
    }

    public void Interact(Player player)
    {
        activated?.Invoke(player);
        interactUI.Hide();

        ui.Show();
        icon.Hide();

        interaction.Disable();
    }

    public void HideUI()
    {
        ui.Hide();
    }
}
