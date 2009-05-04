//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Unity
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to create the container policies required to create a <see cref="FormattedDatabaseTraceListener"/>.
	/// </summary>
	public class FormattedDatabaseTraceListenerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			new PolicyBuilder<FormattedDatabaseTraceListener, FormattedDatabaseTraceListenerData>(
				instanceName,
				(FormattedDatabaseTraceListenerData)configurationObject,
				c => new FormattedDatabaseTraceListener(
					Resolve.Reference<Data.Database>(c.DatabaseInstanceName),
					c.WriteLogStoredProcName,
					c.AddCategoryStoredProcName,
					Resolve.OptionalReference<ILogFormatter>(c.Formatter)))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
