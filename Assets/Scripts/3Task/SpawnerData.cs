using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts.Task3
{
	public struct SpawnerData : IComponentData
	{
		public Entity Prefab;
		public int3 Size;
	}
}
