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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// A <see cref="IDependencyResolverPolicy"/> that will convert a collection of string names
	/// into a collection of objects resolved from the container with the supplied names.
	/// </summary>
	public sealed class ReferenceCollectionResolverPolicy : IDependencyResolverPolicy
	{
		private readonly Type collectionType;
		private readonly Type elementType;
		private readonly IEnumerable<string> keys;

		/// <summary>
		/// Initializes a new instance of the class <see cref="ReferenceCollectionResolverPolicy"/>.
		/// </summary>
		/// <param name="collectionType">The type of the collection to return when resolving the dependency.</param>
		/// <param name="elementType">The (base) type of elements to resolve into the collection.</param>
		/// <param name="keys">The list of keys to resolve.</param>
		public ReferenceCollectionResolverPolicy(Type collectionType, Type elementType, IEnumerable<string> keys)
		{
			this.collectionType = collectionType;
			this.elementType = elementType;
			this.keys = keys;
		}

		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			IList result = (IList)Activator.CreateInstance(collectionType);

			foreach (string key in keys)
			{
				IBuilderContext buildContext = context.CloneForNewBuild(new NamedTypeBuildKey(elementType, key), null);
				result.Add(buildContext.Strategies.ExecuteBuildUp(buildContext));
			}

			return result;
		}
	}
}