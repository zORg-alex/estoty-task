
using System;
using System.Collections.Generic;

public static class IEnumerableExtensions
{
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