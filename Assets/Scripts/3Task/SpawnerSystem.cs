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
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<SpawnerData>();
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{
			spawned = false;
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!spawned)
			{
				var spawner = SystemAPI.GetSingletonEntity<SpawnerData>();
				var spawnerAspect = SystemAPI.GetAspectRO<SpawnerAspect>(spawner);
				SpawnPrefabs(state, spawnerAspect);
				spawned = true;
			}
			else
			{
				AnimateColors(state);
			}
		}

		private void SpawnPrefabs(SystemState state, SpawnerAspect spawnerAspect)
		{
			var offset = spawnerAspect.Position - spawnerAspect.Size / 2;
			var ecb = new EntityCommandBuffer(Allocator.Temp);
			for (int i = 0; i < spawnerAspect.Size.x; i++)
				for (int j = 0; j < spawnerAspect.Size.x; j++)
					for (int k = 0; k < spawnerAspect.Size.x; k++)
					{
						var entity = ecb.Instantiate(spawnerAspect.Prefab);
						ecb.SetComponent(entity, GetLocalTransform(i, j, k, offset, .5f));
						ecb.SetComponent(entity, new SphereData { InitialPosition = new float3(i, j, k) });
					}
			ecb.Playback(state.EntityManager);
		}

		private void AnimateColors(SystemState state)
		{
			foreach (var s in SystemAPI.Query<SphereAspect>())
			{
				s.SetRandomColor();
			}
		}

		public LocalTransform GetLocalTransform(float x, float y, float z, float3 offset, float scale) =>
			new LocalTransform() {
				Position = offset + new float3(x, y, z),
				Rotation = quaternion.identity,
				Scale = scale
			};
	}
}
