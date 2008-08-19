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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Resolves default names for symmetric crypto providers.
	/// </summary>
	public class SymmetricCryptoProviderNameMapper : IConfigurationNameMapper
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns the default symmetric crypto provider name from the configuration in the <paramref name="configSource"/>, if the
		/// value for <paramref name="name"/> is <see langword="null"/> (<b>Nothing</b> in Visual Basic).
		/// </summary>
		/// <param name="name">The current name.</param>
		/// <param name="configSource">The source for configuration information.</param>
		/// <returns>The default symmetric crypto provider name if <paramref name="name"/> is <see langword="null"/> (<b>Nothing</b> in Visual Basic),
		/// otherwise the original value for <b>name</b>.</returns>
		public string MapName(string name, IConfigurationSource configSource)
		{
			if (name != null) return name;

			return new CryptographyConfigurationView(configSource).DefaultSymmetricCryptoProviderName;
		}
	}
}
