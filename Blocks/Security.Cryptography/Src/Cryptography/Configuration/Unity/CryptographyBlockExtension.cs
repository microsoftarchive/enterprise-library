//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Unity
{
	/// <summary>
	/// A <see cref="UnityContainerExtension"/> that registers the policies necessary
	/// to create <see cref="IHashProvider"/> and <see cref="ISymmetricCryptoProvider"/> instances described in the standard
	/// configuration file.
	/// </summary>
	public class CryptographyBlockExtension : EnterpriseLibraryBlockExtension
	{
		/// <summary>
		/// Initialize this extension by adding the Enterprise Library Cryptography Block's policies.
		/// </summary>
		protected override void Initialize()
		{
			CryptographySettings settings
				= (CryptographySettings)ConfigurationSource.GetSection(CryptographyConfigurationView.SectionName);
			if (settings == null)
			{
				return;	// no policies to set up
			}

			CreateProvidersPolicies<IHashProvider, HashProviderData>(
				Context.Policies,
				settings.DefaultHashProviderName,
				settings.HashProviders,
				ConfigurationSource);

			CreateProvidersPolicies<ISymmetricCryptoProvider, SymmetricProviderData>(
				Context.Policies,
				settings.DefaultSymmetricCryptoProviderName,
				settings.SymmetricCryptoProviders,
				ConfigurationSource);
            

            CreateCryptographyManagerPolicies(
                Context.Policies,
                settings);

            Container.RegisterType<CryptographyManager, CryptographyManagerImpl>();
		}

        private void CreateCryptographyManagerPolicies(IPolicyList policies, CryptographySettings settings)
        {
            new PolicyBuilder<CryptographyManagerImpl, CryptographySettings>(
                NamedTypeBuildKey.Make<CryptographyManagerImpl>(),
                settings,
                c => new CryptographyManagerImpl(
                    Resolve.ReferenceDictionary<Dictionary<string, IHashProvider>, IHashProvider, string>(
                        from p in c.HashProviders select new KeyValuePair<string, string>(p.Name, p.Name)),
                    Resolve.ReferenceDictionary<Dictionary<string, ISymmetricCryptoProvider>, ISymmetricCryptoProvider, string>(
                        from p in c.SymmetricCryptoProviders select new KeyValuePair<string,string>(p.Name, p.Name))))
                .AddPoliciesToPolicyList(policies);
        }
	}
}
