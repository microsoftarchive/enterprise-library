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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build an <see cref="ILogFormatter"/> described by a <see cref="FormatterData"/> configuration object.
	/// </summary>
	public class LogFormatterCustomFactory : AssemblerBasedCustomFactory<ILogFormatter, FormatterData>
	{
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public static LogFormatterCustomFactory Instance = new LogFormatterCustomFactory();

		/// <summary>
		/// Returns the configuration object that represents the named <see cref="ILogFormatter"/> instance in the configuration source.
		/// </summary>
		/// <param name="name">The name of the required instance.</param>
		/// <param name="configurationSource">The configuration source where to look for the configuration object.</param>
		/// <returns>The configuration object that represents the instance with name <paramref name="name"/> in the logging 
		/// configuration section from <paramref name="configurationSource"/></returns>
		/// <exception cref="ConfigurationErrorsException"><paramref name="configurationSource"/> does not contain 
		/// logging settings, or the <paramref name="name"/> does not exist in the logging settings.</exception>
		protected override FormatterData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			LoggingSettings settings = LoggingSettings.GetLoggingSettings(configurationSource);
			ValidateSettings(settings);

			FormatterData objectConfiguration = settings.Formatters.Get(name);
			ValidateConfiguration(objectConfiguration, name);

			return objectConfiguration;
		}

		private void ValidateConfiguration(FormatterData objectConfiguration, string name)
		{
			if (objectConfiguration == null)
			{
				throw new ConfigurationErrorsException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionFormatterNotDefined,
						name));
			}
		}

		private void ValidateSettings(LoggingSettings settings)
		{
			if (settings == null)
			{
				throw new ConfigurationErrorsException(Resources.ExceptionLoggingSectionNotFound);
			}
		}
	}
}
