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
	/// Represents a subset of a running application's configuration identifiable by name
	/// as an instrumentation instance class.
	/// </summary>
	/// <remarks>
	/// Class <see cref="NamedConfigurationSetting"/> instances usually represent configuration information
	/// residing in instances of a subclass of <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.NamedConfigurationElement"/>.
	/// </remarks>
	[ManagementEntity]
	public abstract class NamedConfigurationSetting : ConfigurationSetting
	{
		private static readonly PublishedInstancesManager<NamedConfigurationSetting, PublishedInstanceKey>
			publishedInstancesManager = new PublishedInstancesManager<NamedConfigurationSetting, PublishedInstanceKey>();

		private string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationSetting"/> class with 
		/// the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name that identifies the represented configuration information</param>
		protected NamedConfigurationSetting(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationSetting"/> class with 
		/// the given <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name that identifies the represented configuration information</param>
		/// <param name="sourceElement"></param>
		protected NamedConfigurationSetting(ConfigurationElement sourceElement, string name)
			: base(sourceElement)
		{
			this.Name = name;
		}

		/// <summary>
		/// Makes the setting available for WMI clients.
		/// </summary>
		public override void Publish()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.Name);
			publishedInstancesManager.Publish(this, key);
		}

		/// <summary>
		/// Makes the setting unavailable for WMI clients.
		/// </summary>
		public override void Revoke()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.Name);
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
		/// <typeparam name="T">A valid <see cref="NamedConfigurationSetting"/> class.</typeparam>
		/// <returns><see cref="NamedConfigurationSetting"/></returns>
		protected static IEnumerable<T> GetInstances<T>()
			where T : NamedConfigurationSetting
		{
			return publishedInstancesManager.GetInstances<T>();
		}

		/// <summary>
		/// Returns the <see cref="NamedConfigurationSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="applicationName">The value for the ApplicationName key property.</param>
		/// <param name="sectionName">The value for the SectionName key property.</param>
		/// <param name="name">The value for the Name key property.</param>
		/// <returns>The published <see cref="NamedConfigurationSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		public static T BindInstance<T>(string applicationName, string sectionName, string name)
			where T : NamedConfigurationSetting
		{
			PublishedInstanceKey key = new PublishedInstanceKey(applicationName, sectionName, name);
			return publishedInstancesManager.BindInstance<T>(key);
		}

		/// <summary>
		/// Gets the name that identifies the represented configuration information.
		/// </summary>
		[ManagementKey]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// Get and set the application name.
		/// </summary>
		[ManagementKey]
		[ManagementQualifier("Override", Value = "ApplicationName")]
		public override string ApplicationName
		{
			get { return base.ApplicationName; }
			set { base.ApplicationName = value; }
		}

		/// <summary>
		/// Get and set the section name.
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
			private readonly string name;

			public PublishedInstanceKey(string applicationName, string sectionName, string name)
			{
				this.applicationName = applicationName;
				this.sectionName = sectionName;
				this.name = name;
			}

			public override int GetHashCode()
			{
				return this.name != null ? this.name.GetHashCode() : 0;
			}

			public override bool Equals(object obj)
			{
				if (obj is PublishedInstanceKey)
				{
					PublishedInstanceKey otherKey = (PublishedInstanceKey) obj;
					return this.name == otherKey.name
					       && this.applicationName == otherKey.applicationName
					       && this.sectionName == otherKey.sectionName;
				}

				return false;
			}
		}
	}
}
