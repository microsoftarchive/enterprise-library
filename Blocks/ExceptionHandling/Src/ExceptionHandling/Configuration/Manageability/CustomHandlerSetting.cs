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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information from a 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.CustomHandlerData"/> instance.
	/// </summary>
	[ManagementEntity]
	public class CustomHandlerSetting : ExceptionHandlerSetting
	{
		private string handlerType;
		private string[] attributes;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomHandlerSetting"/> class with a configuration element, the name of the handler, 
        /// the filter type, and the attributes for the handler.
        /// </summary>
        /// <param name="sourceElement">The configuration element.</param>
        /// <param name="name">The name of the handler.</param>
        /// <param name="filterType">The filter type.</param>
        /// <param name="attributes">The attributes for the handler.</param>
		public CustomHandlerSetting(ConfigurationElement sourceElement, string name, string filterType, string[] attributes)
			: base(sourceElement, name)
		{
			this.handlerType = filterType;
			this.attributes = attributes;
		}

		/// <summary>
		/// Gets the name of the type for the custom exception handler.
		/// </summary>
		[ManagementConfiguration]
		public string HandlerType
		{
			get { return handlerType; }
			set { handlerType = value; }
		}

		/// <summary>
		/// Gets the collection of attributes for the custom exception handler represented as a 
		/// <see cref="string"/> array of key/value pairs.
		/// </summary>
		[ManagementConfiguration]
		public string[] Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="CustomHandlerSetting"/> instances.
		/// </summary>
		/// <returns></returns>
		[ManagementEnumerator]
		public static IEnumerable<CustomHandlerSetting> GetInstances()
		{
			return GetInstances<CustomHandlerSetting>();
		}

		/// <summary>
		/// Returns the <see cref="CustomHandlerSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Policy"></param>
		/// <param name="ExceptionType"></param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="CustomHandlerSetting"/> instance specified by the values for the key properties, or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static CustomHandlerSetting BindInstance(string ApplicationName,
		                                                string SectionName,
		                                                string Policy,
		                                                string ExceptionType,
		                                                string Name)
		{
			return BindInstance<CustomHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}

		/// <summary>
		/// Saves the changes on the <see cref="CustomHandlerSetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CustomExceptionHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}