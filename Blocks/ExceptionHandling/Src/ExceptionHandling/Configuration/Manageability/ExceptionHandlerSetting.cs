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
	/// Represents the configuration information for an instance of a concrete subclass of
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionHandlerData"/>.
	/// </summary>
	/// <remarks>
	/// There way to relate the objects representing an <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionTypeData"/> instance
	/// and the <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionHandlerData"/> instances in its
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionTypeData.ExceptionHandlers"/> collection is through
	/// the <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.ExceptionHandlerSetting.ExceptionType"/> and
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.ExceptionHandlerSetting.Policy"/> properties. Also,
	/// the order of the handlers in the Handlers collection is represented with the 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.ExceptionHandlerSetting.Order"/> property as the order 
	/// of the handlers is relevant to the exception handling process.
	/// </remarks>
	/// <seealso cref="ExceptionTypeSetting"/>
	/// <seealso cref="ExceptionPolicySetting"/>
	/// <seealso cref="WrapHandlerSetting"/>
	/// <seealso cref="ReplaceHandlerSetting"/>
	/// <seealso cref="CustomHandlerSetting"/>
	[ManagementEntity]
	public abstract class ExceptionHandlerSetting : ConfigurationSetting
	{
		private static readonly PublishedInstancesManager<ExceptionHandlerSetting, PublishedInstanceKey>
			publishedInstancesManager = new PublishedInstancesManager<ExceptionHandlerSetting, PublishedInstanceKey>();

		private string name;
		private string policy;
		private string exceptionType;
		private int order;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionHandlerSetting"/> class.
		/// </summary>
		/// <param name="name">The name of the exception handler.</param>
		/// <param name="sourceElement"></param>
		protected ExceptionHandlerSetting(ConfigurationElement sourceElement, string name)
			: base(sourceElement)
		{
			this.name = name;
		}

		/// <summary>
		/// Makes the setting available for WMI clients.
		/// </summary>
		public override void Publish()
		{
			PublishedInstanceKey key =
				new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.policy, this.exceptionType, this.name);
			publishedInstancesManager.Publish(this, key);
		}

		/// <summary>
		/// Makes the setting unavailable for WMI clients.
		/// </summary>
		public override void Revoke()
		{
			PublishedInstanceKey key =
				new PublishedInstanceKey(this.ApplicationName, this.SectionName, this.policy, this.exceptionType, this.name);
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
			where T : ExceptionHandlerSetting
		{
			return publishedInstancesManager.GetInstances<T>();
		}

		/// <summary>
		/// Returns the <see cref="NamedConfigurationSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="applicationName">The value for the ApplicationName key property.</param>
		/// <param name="sectionName">The value for the SectionName key property.</param>
		/// <param name="policy"></param>
		/// <param name="exceptionType"></param>
		/// <param name="name">The value for the Name key property.</param>
		/// <returns>The published <see cref="NamedConfigurationSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		public static T BindInstance<T>(string applicationName,
		                                string sectionName,
		                                string policy,
		                                string exceptionType,
		                                string name)
			where T : ExceptionHandlerSetting
		{
			PublishedInstanceKey key = new PublishedInstanceKey(applicationName, sectionName, policy, exceptionType, name);
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

		/// <summary>
		/// Gets the type of the exception to which the handler is registered.
		/// </summary>
		/// <remarks>
		/// This property is used toghether with <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.ExceptionHandlerSetting.Policy"/>
		/// to relate the objects representing exception policies, exception types and exception handlers.
		/// </remarks>
		[ManagementKey]
		public string ExceptionType
		{
			get { return exceptionType; }
			set { exceptionType = value; }
		}

		/// <summary>
		/// Gets the name of the policy to which the handler is registered.
		/// </summary>
		/// <remarks>
		/// This property is used toghether with <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.ExceptionHandlerSetting.ExceptionType"/>
		/// to relate the objects representing exception policies, exception types and exception handlers.
		/// </remarks>
		[ManagementKey]
		public string Policy
		{
			get { return policy; }
			set { policy = value; }
		}

		/// <summary>
		/// Gets the order for the represented hadler among the collection of handlers registered to hande the 
		/// same exception type in the same exception policy.
		/// </summary>
		[ManagementProbe]
		public int Order
		{
			get { return order; }
			set { order = value; }
		}

		private struct PublishedInstanceKey
		{
			private readonly string applicationName;
			private readonly string sectionName;
			private readonly string policy;
			private readonly string exceptionType;
			private readonly string name;

			public PublishedInstanceKey(string applicationName,
			                            string sectionName,
			                            string policy,
			                            string exceptionType,
			                            string name)
			{
				this.applicationName = applicationName;
				this.sectionName = sectionName;
				this.policy = policy;
				this.exceptionType = exceptionType;
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
					       && this.policy == otherKey.policy
					       && this.exceptionType == otherKey.exceptionType;
				}

				return false;
			}
		}
	}
}