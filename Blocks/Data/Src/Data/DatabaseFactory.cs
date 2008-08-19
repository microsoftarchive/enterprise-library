//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	/// <summary>
	/// Contains factory methods for creating Database objects.
	/// </summary>
	public static class DatabaseFactory
	{
		/// <summary>
		/// Method for invoking a default Database object. Reads default settings
		/// from the ConnectionSettings.config file.
		/// </summary>
		/// <example>
		/// <code>
		/// Database dbSvc = DatabaseFactory.CreateDatabase();
		/// </code>
		/// </example>
		/// <returns>Database</returns>
		/// <exception cref="System.Configuration.ConfigurationException">
		/// <para>An error occured while reading the configuration.</para>
		/// </exception>
		public static Database CreateDatabase()
		{
            try
            {
				DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
                return factory.CreateDefault();
            }
            catch (ConfigurationErrorsException configurationException)
            {
                TryLogConfigurationError(configurationException, "default");

                throw;
            }
		}

		/// <summary>
		/// Method for invoking a specified Database service object.  Reads service settings
		/// from the ConnectionSettings.config file.
		/// </summary>
		/// <example>
		/// <code>
		/// Database dbSvc = DatabaseFactory.CreateDatabase("SQL_Customers");
		/// </code>
		/// </example>
		/// <param name="name">configuration key for database service</param>
		/// <returns>Database</returns>
		/// <exception cref="System.Configuration.ConfigurationException">
		/// <para><paramref name="instanceName"/> is not defined in configuration.</para>
		/// <para>- or -</para>
		/// <para>An error exists in the configuration.</para>
		/// <para>- or -</para>
		/// <para>An error occured while reading the configuration.</para>        
		/// </exception>
		/// <exception cref="System.Reflection.TargetInvocationException">
		/// <para>The constructor being called throws an exception.</para>
		/// </exception>
		public static Database CreateDatabase(string name)
		{
            try
            {
				DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
				return factory.Create(name);
			}
            catch (ConfigurationErrorsException configurationException)
            {
                TryLogConfigurationError(configurationException, name);

                throw;
            }
		}

        private static void TryLogConfigurationError(ConfigurationErrorsException configurationException, string instanceName)
        {
            try
            {
                DefaultDataEventLogger eventLogger = EnterpriseLibraryFactory.BuildUp<DefaultDataEventLogger>();
                if (eventLogger != null)
                {
                    eventLogger.LogConfigurationError(configurationException, instanceName);
                }
            }
            catch { }
		}
	}

}