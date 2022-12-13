using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		foreach (var spawner in SystemAPI.Query<RefRO<SphereArraySpawner>>())
		{
			SpawnArray(state, spawner);
		}
	}

	private static void SpawnArray(SystemState state, RefRO<SphereArraySpawner> spawner)
	{
		var size = spawner.ValueRO.Size;
		for (int i = 0; i < size.x; i++)
			for (int j = 0; j < size.y; j++)
				for (int k = 0; k < size.z; k++)
				{
					//Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
					//state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(new float3(i, j, k)));

					var entity = state.EntityManager.CreateEntity();
					AnimatedSphere componentData = new AnimatedSphere() { InitialPosition = new float3(i, j, k) };
					state.EntityManager.AddComponentData(entity, componentData);
					state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(componentData.InitialPosition));
				}
	}

	public void OnDestroy(ref SystemState state) { }

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		//foreach (RefRW<AnimatedSphere> s in SystemAPI.Query<RefRW<AnimatedSphere>>())
		//{

		//}
	}

	//private void ProcessSpawner(ref SystemState state, RefRW<SphereArraySpawner> spawner)
	//{
	//	// If the next spawn time has passed.
	//	if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
	//	{
	//		// Spawns a new entity and positions it at the spawner.
	//		Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
	//		state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));

	//		// Resets the next spawn time.
	//		spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
	//	}
	//}
}

public struct Component : IComponentData
{

}