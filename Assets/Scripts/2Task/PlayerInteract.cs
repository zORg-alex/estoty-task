using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class PlayerInteract : Interactor
{
	public Transform receiverTarget;
	private IEnumerator _snapTargetCR;

	public override void InteractStarted()
	{
		base.InteractStarted();
	}
	public override void InteractionFinished()
	{
		base.InteractionFinished();
	}
}