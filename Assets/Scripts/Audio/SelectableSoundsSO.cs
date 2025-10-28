using UnityEngine;

[CreateAssetMenu(fileName = "SelectableSounds", menuName = "ScriptableObjects/SelectableSounds")]
public class SelectableSoundsSO : ScriptableObject
{
	[SerializeField] private AudioClip[] selected;
	[SerializeField] private AudioClip[] clicked;
	public AudioClip[] Selected => selected;
	public AudioClip[] Clicked => clicked;
}