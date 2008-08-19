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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Factory to create <see cref="LogWriter"/> instances.
	/// </summary>
	public class LogWriterFactory
	{
		private IConfigurationSource configurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogWriter"/> class with the default <see cref="IConfigurationSource"/> instance.
		/// </summary>
		public LogWriterFactory()
			: this(ConfigurationSourceFactory.Create())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogWriter"/> class with a <see cref="IConfigurationSource"/> instance.
		/// </summary>
		/// <param name="configurationSource">The source for configuration information.</param>
		public LogWriterFactory(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}

		/// <summary>
		/// Creates a new instance of <see cref="LogWriter"/> based on the configuration in the <see cref="IConfigurationSource"/> 
		/// instance of the factory.
		/// </summary>
		/// <returns>The created <see cref="LogWriter"/> object.</returns>
		public LogWriter Create()
		{
			return EnterpriseLibraryFactory.BuildUp<LogWriter>(configurationSource);
		}
	}
}
