using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Extensions
{
	[Serializable]
	public class UnityEventInt : UnityEvent<int> { }
	
	[Serializable]
	public class UnityEventTimeSpan :  UnityEvent<TimeSpan> { }
}