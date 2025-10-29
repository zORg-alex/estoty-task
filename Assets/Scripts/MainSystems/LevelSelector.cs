using Scripts.Audio;
using Scripts.Extensions;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.MainSystems
{
    public class LevelSelector : MonoBehaviour
    { 
        [Scene, SerializeField] private string task2022;
        [Scene, SerializeField] private string task2025;
        public bool NothingLoaded { get; private set; }
        public bool Scene2022Loaded { get; private set; }
        public bool Scene2025Loaded { get; private set; }
        private static LevelSelector _instance;
        public static LevelSelector Instance => Singletons.GetOrCreateInstanceInScene(ref _instance);

        private void Start() => Initialize();
        private void OnEnable()
        {
            if (this.OnEnableDestroyIfCopy(ref _instance)) return;
            this.OnAssemblyReload(Initialize);
        }

        private void Initialize()
        {
            NothingLoaded = SceneManager.loadedSceneCount == 1;
            Scene2022Loaded = SceneManager.GetSceneByPath(task2022).isLoaded;
            Scene2025Loaded = SceneManager.GetSceneByPath(task2025).isLoaded;
        }


        public void Load2022()
        {
            SceneManager.LoadScene(task2022);
        }

        public void Load2025()
        {
            SceneManager.LoadScene(task2025);
        }
    }
}
