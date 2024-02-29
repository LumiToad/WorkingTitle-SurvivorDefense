using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    GameObject gameObject { get; }
    public bool InteractionLocked();
    public void SetInteraction(bool state);
    public void Interact(Player player);
}
