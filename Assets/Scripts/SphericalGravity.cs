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
		if (preserveUp)
			rigidbody.freezeRotation = true;
	}

	private void FixedUpdate()
	{
		rigidbody.AddForce(SphereCoordinates.GetDown(transform) * 9.81f);
	}

	private void LateUpdate()
	{
		if (preserveUp)
			transform.rotation = SphereCoordinates.GetForwardHorizontalRotation(transform);
	}
}
