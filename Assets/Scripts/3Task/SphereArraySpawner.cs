using Unity.Entities;
using Unity.Mathematics;

public struct SphereArraySpawner : IComponentData
{
	public Entity Prefab;
	public int3 Size;
}
