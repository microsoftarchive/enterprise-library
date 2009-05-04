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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Base class for custom strategies used with ObjectBuilder. Provides methods to retrieve useful information from the context.
	/// </summary>
	public abstract class EnterpriseLibraryBuilderStrategy : BuilderStrategy
	{
		/// <summary>
		/// Returns the <see cref="IConfigurationSource"/> instance from the context.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The <see cref="IConfigurationSource"/> in the context, or a new <see cref="SystemConfigurationSource"/> 
		/// if there is no such source in the context.</returns>
		protected static IConfigurationSource GetConfigurationSource(IBuilderContext context)
		{
			IConfigurationObjectPolicy policy 
				= context.Policies.Get<IConfigurationObjectPolicy>(typeof(IConfigurationSource));

			if (policy == null)
				return new SystemConfigurationSource();
			else
				return policy.ConfigurationSource;
		}

		/// <summary>
		/// Returns the <see cref="ConfigurationReflectionCache"/> instance from the context.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The <see cref="ConfigurationReflectionCache"/> in the context, or a new instance
		/// if there is no such cache in the context.</returns>
		protected static ConfigurationReflectionCache GetReflectionCache(IBuilderContext context)
		{
			IReflectionCachePolicy policy
				= context.Policies.Get<IReflectionCachePolicy>(typeof(IReflectionCachePolicy));

			if (policy == null)
				return new ConfigurationReflectionCache();
			else
				return policy.ReflectionCache;
		}
	}
}
