using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts.Task3
{
	readonly partial struct SphereAspect : IAspect
	{
		public readonly Entity Self;
		readonly TransformAspect Transform;
		readonly RefRO<SphereData> Sphere;
		public float3 InitialPosition => Sphere.ValueRO.InitialPosition;
	}
}
