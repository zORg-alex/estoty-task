using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Scripts.Extensions
{
    public static class Singletons
    {
    	[DebuggerStepThrough]
        public static T GetOrFindByType<T>(ref T instance) where T : MonoBehaviour
        {
            if (!instance)
                instance = GameObject.FindAnyObjectByType<T>();
            return instance;
        }

        public static T GetOrCreateInstanceInScene<T>(ref T instance, T prefab = null, Transform parent = null) where T : MonoBehaviour
        {
            if (!GetOrFindByType(ref instance))
            {
                if (prefab == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    instance.transform.parent = parent;
                }
                else
                    instance = GameObject.Instantiate(prefab, parent);
            }

            return instance;
    	}
    	[DebuggerStepThrough]
    	public static T GetFirstInstanceInResources<T>(ref T instance) where T : UnityEngine.Object
    	{
    		if (!instance)
    			instance = Resources.FindObjectsOfTypeAll<T>().First();
    		return instance;
    	}

    	/// <summary>
    	/// Destroy only this component if it's a copy
    	/// <code>
    	/// private static T _instance;
    	/// public static T Instance => Singletons.GetOrFindByType(ref _instance);
    	/// private void OnEnable()
    	/// {
    	/// 	if (this.OnEnableDestroyIfCopy(ref _instance)) return;
    	/// }</code>
    	/// </summary>
    	/// <returns>true if destroyed</returns>
    	public static bool OnEnableDestroyIfCopy<T>(this T @this, ref T instance) where T : UnityEngine.Object
    	{
    		if (instance == null) instance = @this;
    		if (@this == instance) return false;
    		GameObject.Destroy(@this);
            return true;
    	}
    	/// <summary>
    	/// Destroy whole GameObject if it's a copy
    	/// <_code>
    	/// private static T _instance;
    	/// public static T Instance => Singletons.GetOrFindByType(ref _instance);
    	/// private void OnEnable()
    	/// {
    	/// 	if (this.OnEnableDestroyIfCopy(ref _instance)) return;
    	/// }</_code>
    	/// </summary>
    	/// <returns>true if destroyed</returns>
    	public static bool OnEnableDestroyWholeIfCopy<T>(this T @this, ref T instance) where T : MonoBehaviour
    	{
    		if (instance == null) instance = @this;
    		if (@this == instance) return false;
    		GameObject.Destroy(@this.gameObject);
    		return true;
    	}
    }
}
