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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration
{
	/// <summary>
	/// Configuration section for the MsmqLogDistributor service.
	/// </summary>
	public class MsmqDistributorSettings : SerializableConfigurationSection
	{
		private const string msmqPathProperty = "msmqPath";
		private const string queueTimerIntervalProperty = "queueTimerInterval";
		private const string serviceNameProperty = "serviceName";

		/// <summary>
		/// Retrieves the instance of <see cref="MsmqDistributorSettings"/> from the <see cref="IConfigurationSource"/>.
		/// </summary>
		/// <param name="configurationSource">The <see cref="IConfigurationSource"/> to get the section from.</param>
		/// <returns>The section, if exists in the configuration source.</returns>
		public static MsmqDistributorSettings GetSettings(IConfigurationSource configurationSource)
		{
            if(configurationSource == null) throw new ArgumentNullException("configurationSource");
			return configurationSource.GetSection(SectionName) as MsmqDistributorSettings;
		}

		/// <summary>
		/// The path to the msmq to use.
		/// </summary>
		[ConfigurationProperty(msmqPathProperty, IsRequired= true)]
		public string MsmqPath
		{
			get { return (string)base[msmqPathProperty]; }
			set { base[msmqPathProperty] = value; }
		}

		/// <summary>
		/// The poll interval for getting messages from the queue.
		/// </summary>
		[ConfigurationProperty(queueTimerIntervalProperty, IsRequired= false)]
		public int QueueTimerInterval
		{
			get { return (int)base[queueTimerIntervalProperty]; }
			set { base[queueTimerIntervalProperty] = value; }
		}

		/// <summary>
		/// The name to use in the service.
		/// </summary>
		[ConfigurationProperty(serviceNameProperty, IsRequired= true)]
		public string ServiceName
		{
			get { return (string)base[serviceNameProperty]; }
			set { base[serviceNameProperty] = value; }
		}

		/// <summary>
		/// The name of the configuration section.
		/// </summary>
		public static string SectionName
		{
			get { return "msmqDistributorSettings"; }
		}
	}
}
