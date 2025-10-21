using System;
using System.Collections;
using Scripts.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, ISphereCoordinatesUser {
	private Rigidbody _rigidbody;
	[SerializeField] Transform moveAlongTarget;
	[SerializeField] float movementSpeed = .6f;
	[SerializeField] float lerpSmoothing = .3f;
	[SerializeField] float rotationSpeed = 720f;
	private Vector3 _moveVector;
	private Vector3 _allignedMoveVector;
	private Quaternion _desiredRotation;

	public SphereCoordinates SphereCoordinates { get; set; }

	private void OnEnable()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void Move(Vector2 input)
	{
		//Convert input vector
		var m = new Vector3(input.x, 0, input.y).normalized * movementSpeed;
		//Smooth it
		_moveVector = Vector3.Lerp(_moveVector, m, lerpSmoothing);
	}

	private void FixedUpdate()
	{
		this.ClearGizmos();
		
		if (!moveAlongTarget) { Debug.LogError("MoveAlongTarget missing"); return; }
		if (_moveVector.sqrMagnitude > .00001f)
		{
			_allignedMoveVector = SphereCoordinates.CalculateCurvedHorizontalVector(transform.position, moveAlongTarget.rotation * _moveVector * Time.fixedDeltaTime);
			_desiredRotation = Quaternion.LookRotation(_allignedMoveVector, SphereCoordinates.GetUp(transform.position));
			
		}
		else
			_allignedMoveVector = Vector3.zero;

		var rotation =
			Quaternion.RotateTowards(_rigidbody.rotation, _desiredRotation, rotationSpeed * Time.fixedDeltaTime);
		_rigidbody.Move(transform.position + _allignedMoveVector, rotation);
		
		this.DrawLineGizmo(Color.blue, transform.position, transform.position + rotation * Vector3.forward);
		this.DrawLineGizmo( Color.teal, transform.position, transform.position + _desiredRotation * Vector3.forward);
		this.DrawLineGizmo(transform.position, transform.position + _allignedMoveVector * 100f);
	}

	private void OnDrawGizmos()
	{
		this.DrawGizmos();
	}

	public Vector3 TargetDirection => _allignedMoveVector.normalized;
}
