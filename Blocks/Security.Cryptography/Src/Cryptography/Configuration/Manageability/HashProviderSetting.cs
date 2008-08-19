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

using System.Management.Instrumentation;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information for an instance of a concrete subclass of
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashProviderData"/>.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.HashProviderData"/>
	/// <seealso cref="NamedConfigurationSetting"/>
	/// <seealso cref="ConfigurationSetting"/>
	[ManagementEntity]
	public abstract partial class HashProviderSetting : NamedConfigurationSetting
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HashProviderSetting"/> class.
		/// </summary>
		protected HashProviderSetting(HashProviderData sourceElement, string name)
			: base(sourceElement, name)
		{ }
	}
}
