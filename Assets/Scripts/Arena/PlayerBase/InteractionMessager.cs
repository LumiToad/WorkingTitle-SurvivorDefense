using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractionMessager : MonoBehaviour, IInteractable
{
    public event Action<bool> canInteract;
    public event Action<Player> interact;

    public void Disable()
    {
        GetComponent<Collider>().enabled = false;
    }

    public void OnEnable()
    {
        GetComponent<Collider>().enabled = true;
    }

    public bool InteractionLocked()
    {
        return !GetComponent<Collider>().enabled;
    }

    public void SetInteraction(bool state)
    {
        canInteract?.Invoke(state);
    }

    public void Interact(Player player)
    {
        interact?.Invoke(player);
    }
}
