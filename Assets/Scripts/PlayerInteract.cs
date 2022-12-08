using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour, IInteractor
{
	public List<IInteractable> interactables = new List<IInteractable>();
	public void TriggerEnter(Collider other)
	{
		if (other.GetComponent<InteractableSource>() is InteractableSource interactable)
		{
			interactables.Add(interactable);
		}
	}
	public void TriggerExit(Collider other)
	{

	}

	public void Interact()
	{
		interactables.LastOrDefault().Interact(this);
	}
}