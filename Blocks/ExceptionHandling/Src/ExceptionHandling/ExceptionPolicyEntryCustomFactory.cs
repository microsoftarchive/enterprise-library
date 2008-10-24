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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the general process to build an <see cref="ExceptionPolicyEntry"/> object given a instance of <see cref="ExceptionTypeData"/>.
    /// </summary>
	public class ExceptionPolicyEntryCustomFactory
	{
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
		public static ExceptionPolicyEntryCustomFactory Instance = new ExceptionPolicyEntryCustomFactory();

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="ExceptionPolicyEntry"/> based on an instance of <see cref="ExceptionTypeData"/>.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="ExceptionPolicyEntry"/>.</returns>
		public ExceptionPolicyEntry Create(IBuilderContext context, ExceptionTypeData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			List<IExceptionHandler> handlers = new List<IExceptionHandler>();
			foreach (ExceptionHandlerData handlerData in objectConfiguration.ExceptionHandlers)
			{
				IExceptionHandler handler
					= ExceptionHandlerCustomFactory.Instance.Create(context, handlerData, configurationSource, reflectionCache);
				handlers.Add(handler);
			}

			ExceptionPolicyEntry createdObject
				= new ExceptionPolicyEntry(
					objectConfiguration.PostHandlingAction,
					handlers);

			return createdObject;
		}
	}
}
