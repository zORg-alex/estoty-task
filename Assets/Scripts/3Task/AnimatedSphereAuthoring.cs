using Unity.Entities;
using UnityEngine;

public class AnimatedSphereAuthoring : MonoBehaviour
{
	public Vector3 InitialPosition;
}

public class AnimatedSphereBaker : Baker<AnimatedSphereAuthoring>
{
	public override void Bake(AnimatedSphereAuthoring authoring)
	{
		AddComponent(new AnimatedSphere() { InitialPosition = authoring.InitialPosition });
	}
}