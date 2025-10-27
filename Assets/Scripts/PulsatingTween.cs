using PrimeTween;
using TriInspector;
using UnityEngine;

public class PulsatingTween : MaterialTween
{
	[Button]
	public override void StartTweenTo(Color color)
	{
		Sequence.Create()
			.Group(Tween.Custom(Color.black, color * 2f, .5f, c => SetMaterialProperty(c, Property.EmissionColor), Ease.InOutCubic, 4, CycleMode.Yoyo))
			.Chain(Tween.Custom(Color.black, color * 2f, .25f, c => SetMaterialProperty(c, Property.EmissionColor), Ease.InOutCubic, 8, CycleMode.Yoyo))
			.Chain(Tween.Custom(Color.black, color * 2f, .125f, c => SetMaterialProperty(c, Property.EmissionColor), Ease.InOutCubic, 16, CycleMode.Yoyo))
			.Insert(5.5f, Tween.Custom(currentColor, color, .5f, SetColor))
			.OnComplete(OnAfterTweening);
	}
}