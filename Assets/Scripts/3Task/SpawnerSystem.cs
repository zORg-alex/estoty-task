using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts.Task3
{
	//[BurstCompile]
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial struct SpawnerSystem : ISystem
	{
		private bool spawned;
		//[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<SpawnerData>();
		}

		//[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{

		}

		//[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!spawned)
			{
				var spawner = SystemAPI.GetSingletonEntity<SpawnerData>();
				var spawnerAspect = SystemAPI.GetAspectRO<SpawnerAspect>(spawner);
				SpawnPrefabs(state, spawnerAspect);
				spawned = true;
			}
		}

		private void SpawnPrefabs(SystemState state, SpawnerAspect spawnerAspect)
		{
			var ecb = new EntityCommandBuffer(Allocator.Temp);
			for (int i = 0; i < spawnerAspect.Size.x; i++)
				for (int j = 0; j < spawnerAspect.Size.x; j++)
					for (int k = 0; k < spawnerAspect.Size.x; k++)
					{
						var entity = ecb.Instantiate(spawnerAspect.Prefab);
						ecb.SetComponent(entity, spawnerAspect.GetLocalTransform(i,j,k));
						ecb.SetComponent(entity, new SphereData { InitialPosition = new float3(i, j, k) });
					}
			ecb.Playback(state.EntityManager);
		}
	}
}
