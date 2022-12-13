using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSource : MonoBehaviour, IInteractable
{
	public float transferRate = 10f;
	public ResourceTransferScript Transfer;
	private IEnumerator _snapTargetCR;

	public void Interact(IInteractor interactor)
	{
		if (!Transfer) return;
		if (_snapTargetCR != null) StopCoroutine(_snapTargetCR);
		if (interactor.gameObject.GetComponent<PlayerInteract>() is PlayerInteract playerInteract)
		{
			Transfer.StartParticles();
			_snapTargetCR = Transfer.Snap(null, playerInteract.receiverTarget);
			StartCoroutine(_snapTargetCR);
		}
	}
	public void InteractionFinished(IInteractor interactor)
	{
		StopCoroutine(_snapTargetCR);
		_snapTargetCR = null;
		if (!Transfer) return;
		Transfer.StopParticles();
	}
}
