//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	/// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build an <see cref="ExceptionPolicyImpl"/> described by a <see cref="ExceptionPolicyData"/> configuration object.
    /// </summary>
	public class ExceptionPolicyCustomFactory : ICustomFactory
	{
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="ExceptionPolicyImpl"/> based on an instance of <see cref="ExceptionPolicyData"/>.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="name">The name of the <see cref="ExceptionPolicyImpl"/> that should be created.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="ExceptionPolicyImpl"/>.</returns>
        public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ExceptionPolicyData objectConfiguration = GetConfiguration(name, configurationSource);

			Dictionary<Type, ExceptionPolicyEntry> policyEntries = new Dictionary<Type,ExceptionPolicyEntry>();
			foreach (ExceptionTypeData exceptionTypeData in objectConfiguration.ExceptionTypes)
			{
				ExceptionPolicyEntry entry
					= ExceptionPolicyEntryCustomFactory.Instance.Create(context, exceptionTypeData, configurationSource, reflectionCache);

				policyEntries.Add(exceptionTypeData.Type, entry);
			}

			ExceptionPolicyImpl createdObject
				= new ExceptionPolicyImpl(
					objectConfiguration.Name,
					policyEntries);

			return createdObject;
		}

        /// <summary>
        /// Returns the configuration object that represents the named <see cref="ExceptionPolicyImpl"/> instance in the configuration source.
        /// </summary>
        /// <param name="id">The name of the required instance.</param>
        /// <param name="configurationSource">The configuration source where to look for the configuration object.</param>
        /// <returns>The configuration object that represents the instance with name <paramref name="name"/> in the logging 
        /// configuration section from <paramref name="configurationSource"/></returns>
        private ExceptionPolicyData GetConfiguration(string id, IConfigurationSource configurationSource)
		{
			return new ExceptionHandlingConfigurationView(configurationSource).GetExceptionPolicyData(id);
		}

	}
}
