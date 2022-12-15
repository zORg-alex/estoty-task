using Unity.Core;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts.Task3
{
	readonly partial struct SphereAspect : IAspect
	{
		public readonly Entity Self;
		readonly TransformAspect Transform;
		readonly RefRO<SphereData> Sphere;
		readonly RefRW<URPMaterialPropertyBaseColor> BaseColor;
		readonly RefRW<URPMaterialPropertyEmissionColor> EmissionColor;
		public float3 InitialPosition => Sphere.ValueRO.InitialPosition;
		public float4 Color
		{
			get => BaseColor.ValueRO.Value;
			set
			{
				BaseColor.ValueRW.Value = value;
				EmissionColor.ValueRW.Value = value;
			}
		}
		public void SetRandomColor()
		{
			var t = Time.time;
			//Color = new float4(t % 1, 1 - t % 1, 0, 1);

			Color = new float4(.3f, noise.pnoise(InitialPosition/5f + t /5f, 15f), .7f, 1);
		}
	}
}
