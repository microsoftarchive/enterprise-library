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

using System.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Encapsulates the logic to create the policies required to create instances of a type.
	/// </summary>
	/// <remarks>
	/// This encapsulated logic is used when the policies for a provider of an unknown type must be created.
	/// </remarks>
	public interface IContainerPolicyCreator
	{
		/// <summary>
		/// Adds the policies required to create the provider specified by <paramref name="configurationObject"/>
		/// to <paramref name="policyList"/>.
		/// </summary>
		/// <param name="policyList">The <see cref="IPolicyList"/> to which the policies must be added.</param>
		/// <param name="instanceName">The name to use for the instance's policies.</param>
		/// <param name="configurationObject">The <see cref="ConfigurationElement"/> that describes a provider.</param>
		/// <param name="configurationSource">The <see cref="IConfigurationSource"/> from which additional configuration information
		/// should be retrieved if necessary.</param>
		void CreatePolicies(IPolicyList policyList,
							string instanceName,
							ConfigurationElement configurationObject,
		                    IConfigurationSource configurationSource);
	}
}