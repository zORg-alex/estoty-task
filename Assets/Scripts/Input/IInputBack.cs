using System;
using UnityEngine;

namespace Scripts.Input
{
	public static class IInputExtension
	{
		public static void RegisterInInputBack<TBack>(this TBack self, InputSystem.ActionBehaviour behaviour = InputSystem.ActionBehaviour.WhenFocused) where TBack : MonoBehaviour, IInputBack
			=> RegisterInInput(self, self.OnBack, InputSystem.InputType.UIBack, behaviour);

		public static void RegisterInInputEsc<TEsc>(this TEsc self, InputSystem.ActionBehaviour behaviour = InputSystem.ActionBehaviour.WhenFocused) where TEsc : MonoBehaviour, IInputEsc
			=> RegisterInInput(self, self.OnEsc, InputSystem.InputType.UIEsc, behaviour);

		public static void RegisterInInputTabPrevNext<TTabPrevNext>(this TTabPrevNext self, InputSystem.ActionBehaviour behaviour = InputSystem.ActionBehaviour.WhenFocused) where TTabPrevNext : MonoBehaviour, IInputTabPrevNext
		{
			RegisterInInput(self, self.OnTabPrev, InputSystem.InputType.UITabPrev, behaviour);
			RegisterInInput(self, self.OnTabNext, InputSystem.InputType.UITabNext, behaviour);
		}

		public static void DeregisterInInput<TAny>(this TAny self) where TAny : MonoBehaviour, IInput
			=> InputSystem.DeregisterContextOnInput(self);

		private static void RegisterInInput(MonoBehaviour self, Action action, InputSystem.InputType type, InputSystem.ActionBehaviour behaviour)
		{
			InputSystem.RegisterContextOnInput(self, action, type, behaviour);
		}
	}
	public interface IInputBack : IInput
	{
		public void OnBack();
	}
	public interface IInputEsc : IInput
	{
		public void OnEsc();
	}
    public interface IInputToggleLast : IInput
    {
        public void OnToggleLast();
    }
	public interface IInputTabPrevNext : IInput
	{
		public void OnTabPrev();
		public void OnTabNext();
	}


	public interface IInput { }
}