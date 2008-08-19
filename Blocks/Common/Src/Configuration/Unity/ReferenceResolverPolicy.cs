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

using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Dependency resolver that resolves to a new builder request for a configured key.
	/// </summary>
	public sealed class ReferenceResolverPolicy : IDependencyResolverPolicy
	{
		private readonly NamedTypeBuildKey referenceKey;

		/// <summary>
		/// Initializes a new instance of the class <see cref="ReferenceResolverPolicy"/>
		/// with a given key.
		/// </summary>
		/// <param name="referenceKey">The key to use when resolving a dependency.</param>
		public ReferenceResolverPolicy(NamedTypeBuildKey referenceKey)
		{
			this.referenceKey = referenceKey;
		}

		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			// clones the current context and resolves a new object with the configured key.
			IBuilderContext recursiveContext = context.CloneForNewBuild(this.referenceKey, null);
			return recursiveContext.Strategies.ExecuteBuildUp(recursiveContext);
		}
	}
}