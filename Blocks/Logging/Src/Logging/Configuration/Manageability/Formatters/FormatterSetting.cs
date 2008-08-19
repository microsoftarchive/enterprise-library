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

using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
	/// <summary>
	/// Represents the configuration information for an instance of a concrete subclass of
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.FormatterData"/>.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.FormatterData"/>
	/// <seealso cref="NamedConfigurationSetting"/>
	/// <seealso cref="ConfigurationSetting"/>
	[ManagementEntity]
	public abstract class FormatterSetting : NamedConfigurationSetting
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FormatterSetting"/> class.
		/// </summary>
		protected FormatterSetting(FormatterData sourceElement, string name)
			: base(sourceElement, name)
		{ }
	}
}
