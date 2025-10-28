using Scripts.Extensions;
//using Scripts.Input;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class TabbedMenuView : MenuView//, IInputTabPrevNext
	{
		[SerializeField]
		private Button prevTab;
		[SerializeField]
		private Button nextTab;
		[SerializeField]
		private List<RectTransform> tabs = new();
		private RectTransform _currentTab;
		protected override void OnInitialize()
		{
			base.OnInitialize();
			//this.RegisterInInputTabPrevNext();
			nextTab.onClick.AddListener(OnTabNext);
			prevTab.onClick.AddListener(OnTabPrev);

			_currentTab = tabs.FirstOrDefault();
		}

		// [HorizontalGroup("TabGroup"), Button]
		public void OnTabPrev()
		{
			if (tabs.Count == 0) return;
			var ind = tabs.IndexOf(_currentTab);
			if (ind == -1)
				_currentTab = tabs.FirstOrDefault();
			else
			{
				ind = (ind - 1 + tabs.Count) % tabs.Count;
				_currentTab = tabs[ind];
			}
			UpdateTabVisibility();
		}
		// [HorizontalGroup("TabGroup"), Button]
		public void OnTabNext()
		{
			if (tabs.Count == 0) return;
			var ind = tabs.IndexOf(_currentTab);
			if (ind == -1)
				_currentTab = tabs.FirstOrDefault();
			else
			{
				ind = (ind + 1) % tabs.Count;
				_currentTab = tabs[ind];
			}
			UpdateTabVisibility();
		}

		private void UpdateTabVisibility()
		{
			foreach (var tab in tabs)
			{
				if (_currentTab.IsNullOnly())
					_currentTab = tab;
				tab.gameObject.SetActive(_currentTab == tab);
			}
		}

	}
	public abstract class TabView : MonoBehaviour
	{
		[SerializeField]
		private bool isVisible;
		public bool IsVisible => isVisible;
		private void Start() => OnInitialize();
		private void OnEnable() => this.OnAssemblyReload(OnInitialize);

		protected abstract void OnInitialize();
		void SetVisible(bool visibility)
		{
			//to sync IsVisible property, just in case
			gameObject.SetActive(visibility);
			if (isVisible == visibility) return;
			if (visibility)
			{
				isVisible = true;
				OnShow();
			}
			else
			{
				isVisible = false;
				OnHide();
			}
		}
		public abstract void OnShow();
		public abstract void OnHide();
	}
}