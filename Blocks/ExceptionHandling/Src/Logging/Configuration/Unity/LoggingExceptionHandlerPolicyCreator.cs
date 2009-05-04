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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Unity
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to create the container policies required to create a <see cref="LoggingExceptionHandler"/>.
	/// </summary>
	public class LoggingExceptionHandlerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			LoggingExceptionHandlerData castConfigurationObject = (LoggingExceptionHandlerData)configurationObject;

			// the setting for "use the default logger" is ignored - it makes no sense when using the container.
			new PolicyBuilder<LoggingExceptionHandler, LoggingExceptionHandlerData>(
				NamedTypeBuildKey.Make<LoggingExceptionHandler>(instanceName),
				castConfigurationObject,
				c => new LoggingExceptionHandler(
						castConfigurationObject.LogCategory,
						castConfigurationObject.EventId,
						castConfigurationObject.Severity,
						castConfigurationObject.Title,
						castConfigurationObject.Priority,
						castConfigurationObject.FormatterType,
						Resolve.Reference<LogWriter>(null)))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
