using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour, IInteractor
{
	public UnityEvent OnInteractionAvailable = new UnityEvent();
	public UnityEvent OnNothingToInteract = new UnityEvent();
	public List<IInteractable> interactables = new List<IInteractable>();
	protected IInteractable interactedWith;

	//Appears that UnityEvents can't be Invoked outside of Update call;
	private IEnumerator Start()
	{
		yield return null;
		if (interactables.Count == 0)
			OnNothingToInteract.Invoke();
	}

	public void TriggerEnter(Collider other)
	{
		if (other.GetComponent<IInteractable>() is IInteractable interactable)
		{
			if (interactables.Count == 0)
				OnInteractionAvailable.Invoke();
			interactables.Add(interactable);
		}
	}
	public void TriggerExit(Collider other)
	{
		if (other.GetComponent<IInteractable>() is IInteractable interactable)
		{
			interactables.Remove(interactable);
			if (interactables.Count == 0)
				OnNothingToInteract.Invoke();
		}
		if (interactedWith != null)
		{
			InteractionFinished();
		}
	}

	public virtual void InteractStarted()
	{
		interactedWith = interactables.LastOrDefault();
		interactedWith?.Interact(this);
	}

	public virtual void InteractionFinished()
	{
		interactedWith?.InteractionFinished(this);
		interactedWith = null;
	}

}
