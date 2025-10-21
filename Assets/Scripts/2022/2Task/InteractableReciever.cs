using System.Collections;
using UnityEngine;

public class InteractableReciever : MonoBehaviour, IInteractable
{
	public ResourceTransferScript Transfer;
	private IEnumerator _snapTargetCR;

	public void InteractionStarted(IInteractor interactor)
	{
		if (!Transfer) return;
		if (_snapTargetCR != null) StopCoroutine(_snapTargetCR);
		if (interactor.gameObject.GetComponent<PlayerInteract>() is PlayerInteract playerInteract)
		{
			Transfer.StartParticles();
			_snapTargetCR = Transfer.Snap(playerInteract.receiverTarget, null);
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