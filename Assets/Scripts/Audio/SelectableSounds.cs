using Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectableSounds : MonoBehaviour, ISelectHandler, IPointerClickHandler
{
	[SerializeField] private SelectableSoundsSO settings;


	public void OnPointerClick(PointerEventData eventData)
	{
		if (settings && settings.Clicked.Length > 0)
		{
			var clip = settings.Clicked[Random.Range(0, settings.Clicked.Length)];
			AudioController.Instance.PlayUIClip(clip);
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (settings && settings.Selected.Length > 0)
		{
			var clip = settings.Selected[Random.Range(0, settings.Selected.Length)];
			AudioController.Instance.PlayUIClip(clip);
		}
	}
}