using System;
using UnityEngine;
using Scripts.Input;
using Scripts.Extensions;
using Scripts.MainSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class MenuView : UIContext, IInputBack, IInputEsc
	{
		[SerializeField] private Transform mainMenuBackground;
		[SerializeField] private Button returnButton;
		[SerializeField] private Button task2022button;
		[SerializeField] private Button task2025button;
		
		private Action _onShow = () => { };
		private Action _onHide = () => { };
		public event Action OnShowEvent { add => _onShow += value; remove => _onShow -= value; }
		public event Action OnHideEvent { add => _onHide += value; remove => _onHide -= value; }
		
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
			
			UpdateButtonsVisibility();
			task2022button.onClick.AddListener(Load2022);
			task2025button.onClick.AddListener(Load2025);
        }

		private void Load2025()
		{
			LevelSelector.Instance.Load2025();
			Hide();
		}

		private void Load2022()
		{
			LevelSelector.Instance.Load2022();
			Hide();
		}

		private void UpdateButtonsVisibility()
		{
			if (LevelSelector.Instance.NothingLoaded)
			{
				returnButton.gameObject.SetActive(false);
				mainMenuBackground?.gameObject.SetActive(true);
			}
			else
			{
				returnButton.gameObject.SetActive(true);
				mainMenuBackground?.gameObject.SetActive(false);
			}

			if (LevelSelector.Instance.Scene2022Loaded)
			{
				task2022button.gameObject.SetActive(false);
				task2025button.gameObject.SetActive(true);
			}

			if (LevelSelector.Instance.Scene2025Loaded)
			{
				task2022button.gameObject.SetActive(true);
				task2025button.gameObject.SetActive(false);
			}
		}

		public virtual void OnBack() => Hide();
		public virtual void OnEsc()
		{
			if (Interactable)
				Hide();
			else
				Show();
		}

		public override void OnShow()
		{
			UpdateButtonsVisibility();
			InputSystem.Instance.FocusedContext = gameObject;
			_onShow.Invoke();
		}

		public override void OnHide()
		{
			_onHide.Invoke();
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