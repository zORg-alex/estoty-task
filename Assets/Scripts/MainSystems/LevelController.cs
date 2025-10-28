using Scripts.Audio;
using Scripts.Extensions;
using UnityEngine;

namespace Scripts.MainSystems
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private AudioClip _levelMusic;

    	private void Start()
    	{
    		if (!Application.isPlaying) return;
    		if (!_levelMusic)
    			AudioController.Instance.StopMusic();
    		else
    			AudioController.Instance.PlayMusic(_levelMusic);
    		Initialize();
    	}

    	private void OnEnable() => this.OnAssemblyReload(Initialize);

        private void Initialize()
        {

        }
    }
}
