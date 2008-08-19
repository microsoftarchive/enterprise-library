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

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// 
	/// </summary>
	[ManagementEntity]
	public abstract class ConfigurationSectionSetting : ConfigurationSetting
	{
		private static readonly PublishedInstancesManager<ConfigurationSectionSetting, PublishedInstanceKey>
			publishedInstancesManager = new PublishedInstancesManager<ConfigurationSectionSetting, PublishedInstanceKey>();

		/// <summary>
		/// Initializes a new instance of class <see cref="ConfigurationSectionSetting"/>
		/// </summary>
		protected ConfigurationSectionSetting()
		{
		}

		/// <summary>
		/// Initializes a new instance of class <see cref="ConfigurationSectionSetting"/>
		/// </summary>
		/// <param name="sourceElement">The <see cref="ConfigurationElement"/> the <see cref="ConfigurationSectionSetting"/> represents</param>
		protected ConfigurationSectionSetting(ConfigurationElement sourceElement)
			: base(sourceElement)
		{
		}

		/// <summary>
		/// Makes the setting available for WMI clients.
		/// </summary>
		public override void Publish()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName);
			publishedInstancesManager.Publish(this, key);
		}

		/// <summary>
		/// Makes the setting unavailable for WMI clients.
		/// </summary>
		public override void Revoke()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName);
			publishedInstancesManager.Revoke(this, key);
		}

		/// <summary>
		/// Clear collection of published instances.
		/// </summary>
		public static void ClearPublishedInstances()
		{
			publishedInstancesManager.ClearPublishedInstances();
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="ConfigurationSectionSetting"/> instances.
		/// </summary>
		/// <typeparam name="T">A valid <see cref="ConfigurationSectionSetting"/> class.</typeparam>
		/// <returns><see cref="ConfigurationSectionSetting"/></returns>
		protected static IEnumerable<T> GetInstances<T>()
			where T : ConfigurationSectionSetting
		{
			return publishedInstancesManager.GetInstances<T>();
		}

		/// <summary>
		/// Returns the <see cref="ConfigurationSectionSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="applicationName">The value for the ApplicationName key property.</param>
		/// <param name="sectionName">The value for the SectionName key property.</param>
		/// <returns>The published <see cref="ConfigurationSectionSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		protected static T BindInstance<T>(string applicationName, string sectionName)
			where T : ConfigurationSectionSetting
		{
			PublishedInstanceKey key = new PublishedInstanceKey(applicationName, sectionName);
			return publishedInstancesManager.BindInstance<T>(key);
		}

		/// <summary>
		/// Gets the name that identifies the represented configuration information.
		/// </summary>
		[ManagementKey]
		[ManagementQualifier("Override", Value = "ApplicationName")]
		public override string ApplicationName
		{
			get { return base.ApplicationName; }
			set { base.ApplicationName = value; }
		}

		/// <summary>
		/// Get and set the application name.
		/// </summary>
		[ManagementKey]
		[ManagementQualifier("Override", Value = "SectionName")]
		public override string SectionName
		{
			get { return base.SectionName; }
			set { base.SectionName = value; }
		}

		private struct PublishedInstanceKey
		{
			private readonly string applicationName;
			private readonly string sectionName;

			public PublishedInstanceKey(string applicationName, string sectionName)
			{
				this.applicationName = applicationName;
				this.sectionName = sectionName;
			}

			public override int GetHashCode()
			{
				return this.sectionName != null ? this.sectionName.GetHashCode() : 0;
			}

			public override bool Equals(object obj)
			{
				if (obj is PublishedInstanceKey)
				{
					PublishedInstanceKey otherKey = (PublishedInstanceKey) obj;
					return this.applicationName == otherKey.applicationName
					       && this.sectionName == otherKey.sectionName;
				}

				return false;
			}
		}
	}
}