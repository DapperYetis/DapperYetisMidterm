using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool Interact();

    public bool CanInteract();
    public int GetCost();
}
