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

using System;
using System.Management.Instrumentation;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
	/// <summary>
	/// Represents the configuration information for an instance of a concrete subclass of
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LogFilterData"/>.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.LogFilterData"/>
	/// <seealso cref="NamedConfigurationSetting"/>
	/// <seealso cref="ConfigurationSetting"/>
	[ManagementEntity]
	public abstract partial class LogFilterSetting : NamedConfigurationSetting
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogFilterSetting"/> class.
		/// </summary>
		protected LogFilterSetting(LogFilterData sourceElement, string name)
			: base(sourceElement, name)
		{ }
	}
}