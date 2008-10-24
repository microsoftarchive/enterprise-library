//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information for a 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionTypeData"/> instance.
	/// </summary>
	/// <remarks>
	/// ExceptionTypeData instances are held by 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionPolicyData"/>, but
	/// the wmi objects that represent them related only by matching values for the 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.ExceptionTypeSetting.Policy"/>
	/// property.
	/// </remarks>
	/// <seealso cref="ExceptionPolicySetting"/>
	/// <seealso cref="ExceptionHandlerSetting"/>
	[ManagementEntity]
	public class ExceptionTypeSetting : ConfigurationSetting
	{
		private static readonly PublishedInstancesManager<ExceptionTypeSetting, PublishedInstanceKey>
			publishedInstancesManager = new PublishedInstancesManager<ExceptionTypeSetting, PublishedInstanceKey>();

		private string name;
		private string policy;
		private string exceptionTypeName;
		private string postHandlingAction;

        /// <summary>
        /// Initialize a new instance of the <see cref="ExceptionTypeSetting"/> class with a coniguration source,
        /// the name of the exception type, the type name and the post handling action.
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <param name="name"></param>
        /// <param name="exceptionTypeName"></param>
        /// <param name="postHandlingAction"></param>
		public ExceptionTypeSetting(ConfigurationElement sourceElement,
		                              string name,
		                              string exceptionTypeName,
		                              string postHandlingAction)
			: base(sourceElement)
		{
			this.name = name;
			this.exceptionTypeName = exceptionTypeName;
			this.postHandlingAction = postHandlingAction;
		}

		/// <summary>
		/// Makes the setting available for WMI clients.
		/// </summary>
		public override void Publish()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.policy, this.Name);
			publishedInstancesManager.Publish(this, key);
		}

		/// <summary>
		/// Makes the setting unavailable for WMI clients.
		/// </summary>
		public override void Revoke()
		{
			PublishedInstanceKey key = new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.policy, this.Name);
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

		/// <summary>
		/// Gets the name of the exception type to handle.
		/// </summary>
		[ManagementProbe]
		public string ExceptionTypeName
		{
			get { return exceptionTypeName; }
			set { exceptionTypeName = value; }
		}

		/// <summary>
		/// Gets the name of the policy to which the represented 
		/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionTypeData"/> instance
		/// belongs.
		/// </summary>
		[ManagementKey]
		public string Policy
		{
			get { return policy; }
			set { policy = value; }
		}

		/// <summary>
		/// Gets the name of the value for the for the post handling action.
		/// </summary>
		[ManagementConfiguration]
		public string PostHandlingAction
		{
			get { return postHandlingAction; }
			set { postHandlingAction = value; }
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="ExceptionTypeSetting"/> instances.
		/// </summary>
		[ManagementEnumerator]
		public static IEnumerable<ExceptionTypeSetting> GetInstances()
		{
			return publishedInstancesManager.GetInstances<ExceptionTypeSetting>();
		}

		/// <summary>
		/// Returns the <see cref="ExceptionTypeSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Policy"></param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="ExceptionTypeSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static ExceptionTypeSetting BindInstance(string ApplicationName, string SectionName, string Policy, string Name)
		{
			PublishedInstanceKey key = new PublishedInstanceKey(ApplicationName, SectionName, Policy, Name);
			return publishedInstancesManager.BindInstance<ExceptionTypeSetting>(key);
		}

		/// <summary>
		/// Saves the changes on the <see cref="ExceptionTypeSetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return false; // no changes to save
		}

		private struct PublishedInstanceKey
		{
			private readonly string applicationName;
			private readonly string sectionName;
			private readonly string policy;
			private readonly string name;

			public PublishedInstanceKey(string applicationName, string sectionName, string policy, string name)
			{
				this.applicationName = applicationName;
				this.sectionName = sectionName;
				this.policy = policy;
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
					       && this.sectionName == otherKey.sectionName
					       && this.policy == otherKey.policy;
				}

				return false;
			}
		}
	}
}
