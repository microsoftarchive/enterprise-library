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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Unity
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to create the container policies required to create a <see cref="SymmetricStorageEncryptionProvider"/>.
	/// </summary>
	public class SymmetricStorageEncryptionProviderPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
            SymmetricStorageEncryptionProviderData castConfigurationObject = (SymmetricStorageEncryptionProviderData)configurationObject;

            new PolicyBuilder<SymmetricStorageEncryptionProvider, SymmetricStorageEncryptionProviderData>(
                    instanceName,
                    castConfigurationObject,
                    c => new SymmetricStorageEncryptionProvider(Resolve.Reference<ISymmetricCryptoProvider>(c.SymmetricInstance))
                    )
                    .AddPoliciesToPolicyList(policyList);
		}
	}
}
