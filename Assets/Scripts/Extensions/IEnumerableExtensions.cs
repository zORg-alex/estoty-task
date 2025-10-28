using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scripts.Extensions
{
	public static class IEnumerableExtensions
	{
		[DebuggerStepThrough]
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}

		[DebuggerStepThrough]
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
		{
			int i = 0;
			foreach (var item in enumerable)
			{
				action(item, i++);
			}
		}

		public static bool TryFind<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T result)
		{
			foreach (T item in source)
			{
				if (predicate(item))
				{
					result = item;
					return true;
				}
			}

			result = default(T);
			return false;
		}

		public static bool TryFind<T>(this IEnumerable<T> source, Func<T, bool> predicate, out T result, out int index)
		{
			index = 0;
			foreach (T item in source)
			{
				if (predicate(item))
				{
					result = item;
					return true;
				}

				index++;
			}

			result = default(T);
			return false;
		}
	}
}