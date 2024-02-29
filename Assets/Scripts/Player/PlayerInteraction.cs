using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<IInteractable> interactables = new List<IInteractable>();

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable == null || interactables.Contains(interactable)) return;


        interactable?.SetInteraction(true);
        interactables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable == null || !interactables.Contains(interactable)) return;

        interactable?.SetInteraction(false);
        interactables.Remove(interactable);
    }

    public IInteractable TryGetInteractTarget(PlayerInputActions input, Player player)
    {
        if (!input.Player.Interact.WasPerformedThisFrame()) return null;

        foreach(var interactable in interactables)
        {
            if (interactable.InteractionLocked()) continue;
            return interactable;
        }

        interactables = new List<IInteractable>();

        return null;
    }
}
