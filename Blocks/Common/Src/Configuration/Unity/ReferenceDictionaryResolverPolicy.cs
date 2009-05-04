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
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// A <see cref="IDependencyResolverPolicy"/> that will convert a collection of string names and 
	/// into a dictionary of objects resolved from the container with the supplied names.
	/// </summary>
	/// <typeparam name="TDictionary">The type of dictionary to create when resolving.</typeparam>
	/// <typeparam name="T">The type of object to resolve.</typeparam>
	/// <typeparam name="TKey">The type of the suppliled keys.</typeparam>
	[SuppressMessage("Microsoft.Design", "CA1005",
		Justification = "Three types are needed here to achieve type safety.")]
	public sealed class ReferenceDictionaryResolverPolicy<TDictionary, T, TKey> : IDependencyResolverPolicy
		where TDictionary : IDictionary<TKey, T>, new()
	{
		private readonly IEnumerable<KeyValuePair<string, TKey>> dependencyKeys;

		/// <summary>
		/// Initializes a new instance of the class <see cref="ReferenceDictionaryResolverPolicy{TDictionary, T, TKey}"/>.
		/// </summary>
		/// <param name="dependencyKeys">The list of names and keys.</param>
		public ReferenceDictionaryResolverPolicy(IEnumerable<KeyValuePair<string, TKey>> dependencyKeys)
		{
			this.dependencyKeys = dependencyKeys;
		}

		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			TDictionary dictionary = new TDictionary();

			foreach (KeyValuePair<string, TKey> keyPair in dependencyKeys)
			{
				IBuilderContext buildContext = context.CloneForNewBuild(NamedTypeBuildKey.Make<T>(keyPair.Key), null);
				T createdElement = (T)buildContext.Strategies.ExecuteBuildUp(buildContext);
				dictionary.Add(keyPair.Value, createdElement);
			}

			return dictionary;
		}
	}
}
