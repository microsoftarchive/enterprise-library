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
	/// Implementation of <see cref="IBuilderStrategy"/> which maps null instance names into a different name.
	/// </summary>
	/// <remarks>
	/// The strategy is used to deal with default names.
	/// </remarks>
	/// <seealso cref="ConfigurationNameMapperAttribute"/>
	/// <seealso cref="IConfigurationNameMapper"/>
	public class ConfigurationNameMappingStrategy : EnterpriseLibraryBuilderStrategy
	{
		/// <summary>
		/// Override of <see cref="IBuilderStrategy.PreBuildUp"/>. Updates the instance name using a name mapper associated to type 
		/// to build so later strategies in the build chain will use the updated instance name.
		/// </summary>
		/// <remarks>
		/// Will only update the instance name if it is <see langword="null"/>.
		/// </remarks>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <exception cref="System.Configuration.ConfigurationErrorsException"> when the configuration required to do the mapping is not present or is 
		/// invalid in the configuration source.</exception>
		public override void PreBuildUp(IBuilderContext context)
		{
			base.PreBuildUp(context);

			NamedTypeBuildKey key = (NamedTypeBuildKey) context.BuildKey;

			if (key.Name == null)
			{
				ConfigurationReflectionCache reflectionCache = GetReflectionCache(context);

				IConfigurationNameMapper mapper = reflectionCache.GetConfigurationNameMapper(key.Type);
				if (mapper != null)
				{
					context.BuildKey = new NamedTypeBuildKey(key.Type, mapper.MapName(null, GetConfigurationSource(context)));
				}
			}
		}
	}
}
