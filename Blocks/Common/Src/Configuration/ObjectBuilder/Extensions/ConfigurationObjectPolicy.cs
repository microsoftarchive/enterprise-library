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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// A configuration object policy that holds a reference to an <see cref="IConfigurationSource"/>.
	/// </summary>
	public class ConfigurationObjectPolicy : IConfigurationObjectPolicy
	{
		private IConfigurationSource configurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationObjectPolicy"/> class with a configuration source.
		/// </summary>
		/// <param name="configurationSource">The configuration source.</param>
		public ConfigurationObjectPolicy(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}

		/// <summary>
		/// Gets the configuration source.
		/// </summary>
		public IConfigurationSource ConfigurationSource
		{
			get { return configurationSource; }
		}
	}
}
