using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class SpawnerAuthoring : MonoBehaviour
{
	public AnimatedSphereAuthoring Prefab;
	public Vector3Int Size;
}

class SpawnerBaker : Baker<SpawnerAuthoring>
{
	public override void Bake(SpawnerAuthoring authoring)
	{
		AddComponent(new SphereArraySpawner
		{
			// By default, each authoring GameObject turns into an Entity.
			// Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
			Prefab = GetEntity(authoring.Prefab),
			Size = new int3(authoring.Size.x, authoring.Size.y, authoring.Size.z)
		});
	}
}
