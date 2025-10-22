using Scripts.Extensions;
using UnityEngine;

public class Hole : MonoBehaviour
{
	private MaterialTween _tween;
	private void Start() => Initialize();
	private void OnEnable() => this.OnAssemblyReload(Initialize);

	private void Initialize()
	{
		TryGetComponent(out _tween);
	}
}