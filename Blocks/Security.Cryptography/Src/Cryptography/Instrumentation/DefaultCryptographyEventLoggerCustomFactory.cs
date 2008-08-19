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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="DefaultCryptographyEventLogger"/> according to instrumentation configuration.
	/// </summary>
	public class DefaultCryptographyEventLoggerCustomFactory : DefaultEventLoggerCustomFactoryBase
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="DefaultCryptographyEventLogger"/>.
		/// </summary>
		/// <param name="instrumentationConfigurationSection">The instrumentation section that is used as configuration.</param>
		/// <returns>A fully initialized instance of <see cref="DefaultCryptographyEventLogger"/>.</returns>
		protected override object DoCreateObject(InstrumentationConfigurationSection instrumentationConfigurationSection)
		{
			return new DefaultCryptographyEventLogger(instrumentationConfigurationSection.EventLoggingEnabled, instrumentationConfigurationSection.WmiEnabled);
		}
	}
}
