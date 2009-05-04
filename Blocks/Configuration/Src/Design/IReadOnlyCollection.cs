//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a collection that is readonly.
	/// </summary>
	/// <typeparam name="T">The types of objects in the collection.</typeparam>
	public interface IReadOnlyCollection<T> : IEnumerable<T>
	{
		/// <summary>
		/// When implemented by a class, gets the item based on the name.
		/// </summary>
		/// <param name="name">The name of the item to get.</param>
		/// <returns>The item based on the name.</returns>
		T this[string name] { get; }

		/// <summary>
		/// When implemented by a class, gets the item at the specified index.
		/// </summary>
		/// <param name="index">The index of the item to get.</param>
		/// <returns>The item at the specified index.</returns>
		T this[int index] { get; }

		/// <summary>
		/// When implemented by a class, gets the count of items in the collection.
		/// </summary>
		/// <value>
		/// The count of items in the collection.
		/// </value>
		int Count { get; }

		/// <summary>
		/// When implemented by a class, determines if the item is in the collection.
		/// </summary>
		/// <param name="item">The item to find.</param>
		/// <returns><c>true</c> if the item is in the collection; otherwise <c>false</c>.</returns>
		bool Contains(T item);

		/// <summary>
		/// When implemented by a class, determines if the item is in the collection based on name.
		/// </summary>
		/// <param name="nodeName">The name of the item to find.</param>
		/// <returns><c>true</c> if the item is in the collection; otherwise <c>false</c>.</returns>
		bool Contains(string nodeName);

		/// <summary>
		/// When implemented by a class, performs the specified action on each item.
		/// </summary>
		/// <param name="action">The Action to perform on each element.</param>
		void ForEach(Action<T> action);
	}
}
