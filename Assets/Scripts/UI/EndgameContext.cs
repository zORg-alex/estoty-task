using System;
using Cysharp.Threading.Tasks;
using Scripts.Extensions;
using Scripts.Input;
using UnityEngine;

namespace Scripts.UI
{
	public class EndgameContext : UIContext, IInputBack, IInputEsc
	{
		private static EndgameContext _instance;
		public static EndgameContext Instance => Singletons.GetOrCreateInstanceInScene(ref _instance);

		[SerializeField] private TextProvider text;
		private Action _onHidden;
		
		private void Start() => Initialize();
		private void OnEnable() => this.OnAssemblyReload(Initialize);

		private void Initialize()
		{
			if (this.OnEnableDestroyIfCopy(ref _instance)) return;
			this.RegisterInInputBack();
			this.RegisterInInputEsc();
		}

		public void ShowScore(int score, Action onAfterClosed)
		{
			text?.SetText(score);
			_onHidden = onAfterClosed;
			Show();
		}

		public override void OnShow()
		{
			InputSystem.Instance.FocusedContext = gameObject;
		}

		public void OnBack() => Hide();
		public void OnEsc() => Hide();

		public override void OnHide()
		{
			_onHidden?.Invoke();
			_onHidden = null;
			UniTask.DelayFrame(1, PlayerLoopTiming.Update)
				.ContinueWith(MenuView.Instance.Show)
				.Forget();
		}
	}
}