using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Task3
{
	public class SphereMono : MonoBehaviour
	{
		public float3 InitialPosition;
	}
	public class SphereBaker : Baker<SphereMono>
	{
		public override void Bake(SphereMono authoring)
		{
			AddComponent(new SphereData());
		}
	}
}
