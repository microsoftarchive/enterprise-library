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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Utility class that is used to specify the need for resolving objects when specifying policies with the
	/// <see cref="PolicyBuilder{T, TSource}"/> in a strongly typed way.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The methods in this class don't perform any useful action. Instead, using them instructs the policy builder
	/// to generate resolution policies.
	/// </para>
	/// <para>
	/// The resolution indicated by the methods in the <see cref="Resolve"/> class will be performed at build time.
	/// </para>
	/// </remarks>
	public static class Resolve
	{
		/// <summary>
		/// Resolve <paramref name="key"/> to an instance of <typeparamref name="T"/>.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static T Reference<T>(string key)
		{
			return default(T);
		}

		/// <summary>
		/// Resolve <paramref name="key"/> to an instance of <typeparamref name="T"/> if
		/// key is not empty; otherwise rsolve to <see langword="null"/>.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static T OptionalReference<T>(string key)
		{
			return default(T);
		}

		/// <summary>
		/// Resolve to a new instance of collection type <typeparamref name="TCollection"/>
		/// populated with the result of resolving instances of type <typeparamref name="T"/>
		/// with the names in <paramref name="keys"/>.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to a collection TCollection of elements of type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static TCollection ReferenceCollection<TCollection, T>(IEnumerable<string> keys)
			where TCollection : ICollection<T>, new()
		{
			return default(TCollection);
		}

		/// <summary>
		/// Resolve to a new instance of dictionary type <typeparamref name="TDictionary"/>
		/// populated with the result of resolving instances of type <typeparamref name="T"/>
		/// with the names in <paramref name="keys"/>.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to indicate the resolution to a dictionary TDictionary of elements of type T")]
		[SuppressMessage("Microsoft.Usage", "CA1801",
			Justification = "The purpose of this method is to indicate the resolution to type T, parameters are not used.")]
		public static TDictionary ReferenceDictionary<TDictionary, T, TKey>(IEnumerable<KeyValuePair<string, TKey>> keys)
			where TDictionary : IDictionary<TKey, T>, new()
		{
			return default(TDictionary);
		}
	}
}
