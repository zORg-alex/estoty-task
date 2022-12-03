using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Rigidbody))]
public class SphericalGravity : MonoBehaviour {
	public bool preserveUp = true;
	new Rigidbody rigidbody;
	SphereCoordinates sc;
	private void OnEnable()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.freezeRotation = true;
		sc = GetComponentInParent<SphereCoordinates>();
	}

	private void FixedUpdate()
	{
		rigidbody.AddForce(sc.GetDown(transform) * 9.81f);
	}

	private void LateUpdate()
	{
		transform.rotation = sc.GetForwardHorizontalRotation(transform);
	}
}
