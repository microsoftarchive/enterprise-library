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
	/// Dependency resolver that resolves to a constant value.
	/// </summary>
	public sealed class ConstantResolverPolicy : IDependencyResolverPolicy
	{
		private readonly object value;

		/// <summary>
		/// Initializes a new instance of the class <see cref="ConstantResolverPolicy"/>.
		/// </summary>
		/// <param name="value">The constant value to resolve to.</param>
		public ConstantResolverPolicy(object value)
		{
			this.value = value;
		}

		object IDependencyResolverPolicy.Resolve(IBuilderContext context)
		{
			return this.value;
		}
	}
}
