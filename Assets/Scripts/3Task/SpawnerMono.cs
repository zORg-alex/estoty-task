using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Task3
{
	public class SpawnerMono : MonoBehaviour
	{
		public GameObject Prefab;
		public int3 Size;
	}
	public class SpawnerBaker : Baker<SpawnerMono>
	{
		public override void Bake(SpawnerMono authoring)
		{
			AddComponent(new SpawnerData
			{
				Prefab = GetEntity(authoring.Prefab),
				Size = authoring.Size
			});
		}
	}
}
