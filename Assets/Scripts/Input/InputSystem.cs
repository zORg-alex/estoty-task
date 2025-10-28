using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Scripts.Extensions;
using TriInspector;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Scripts.Input
{
	[DeclareFoldoutGroup("ev", Title = "Events")]
	[ExecuteAlways]
	public class InputSystem : MonoBehaviour
	{
		[SerializeField] private InputActionReference refEscAction;
		[SerializeField] private InputActionReference refBackAction;
		[SerializeField] private InputActionReference refNextAction;
		[SerializeField] private InputActionReference refPrevAction;

		private static InputSystem _instance;

		public static InputSystem Instance
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return Singletons.GetOrFindByType(ref _instance); }
		}

		private void OnEnable()
		{
			if (this.OnEnableDestroyWholeIfCopy(ref _instance)) return;

			Subscribe();
			this.OnAssemblyReload(Initialize);
		}

		private void Start() => Initialize();

		private void Initialize()
		{

		}

		private void OnDisable()
		{
			Unsubscribe();
		}

		private void Subscribe()
		{
			SubscribeRegistered();
		}

		private void Unsubscribe()
		{
			Unsubscribe(_registeredInputs);
		}

		public struct InputActionContext
		{
			public InputAction Action;
			public string Name;

			public InputActionContext(InputAction action)
			{
				Action = action;
				Name = action.name;
			}

			public InputActionContext(string name)
			{
				Action = null;
				Name = name;
			}

			public static implicit operator InputActionContext(InputAction inputAction) =>
				new InputActionContext(inputAction);
		}

		//UI action events

		[Group("ev"), SerializeField] private UnityEvent _onToggleLastPanel = new UnityEvent();

		public event UnityAction OnToggleLastPanel
		{
			add => _onToggleLastPanel.AddListener(value);
			remove => _onToggleLastPanel.RemoveListener(value);
		}

		[Group("ev"), SerializeField] private UnityEvent _onToggleInventory = new UnityEvent();

		public event UnityAction OnToggleInventory
		{
			add => _onToggleInventory.AddListener(value);
			remove => _onToggleInventory.RemoveListener(value);
		}

		[Group("ev"), SerializeField] private UnityEvent _onTabNext = new UnityEvent();

		public event UnityAction OnTabNext
		{
			add => _onTabNext.AddListener(value);
			remove => _onTabNext.RemoveListener(value);
		}

		[Group("ev"), SerializeField] private UnityEvent _onTabPrevious = new UnityEvent();

		public event UnityAction OnTabPrevious
		{
			add => _onTabPrevious.AddListener(value);
			remove => _onTabPrevious.RemoveListener(value);
		}

		[Group("ev"), SerializeField] private UnityEvent _onToggleGameMenu = new UnityEvent();

		public event UnityAction OnToggleGameMenu
		{
			add => _onToggleGameMenu.AddListener(value);
			remove => _onToggleGameMenu.RemoveListener(value);
		}

		[Group("ev"), SerializeField] private UnityEvent _onBack = new UnityEvent();

		public event UnityAction OnBack
		{
			add => _onBack.AddListener(value);
			remove => _onBack.RemoveListener(value);
		}

		private void TabNext(InputAction.CallbackContext context)
		{
			if (_focusedContext != null)
				_onTabNext.Invoke();
		}

		private void TabPrevious(InputAction.CallbackContext context)
		{
			if (_focusedContext != null)
				_onTabPrevious.Invoke();
		}

		private void ToggleInventory(InputAction.CallbackContext obj) => _onToggleInventory.Invoke();

		private void ToggleLast(InputAction.CallbackContext obj) => _onToggleLastPanel.Invoke();

		private void ToggleGameMenu(InputAction.CallbackContext context) => _onToggleGameMenu.Invoke();

		private void Back(InputAction.CallbackContext ctx)
		{
			if (_focusedContext != null)
				_onBack.Invoke();
		}


		// Some UI integration

		public enum InputType
		{
			UIBack,
			UIEsc,
			UITabPrev,
			UITabNext
		}

		public InputAction InputActions(InputType inputType) => inputType switch
		{
			InputType.UIEsc => refEscAction,
			InputType.UIBack => refBackAction,
			InputType.UITabPrev => refPrevAction,
			InputType.UITabNext => refNextAction,
			_ => null,
		};

		public enum ActionBehaviour
		{
			Always,
			WhenFocused,
			WhenNotFocused,
			WhenNothingFocused
		}

		private List<RegisteredAction> _registeredInputs = new List<RegisteredAction>();

		private GameObject _focusedContext;

		public GameObject FocusedContext
		{
			get => _focusedContext;
			set
			{
				_focusedContext = value;
				FocusedContextId = value ? value.GetInstanceID() : 0;
			}
		}

		public int FocusedContextId { get; internal set; }

		internal static void RegisterContextOnInput(Component receiver, Action action, InputType type,
													ActionBehaviour behaviour)
		{
			if (!Application.isPlaying) return;

			RegisteredAction item = new(receiver, action, type, behaviour);
			if (Instance && !Instance._registeredInputs.Contains(item))
				Instance._registeredInputs.Add(item);
			if (Instance.enabled)
				Instance.SubscribeAction(item);
		}

		internal static void DeregisterContextOnInput(Component receiver)
		{
			if (!Instance) return;
			int id = receiver.GetInstanceID();
			List<RegisteredAction> ctxes = Instance._registeredInputs.FindAll(i => i.Id == id).ToList();

			Instance._registeredInputs.RemoveAll(ctxes.Contains);
			Instance.Unsubscribe(ctxes);
		}

		private void SubscribeRegistered()
		{
			foreach (var inp in Instance._registeredInputs)
			{
				SubscribeAction(inp);
			}
		}

		private void SubscribeAction(RegisteredAction inp)
		{
			if (InputActions(inp.Type) is InputAction a)
				a.performed += inp.Call;
		}

		private void Unsubscribe(List<RegisteredAction> inputContexts)
		{
#if UNITY_EDITOR
			if (Instance == null) return;
#endif
			foreach (var inp in inputContexts)
			{
				if (InputActions(inp.Type) is InputAction a)
					a.performed -= inp.Call;
			}
		}
	}

	internal struct RegisteredAction
	{
		public int Id;
		private readonly Component _component;
		public Action Action;
		public InputSystem.InputType Type;
		private readonly InputSystem.ActionBehaviour _behaviour;

		public RegisteredAction(Component component, Action action, InputSystem.InputType type,
								InputSystem.ActionBehaviour behaviour)
		{
			Id = component.gameObject.GetInstanceID();
			_component = component;
			Action = action;
			Type = type;
			_behaviour = behaviour;
		}

		public override bool Equals(object obj)
		{
			return obj is RegisteredAction other &&
				   Id == other.Id &&
				   EqualityComparer<Action>.Default.Equals(Action, other.Action) &&
				   Type == other.Type;
		}

		internal void Call(InputAction.CallbackContext context)
		{
			if (!_component || !_component.gameObject.activeInHierarchy) return;
			switch (_behaviour)
			{
				case InputSystem.ActionBehaviour.Always:
					Action.Invoke();
					break;
				case InputSystem.ActionBehaviour.WhenFocused:
					if (InputSystem.Instance.FocusedContextId == Id)
						Action.Invoke();
					break;
				case InputSystem.ActionBehaviour.WhenNotFocused:
					if (InputSystem.Instance.FocusedContextId != Id)
						Action.Invoke();
					break;
				case InputSystem.ActionBehaviour.WhenNothingFocused:
					if (InputSystem.Instance.FocusedContextId == 0)
						Action.Invoke();
					break;
				default:
					break;
			}
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Action, Type, _behaviour);
		}
	}

	internal struct ActionData
	{
		public Action<InputAction.CallbackContext> action;
		public int priority;
		public string name;

		public ActionData(Action<InputAction.CallbackContext> action, int priority, string name)
		{
			this.action = action;
			this.priority = priority;
			this.name = name;
		}

		public override bool Equals(object obj)
		{
			return obj is ActionData other &&
				   EqualityComparer<Action<InputAction.CallbackContext>>.Default.Equals(action, other.action) &&
				   priority == other.priority &&
				   name == other.name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(action, priority, name);
		}

		public void Deconstruct(out Action<InputAction.CallbackContext> action, out int priority, out string name)
		{
			action = this.action;
			priority = this.priority;
			name = this.name;
		}

		public static implicit operator (Action<InputAction.CallbackContext> action, int priority, string name)(
			ActionData value)
		{
			return (value.action, value.priority, value.name);
		}

		public static implicit operator ActionData(
			(Action<InputAction.CallbackContext> action, int priority, string name) value)
		{
			return new ActionData(value.action, value.priority, value.name);
		}
	}
}