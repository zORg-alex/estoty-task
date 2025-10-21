using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphericalGravity : MonoBehaviour, ISphereCoordinatesUser {
	public bool preserveUp = true;
	new Rigidbody rigidbody;
	SphereCoordinates sc;

	public SphereCoordinates SphereCoordinates { get; set; }

	private void OnEnable()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
	}

	private void FixedUpdate()
	{
		rigidbody.freezeRotation = preserveUp;
		rigidbody.AddForce(SphereCoordinates.GetDown(transform) * 9.81f);
	}

	private void LateUpdate()
	{
		if (preserveUp)
			transform.rotation = SphereCoordinates.GetForwardHorizontalRotation(transform);
	}

#if UNITY_EDITOR
	[ContextMenu("ApplyRotation")]
	public void ApplyRotation()
	{
		LateUpdate();
	}
#endif
}
