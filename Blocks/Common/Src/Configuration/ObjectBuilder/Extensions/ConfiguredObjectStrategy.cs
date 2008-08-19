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
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="IBuilderStrategy"/> which creates objects.
	/// </summary>
	/// <remarks>
	/// <para>The strategy looks for the <see cref="CustomFactoryAttribute">CustomFactory</see> attribute to 
	/// retrieve the <see cref="ICustomFactory"/> implementation needed to build the requested types based on 
	/// configuration.</para>
	/// <para>The provided context must have a <see cref="ConfigurationObjectPolicy"/> holding a <see cref="IConfigurationSource"/>
	/// where to request the configuration information.</para>
	/// </remarks>
	/// <seealso cref="ICustomFactory"/>
	/// <seealso cref="CustomFactoryAttribute"/>
	/// <seealso cref="ConfigurationObjectPolicy"/>
	public class ConfiguredObjectStrategy : EnterpriseLibraryBuilderStrategy
	{
		/// <summary>
		/// Override of <see cref="IBuilderStrategy.PreBuildUp"/>. 
		/// Creates the requested object using the custom factory associated to the type specified by the context's key,
		/// and updates the context's existing object.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <exception cref="InvalidOperationException"> when the requested type does not have the 
		/// required <see cref="CustomFactoryAttribute">CustomFactory</see> attribute.</exception>
		/// <exception cref="System.Configuration.ConfigurationErrorsException"> when the configuration for the requested ID is not present or is 
		/// invalid in the configuration source.</exception>
		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);

			IConfigurationSource configurationSource = GetConfigurationSource(context);
			ConfigurationReflectionCache reflectionCache = GetReflectionCache(context);

			NamedTypeBuildKey key = (NamedTypeBuildKey) context.BuildKey;
			string id = key.Name;
			Type t = key.Type;

			ICustomFactory factory = GetCustomFactory(t, reflectionCache);
			if (factory != null)
			{
				context.Existing = factory.CreateObject(context, id, configurationSource, reflectionCache);
			}
			else
			{
				throw new InvalidOperationException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionCustomFactoryAttributeNotFound,
						t.FullName,
						id));
			}
		}

		private static ICustomFactory GetCustomFactory(Type t, ConfigurationReflectionCache reflectionCache)
		{
			ICustomFactory customFactory = reflectionCache.GetCustomFactory(t);

			return customFactory;
		}
	}
}
