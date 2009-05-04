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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Unity
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to create the container policies required to create a <see cref="FaultContractExceptionHandler"/>.
	/// </summary>
	public class FaultContractExceptionHandlerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			FaultContractExceptionHandlerData castConfigurationObject = (FaultContractExceptionHandlerData)configurationObject;

			new PolicyBuilder<FaultContractExceptionHandler, FaultContractExceptionHandlerData>(
				NamedTypeBuildKey.Make<FaultContractExceptionHandler>(instanceName),
				castConfigurationObject,
				c => new FaultContractExceptionHandler(
					Type.GetType(castConfigurationObject.FaultContractType),
					castConfigurationObject.ExceptionMessage,
					castConfigurationObject.Attributes))
				.AddPoliciesToPolicyList(policyList);
		}
	}
}
