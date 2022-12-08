using System;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvents : MonoBehaviour
{
	public UnityEventCollider onTriggerEnter = new UnityEventCollider();
	public UnityEventCollider onTriggerExit = new UnityEventCollider();
	private void OnTriggerEnter(Collider other) => onTriggerEnter.Invoke(other);
	private void OnTriggerExit(Collider other) => onTriggerExit.Invoke(other);
}
[Serializable]
public class UnityEventCollider : UnityEvent<Collider> { }
