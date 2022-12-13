using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ResourceTransferScript : MonoBehaviour
{
	private VisualEffect vfx;
	public Transform target;
	public Transform source;

	private void OnEnable() => vfx = GetComponent<VisualEffect>();
	public void StartParticles() => vfx.SendEvent("StartParticles");
	public void StopParticles() => vfx.SendEvent("StopParticles");

	public IEnumerator Snap(Transform source, Transform target)
	{
		while (true)
		{
			if (source != null) this.source.position = source.position;
			if (target != null) this.target.position = target.position;
			yield return null;
		}
	}
}
