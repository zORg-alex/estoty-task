using UnityEngine;
using Scripts.Input;
using Scripts.Extensions;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Scripts.UI
{
	public class MenuView : UIContext, IInputBack, IInputEsc
	{
		private static MenuView _instance;
		public static MenuView Instance => Singletons.GetOrCreateInstanceInScene(ref _instance);
		protected override void OnInitialize()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			if (this.OnEnableDestroyIfCopy(ref _instance)) return;
			
			this.RegisterInInputBack();
			this.RegisterInInputEsc(InputSystem.ActionBehaviour.WhenFocused);
			this.RegisterInInputEsc(InputSystem.ActionBehaviour.WhenNothingFocused);
        }

		public virtual void OnBack() => Hide();
		public virtual void OnEsc() => Hide();

		public override void OnShow()
		{
			InputSystem.Instance.FocusedContext = gameObject;
		}

		public override void OnHide()
		{
			
		}

		public void ApplicationQuit()
		{
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
	}
}