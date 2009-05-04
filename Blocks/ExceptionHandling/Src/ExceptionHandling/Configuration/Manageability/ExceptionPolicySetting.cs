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
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionPolicyData"/> instance.
	/// </summary>
	/// <remarks>
	/// ExceptionPolicyData do not have any properties of their own that are not collections of other configuration objects.
	/// Instances of <see cref="ExceptionPolicySetting"/> are still published for the sake of completeness.
	/// </remarks>
	/// <seealso cref="ExceptionTypeSetting"/>
	/// <seealso cref="ExceptionHandlerSetting"/>
	[ManagementEntity]
	public class ExceptionPolicySetting : NamedConfigurationSetting
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="ExceptionPolicySetting"/> class with a configuration element and the name of the policy.
        /// </summary>
        /// <param name="sourceElement">The configuration element for the policy.</param>
        /// <param name="name">The name of the policy</param>
		public ExceptionPolicySetting(ConfigurationElement sourceElement, string name)
			: base(sourceElement, name)
		{
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="ExceptionPolicySetting"/> instances.
		/// </summary>
		[ManagementEnumerator]
		public static IEnumerable<ExceptionPolicySetting> GetInstances()
		{
			return GetInstances<ExceptionPolicySetting>();
		}

		/// <summary>
		/// Returns the <see cref="ExceptionPolicySetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="ExceptionPolicySetting"/> instance specified by the values for
		/// the key properties, or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static ExceptionPolicySetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return BindInstance<ExceptionPolicySetting>(ApplicationName, SectionName, Name);
		}

		/// <summary>
		/// Saves the changes on the <see cref="ExceptionPolicySetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return false; // no changes to save
		}
	}
}
