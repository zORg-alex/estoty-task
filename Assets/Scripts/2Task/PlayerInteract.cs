using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class PlayerInteract : Interactor
{
	public Transform receiverTarget;
	public ResourceTransferScript PlayerTransfer;
	private IEnumerator _snapTargetCR;

	public override void InteractStarted()
	{
		base.InteractStarted();

		if (_snapTargetCR != null) StopCoroutine(_snapTargetCR);
		if (interactedWith is InteractableReciever receiver && PlayerTransfer)
		{
			PlayerTransfer.StartParticles();
			_snapTargetCR = PlayerTransfer.SnapTarget(receiver.transform);
			StartCoroutine(_snapTargetCR);
		}
	}
	public override void InteractionFinished()
	{
		if (interactedWith is InteractableReciever receiver && PlayerTransfer)
		{
			StopCoroutine(_snapTargetCR);
			_snapTargetCR = null;
			if (!PlayerTransfer) return;
			PlayerTransfer.StopParticles();
		}
		base.InteractionFinished();
	}
}