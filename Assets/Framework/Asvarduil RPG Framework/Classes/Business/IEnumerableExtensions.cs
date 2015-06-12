using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public static class IEnumerableExtensions
{
	#region Methods

	/// <summary>
	/// Determines whether the collection is null or contains no elements.
	/// </summary>
	/// <typeparam name="T">The IEnumerable type.</typeparam>
	/// <param name="enumerable">The enumerable, which may be null or empty.</param>
	/// <returns>
	///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
	{
		if (enumerable == null)
		{
			return true;
		}

		/* If this is a list, use the Count property for efficiency. 
         * The Count property is O(1) while IEnumerable.Count() is O(N). */
		ICollection<T> collection = enumerable as ICollection<T>;
		if (collection != null)
		{
			return collection.Count < 1;
		}

		return ! enumerable.Any();
	}

    /// <summary>
    /// Copies a list of ICloneable objects to a new list.
    /// </summary>
    /// <typeparam name="T">The type of the IEnumerable; this type must implement ICloneable.</typeparam>
    /// <param name="enumerable">The enumerable, which may not be null</param>
    /// <returns>A deep copy of the given list.</returns>
    public static List<T> CopyList<T>(this IEnumerable<T> enumerable)
        where T : ICloneable
    {
        if (enumerable == null)
            throw new ArgumentNullException("enumerable");

        var result = new List<T>();
        foreach (var current in enumerable)
        {
            T clone = (T) current.Clone();
            result.Add(clone);
        }

        return result;
    }

	#endregion Methods
}
