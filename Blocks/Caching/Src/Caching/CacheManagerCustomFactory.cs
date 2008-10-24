//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build an instance of <see cref="ICacheManager"/> described by a <see cref="CacheManagerDataBase"/> configuration object.
	/// </summary>
	public class CacheManagerCustomFactory : AssemblerBasedCustomFactory<ICacheManager, CacheManagerDataBase>
	{
		/// <summary>
		/// Returns the configuration object that represents the named <see cref="ICacheManager"/> instance in the specified <paramref name="configurationSource"/>.
		/// </summary>
		/// <param name="name">The name of the required instance.</param>
		/// <param name="configurationSource">The configuration source where to look for the configuration object.</param>
		/// <returns>The configuration object that represents the instance with name <paramref name="name"/> in the cache 
		/// configuration section from <paramref name="configurationSource"/></returns>
		protected override CacheManagerDataBase GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			CachingConfigurationView view = new CachingConfigurationView(configurationSource);
			return view.GetCacheManagerData(name);
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="ICacheManager"/> described by the <see cref="CacheManagerDataBase"/> configuration section.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="ICacheManager"/>.</returns>
		public override ICacheManager Create(IBuilderContext context, CacheManagerDataBase objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ICacheManager manager = base.Create(context, objectConfiguration, configurationSource, reflectionCache);
			
			RegisterObject(context, objectConfiguration.Name, manager);

			return manager;
		}

		private void RegisterObject(IBuilderContext context, string name, ICacheManager createdObject)
		{
			if (context.Locator != null)
			{
				context.Locator.Add(new NamedTypeBuildKey(typeof(ICacheManager), name), createdObject);
			}
			if (context.Lifetime != null)
			{
				context.Lifetime.Add(createdObject);
			}
		}		
	}
}
