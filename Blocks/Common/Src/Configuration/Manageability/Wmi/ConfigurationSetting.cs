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

using System;
using System.Configuration;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Represents a subset of a running application's configuration as an instrumentation instance class.
	/// </summary>
	/// <remarks>
	/// Class <see cref="ConfigurationSetting"/> is the base of the hierarchy of classes that represent configuration
	/// information as WMI objects. It allows generic queries to be written to retrieve all the configuration 
	/// information published for a given application. Properties <see cref="ConfigurationSetting.ApplicationName"/> and
	/// <see cref="ConfigurationSetting.SectionName"/> provide a way to filter the required information to a single
	/// application or configuration section.
	/// </remarks>
	[ManagementEntity]
	public abstract partial class ConfigurationSetting
	{
		private string applicationName;
		private string sectionName;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationSetting"/> class.
		/// </summary>
		protected ConfigurationSetting()
		{ }

		/// <summary>
        /// Initializes a new instance of class <see cref="ConfigurationSetting"/>
		/// </summary>
        /// <param name="sourceElement">The <see cref="ConfigurationElement"/> the <see cref="ConfigurationSetting"/> represents</param>
		protected ConfigurationSetting(ConfigurationElement sourceElement)
		{
			this.SourceElement = sourceElement;
		}

		/// <summary>
		/// Gets the name of the application to which the <see cref="ConfigurationSetting"/> instance represents
		/// configuration information.
		/// </summary>
		[ManagementProbe]
		public virtual string ApplicationName
		{
			get { return applicationName; }
			set { applicationName = value; }
		}

		/// <summary>
		/// Gets the name of the section where the <see cref="ConfigurationSetting"/> instance represented
		/// configuration information resides.
		/// </summary>
		[ManagementProbe]
		public virtual string SectionName
		{
			get { return sectionName; }
			set { sectionName = value; }
		}

		/// <summary>
        /// Makes the setting available for WMI clients.
		/// </summary>
        /// <remarks>Must be overridden by subclasses to perform the actual Publish.</remarks>
		public abstract void Publish();

        /// <summary>
        /// Makes the setting unavailable for WMI clients.
        /// </summary>
        /// <remarks>Must be overridden by subclasses to perform the actual Revoke.</remarks>
		public abstract void Revoke();

		/// <summary>
        /// Called by the WMI infrastructure when changes to properties on the setting have been posted.
        /// Changes the <see cref="ConfigurationSetting.SourceElement"/> and notifies of the changes.
		/// </summary>
		[ManagementCommit]
		public void Commit()
		{
			if (this.SourceElement != null
				&& SaveChanges(this.SourceElement)
				&& this.Changed != null)
			{
				this.Changed(this, new EventArgs());
			}
		}

        /// <summary>
        /// Pushes the current property values to the <see cref="ConfigurationSetting.SourceElement"/>
        /// </summary>
        /// <remarks>Must be overridden by subclasses to perform the actual save.</remarks>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected virtual bool SaveChanges(ConfigurationElement sourceElement)
		{
			return false;// throw new NotImplementedException();
		}

		/// <summary>
        /// The <see cref="ConfigurationElement"/> the <see cref="ConfigurationSetting"/> represents
		/// </summary>
		public ConfigurationElement SourceElement { get; set; }

		/// <summary>
        /// Ocurrs when a <see cref="ConfigurationSetting"/> is changed.
		/// </summary>
		public event EventHandler<EventArgs> Changed;
	}
}