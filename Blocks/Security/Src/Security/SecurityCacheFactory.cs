//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
	/// <summary>
	/// Static factory class used to get instances of a specified ISecurityCacheProvider
	/// </summary>
	public static class SecurityCacheFactory
	{
		private static SecurityCacheProviderFactory factory = new SecurityCacheProviderFactory(ConfigurationSourceFactory.Create());

		/// <summary>
		/// Returns the default ISecurityCacheProvider instance. 
		/// Guaranteed to return an intialized ISecurityCacheProvider if no exception thrown
		/// </summary>
		/// <returns>Default SecurityCache provider instance.</returns>
        /// <exception cref="ConfigurationException">Unable to create default <see cref="ISecurityCacheProvider"/></exception>
		public static ISecurityCacheProvider GetSecurityCacheProvider()
		{
			try
			{
				return factory.CreateDefault();
			}
			catch (ConfigurationErrorsException configurationException)
			{
				TryLogConfigurationError(configurationException, "default");

				throw;
			}
		}

		/// <summary>
		/// Returns the named ISecurityCacheProvider instance. Guaranteed to return an initialized ISecurityCacheProvider if no exception thrown.
		/// </summary>
		/// <param name="securityCacheProviderName">Name defined in configuration for the SecurityCache provider to instantiate</param>
		/// <returns>Named SecurityCache provider instance</returns>
		/// <exception cref="ArgumentNullException">providerName is null</exception>
		/// <exception cref="ArgumentException">providerName is empty</exception>
		/// <exception cref="ConfigurationException">Could not find instance specified in providerName</exception>
		/// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
		public static ISecurityCacheProvider GetSecurityCacheProvider(string securityCacheProviderName)
		{
			try
			{
				return factory.Create(securityCacheProviderName);
			}
			catch (ConfigurationErrorsException configurationException)
			{
				TryLogConfigurationError(configurationException, "default");

				throw;
			}
		}

		private static void TryLogConfigurationError(ConfigurationErrorsException configurationException, string instanceName)
		{
			try
			{
				DefaultSecurityEventLogger eventLogger = EnterpriseLibraryFactory.BuildUp<DefaultSecurityEventLogger>();
				if (eventLogger != null)
				{
					eventLogger.LogConfigurationError(instanceName, Resources.ErrorSecurityCacheConfigurationFailedMessage, configurationException);
				}
			}
			catch { }
		}
	}
}