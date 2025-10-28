using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Extensions
{
	public static class UnityEngineUIExtensions
	{
		public static void SetSprite(this Image image, Sprite sprite) => image.sprite = sprite;
	}
}
