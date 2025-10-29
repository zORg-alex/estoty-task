using System;
using Scripts.Extensions;
using Sirenix.OdinInspector;
using System.Diagnostics;
using Scripts.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
	[ExecuteAlways]
	[RequireComponent(typeof(CanvasGroup), typeof(GraphicRaycaster))]
	public abstract class UIContext : MonoBehaviour
	{
		private CanvasGroup CanvasGroup { get { if (!_canvasGroup) TryGetComponent(out _canvasGroup); return _canvasGroup; } }
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private Selectable firstSelected;

		//Passes Show/Hide to anything implementing this interface
		private IOnShowHide[] _contextsOnShowHide;

        public bool Interactable
        {
            [DebuggerStepThrough]
            get => CanvasGroup.interactable;
        }

        [SerializeField]
		private Button[] backButtons = Array.Empty<Button>();
		protected UIContext PreviousContext;
		protected bool LockedIn;

		private void Awake()
		{
			if (Interactable && firstSelected && EventSystem.current)
				EventSystem.current.firstSelectedGameObject = firstSelected.gameObject;
			_contextsOnShowHide = GetComponents<IOnShowHide>();
			if (Interactable)
				Show();
		}

		private void Start() => Initialize();
		private void OnEnable() => this.OnAssemblyReload(Initialize);

		protected virtual void OnInitialize() { }
		private void Initialize()
		{
			foreach (var b in backButtons)
			{
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(Hide);
			}
			OnInitialize();
		}

		internal void ToggleShowHide()
		{
			if (Interactable)
				Hide();
			else
				Show();
		}

		[Button]
		public void Hide()
		{
			if (LockedIn)
				return;

			CanvasGroup.alpha = 0f;
			CanvasGroup.interactable = false;
			CanvasGroup.blocksRaycasts = false;

			_contextsOnShowHide?.ForEach(c => { if (c != null) c.OnHide(); });

			if (PreviousContext)
			{
				PreviousContext.Show();
			}

			OnHide();
		}

		public void Show(UIContext previousContext)
		{
			if (previousContext)
			{
				PreviousContext = previousContext;
				PreviousContext.SetInteractable(false);
			}
			InputSystem.Instance.FocusedContext = gameObject;
			Show();
		}

		protected void SetInteractable(bool value)
		{
			CanvasGroup.interactable = value;
		}

		[Button]
		public void Show()
		{
			CanvasGroup.alpha = 1f;
			CanvasGroup.interactable = true;
			CanvasGroup.blocksRaycasts = true;

			if (firstSelected && EventSystem.current)
				EventSystem.current.firstSelectedGameObject = firstSelected.gameObject;

			_contextsOnShowHide?.ForEach(c=>c.OnShow());
			if (Interactable && firstSelected && EventSystem.current)
				EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);

			OnShow();
		}
		
		public void ShowIfHidden()
		{
			if (!CanvasGroup.interactable && !CanvasGroup.blocksRaycasts && CanvasGroup.alpha == 0f)
				Show();
		}
		
		public void HideIfShown()
		{
			
			if (CanvasGroup.interactable && CanvasGroup.blocksRaycasts && Mathf.Approximately(CanvasGroup.alpha, 1f))
				Hide();
		}
		

		public virtual void OnShow() { }
		public virtual void OnHide() { }

		public void SetFirstSelected(Selectable selectable)
		{
			firstSelected = selectable;
		}
	}

	public interface IOnShowHide {
		void OnShow();
		void OnHide();
	}
}