using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ResourceTransferScript : MonoBehaviour
{
	private VisualEffect vfx;
	public Transform target;

	private void OnEnable() => vfx = GetComponent<VisualEffect>();
	public void StartParticles() => vfx.SendEvent("StartParticles");
	public void StopParticles() => vfx.SendEvent("StopParticles");

	public IEnumerator SnapTarget(Transform transform)
	{
		while (true)
		{
			target.position = transform.position;
			yield return null;
		}
	}
}
