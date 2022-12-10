using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour, IInteractor
{
	public Transform receiverTarget;
	public List<IInteractable> interactables = new List<IInteractable>();
	public UnityEvent OnInteractionAvailable = new UnityEvent();
	public UnityEvent OnNothingToInteract = new UnityEvent();
	private IInteractable interactedWith;
	public ResourceTransferScript PlayerTransfer;
	private IEnumerator _snapTargetCR;

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
			interactedWith.InteractionFinished(this);
			interactedWith = null;
		}
	}

	//Appears that UnityEvents can't be Invoked outside of Update call;
	private IEnumerator Start()
	{
		yield return null;
		if (interactables.Count == 0)
			OnNothingToInteract.Invoke();
	}

	public void InteractStarted()
	{
		interactedWith = interactables.LastOrDefault();
		interactedWith?.Interact(this);
		InteractWithReceiver();
	}

	public void InteractionFinished()
	{
		interactedWith?.InteractionFinished(this);
		FinishInteractWithReceiver();
	}

	private void InteractWithReceiver()
	{
		if (_snapTargetCR != null) StopCoroutine(_snapTargetCR);
		if (interactedWith is InteractableReciever receiver && PlayerTransfer)
		{
			PlayerTransfer.StartParticles();
			_snapTargetCR = PlayerTransfer.SnapTarget(receiver.transform);
			StartCoroutine(_snapTargetCR);
		}
	}

	private void FinishInteractWithReceiver()
	{
		if (interactedWith is InteractableReciever receiver && PlayerTransfer)
		{
			StopCoroutine(_snapTargetCR);
			_snapTargetCR = null;
			if (!PlayerTransfer) return;
			PlayerTransfer.StopParticles();
		}
		interactedWith = null;
	}
}