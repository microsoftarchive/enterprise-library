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

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build an instance of <see cref="IHashProvider"/> described by a <see cref="HashProviderData"/> configuration object.
	/// </summary>
	public class HashProviderCustomFactory : AssemblerBasedCustomFactory<IHashProvider, HashProviderData>
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Looks up a specified <see cref="IHashProvider"/>'s configuration from the given <paramref name="configurationSource"/>. 
		/// </summary>
		/// <param name="name">The name of the <see cref="IHashProvider"/> for which the configuration should be looked up.</param>
		/// <param name="configurationSource">The configuration source which should be used.</param>
		/// <returns>The configuration for the specified <see cref="IHashProvider"/>.</returns>
		protected override HashProviderData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			return new CryptographyConfigurationView(configurationSource).GetHashProviderData(name);
		}
	}
}
