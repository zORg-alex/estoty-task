using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts.Task3
{
	readonly partial struct SpawnerAspect : IAspect
	{
		public readonly Entity Self;
		readonly TransformAspect Transform;
		readonly RefRO<SpawnerData> Spawner;

		public float3 Position => Transform.WorldPosition;
		public quaternion Rotation => Transform.WorldRotation;

		public int3 Size => Spawner.ValueRO.Size;

		public Entity Prefab => Spawner.ValueRO.Prefab;
	}
}
